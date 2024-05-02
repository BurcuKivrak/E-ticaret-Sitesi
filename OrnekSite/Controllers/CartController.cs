using OrnekSite.Entity;
using OrnekSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrnekSite.Controllers
{
    public class CartController : Controller
    {

        DataContext db = new DataContext();// veritabanı baglantisi
        // GET: Cart
        public ActionResult Index()
        {
            return View(GetCart());
        }

        private void SaveOrder(Cart cart, ShippingDetails model)
        {
            var order = new Order();
            order.OrderNumber = "A" + (new Random().Next(1111, 9999).ToString());
            order.Total = cart.Total();
            order.OrderDate = DateTime.Now;
            order.UserName = User.Identity.Name;
            order.OrderState = OrderState.Bekleniyor;
            order.Adres = model.Adres;
            order.Sehir = model.Sehir;
            order.Semt = model.Semt;
            order.Mahalle = model.Mahalle;
            order.PostaKodu = model.PostaKodu;
            order.OrderLines = new List<OrderLine>();
            foreach (var item in cart.CartLines)
            {
                var orderline = new OrderLine();
                orderline.Quantity = item.Quantity;
                orderline.Price = item.Quantity * item.Product.Price;
                orderline.ProductId = item.Product.Id;
                order.OrderLines.Add(orderline);
            }
            db.Orders.Add(order);// siparisleri veritabanına kaydeder.
            db.SaveChanges();
        }

        public ActionResult CheckOut()
        {
            return View( new ShippingDetails()); 
        }

        [HttpPost]
        public ActionResult CheckOut(ShippingDetails model)
        {
            var cart = GetCart();
            if(cart.CartLines.Count==0) // sepet boş ise
            {
                ModelState.AddModelError("UrunYok", "Sepetinizde ürün bulunmamaktadır.");
            }
            if(ModelState.IsValid) // kullanici sepete urun koymus ve gerekli alanlari doldurdu ise
            {
                SaveOrder(cart, model);
                cart.Clear();
                return View("SiparisTamamlandi");
            }
            else
            {
                return View(model);
            }
            
        }


        public PartialViewResult Summary()
        {
            return PartialView(GetCart());
        }
        public PartialViewResult Summary1()
        {
            return PartialView(GetCart());
        }

        public ActionResult RemoveFromCart(int Id)
        {
            var product = db.Products.FirstOrDefault(i => i.Id == Id);
            if(product!=null)
            {
                GetCart().DeleteProduct(product);

            }
            return RedirectToAction("Index");
        }

        public ActionResult AddToCart(int Id)
        {
            var product = db.Products.FirstOrDefault(i => i.Id == Id);
            if(product!=null)
            {
                GetCart().AddProduct(product, 1); // tek seferde sepete eklenecek urun miktari(quantity)
            }

                 
            return RedirectToAction("Index");
        }
        public Cart GetCart()
        {
            var cart = (Cart)Session["Cart"];
            if(cart==null) // cart yoksa yeni bi kart olustur.
            {
                cart = new Cart();
                Session["Cart"] = cart;  
            }
            return cart;
        }
    }
}