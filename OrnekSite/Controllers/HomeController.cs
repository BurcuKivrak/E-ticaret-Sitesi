using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OrnekSite.Entity; // veritabanını tanıması için proejninin entitysi kütüphanesi eklenemli.

namespace OrnekSite.Controllers
{
    public class HomeController : Controller
    {
        DataContext db = new DataContext(); // veritabanı bağlantısı sağlandı.

        public PartialViewResult _FeaturedProductList()
        {
            return PartialView(db.Products.Where(i => i.IsApproved && i.IsFeatured).Take(5).ToList());
        }

        public ActionResult Adres()
        {
            return View();
        }

        public ActionResult Search(String q) // q ile kullanıcıdan arama metni alinir.
        {
            var p=db.Products.Where(i=>i.IsApproved==true);
            if(!string.IsNullOrEmpty(q)) // q nun ici bos ya da null degil ise arama yaapr.
            {
                p = p.Where(i => i.Name.Contains(q) || i.Description.Contains(q));
            }
            return View(p.ToList());
        }
        public PartialViewResult Slider()
        {
            return PartialView(db.Products.Where(i => i.IsApproved && i.Slider).Take(5).ToList());
        }

        // GET: Home
        public ActionResult Index()
        {
            return View(db.Products.Where(i=>i.IsHome&&i.IsApproved).ToList());
        }
        
        public ActionResult ProductDetails(int id)
        {
            return View(db.Products.Where(i=>i.Id==id).FirstOrDefault()); // geriye dönecek değer bir adet olsun diye(Tek Ürün)
        }

        public ActionResult Product()
        {
            return View(db.Products.ToList()); // product sayfasına tüm ürünlerin eklenmesi
        }

        public ActionResult ProductList(int id) // dışarıdan gelecek id degeri(kategoriId) aynı olan degerler listeleencek.
        {

            return View(db.Products.Where(i=>i.CategoryId==id).ToList());
        }

    }
}