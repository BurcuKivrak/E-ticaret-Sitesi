using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using OrnekSite.Entity;
using OrnekSite.Identity;
using OrnekSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrnekSite.Controllers
{
    public class AccountController : Controller
    {
        DataContext db = new DataContext();

        private UserManager<ApplicationUser> UserManager;
        private RoleManager<ApplicationRole> RoleManager;
        public AccountController()
        {
            var userStore = new UserStore<ApplicationUser>(new IdentityDataContext());
            UserManager = new UserManager<ApplicationUser>(userStore);

            var roleStore = new RoleStore<ApplicationRole>(new IdentityDataContext());
            RoleManager = new RoleManager<ApplicationRole>(roleStore);
        }
        // GET: Account

        public ActionResult ChangePassword()
        {

            return View();
        }
        [HttpPost]
        [Authorize]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if(ModelState.IsValid)
            {
                var result = UserManager.ChangePassword(User.Identity.GetUserId(), model.oldPassword, model.NewPassword);
                return View("Update");
            }
            return View(model);
        }

        public PartialViewResult UserCount()
        {
            var u = UserManager.Users;
            return PartialView(u);
        }

        public ActionResult UserList()
        {
            var u = UserManager.Users;
            return View(u);
        }

        public ActionResult UserProfile()
        {
            var id = HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId();
            var user = UserManager.FindById(id);
            var data = new UserProfile()
            {
                id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                Username = user.UserName
                
            };
            return View(data);
        }
        [HttpPost]
        public ActionResult UserProfile(UserProfile model)
        {
            var user = UserManager.FindById(model.id);
            user.Name = model.Name;
            user.Surname = model.Surname;
            user.UserName = model.Username;
            user.Email = model.Email;
            UserManager.Update(user);
            return View("Update");
        }
        
        public ActionResult Login()
        {
            return View();
        }


       public ActionResult LogOut()
        {
            var authManager = HttpContext.GetOwinContext().Authentication;
            authManager.SignOut(); // kullanici cikartildi
            return RedirectToAction("Index", "Home"); // Cikis yaptiktan sonra gidecegi yer
        }
        [HttpPost]
        public ActionResult Login(Login model, string returnUrl)
        {
            if(ModelState.IsValid)
            {
                var user = UserManager.Find(model.Username, model.Password); //kullanıcıdan alınan ad  ve sifre ile arama yapacak.

                if(user!=null)
                {
                    var authManager = HttpContext.GetOwinContext().Authentication; // owin packeges dahil edildi. Eşleşme olursa httpcontext metotu cagırılır.
                    var Identityclaims = UserManager.CreateIdentity(user, "ApplicationCookie"); //olusturulan user cookie icinde atılır.
                    var authProperties = new AuthenticationProperties();
                    authProperties.IsPersistent = model.RememberMe; // section kısmının sürekli açık olmasını sağlar ya da kapnmasını belirler. oturum durumunu kontrol eder.
                    authManager.SignIn(authProperties,Identityclaims); // Giris islemi tamamlandı
                    if(!string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home");

                }

                else
                {
                    ModelState.AddModelError("LoginUserError", "Böyle Bir Kullanıcı Mevcut Değildir !");
                }
            }

            return View(model);
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(Register model)
        {
            if(ModelState.IsValid)
            {
                var user = new ApplicationUser();
                user.Name = model.Name; ;
                user.Surname = model.Surname;
                user.Email = model.Email;
                user.UserName = model.Username;
                var result = UserManager.Create(user, model.Password);
                if(result.Succeeded)
                {
                    if(RoleManager.RoleExists("user"))
                    {
                        UserManager.AddToRole(user.Id, "user");
                    }
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    ModelState.AddModelError("RegisterError", "Kullanıcı Oluşturma Hatası..");
                }
            }
            return View(model);
        }
        public ActionResult Index()
        {
            var username = User.Identity.Name;
            var Orders = db.Orders.Where(i => i.UserName == username).Select(i => new UserOrder
            {
                Id = i.Id,
                OrderNumber = i.OrderNumber,
                OrderState = i.OrderState,
                OrderDate = i.OrderDate,
                Total = i.Total
            }).OrderByDescending(i => i.OrderDate).ToList();
            return View(Orders);
        }

        public ActionResult Details(int id)
        {
            var model = db.Orders.Where(i => i.Id == id).Select(i => new OrderDetails()
            {
                OrderId = i.Id,
                OrderNumber = i.OrderNumber,
                Total = i.Total,
                OrderDate = i.OrderDate,
                OrderState = i.OrderState,
                Adres = i.Adres,
                Sehir = i.Sehir,
                Semt = i.Semt,
                Mahalle = i.Mahalle,
                PostaKodu = i.PostaKodu,
                OrderLines = i.OrderLines.Select(x => new OrderLineModel()
                {
                    ProductId = x.ProductId,
                    Image = x.Product.image,
                    ProductName = x.Product.Name,
                    Quantity = x.Quantity,
                    Price = x.Price
                }).ToList()
            }).FirstOrDefault();// tek siparis bilgisi gelir

            return View(model);
        }
    }
}