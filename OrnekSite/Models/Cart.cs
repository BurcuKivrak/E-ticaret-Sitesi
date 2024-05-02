using OrnekSite.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrnekSite.Models
{
    public class Cart
    {
        private List<CartLine> _cartLines = new List<CartLine>(); // kullanıcıya özel olan kart olusturulur. Eklenen urunler siralanir.
        public List<CartLine> CartLines // listeyi disariya acma
        {
            get { return _cartLines; } 
            
        }
        public void AddProduct(Product product,int quantity) // urun ekleme (urun ve eklenecek miktar)
        {
            var line = _cartLines.FirstOrDefault(i => i.Product.Id == product.Id); // eklenen urunun sepette gornunmesi
            if(line==null) // urun sepette yoksa ekle
            {
                _cartLines.Add(new CartLine() { Product = product, Quantity = quantity });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public void DeleteProduct(Product product)
        {
            _cartLines.RemoveAll(i => i.Product.Id == product.Id);
        }

        public double Total()
        {
            return _cartLines.Sum(i => i.Product.Price * i.Quantity);
        }
        public void Clear() // ürün temizleme
        {
            _cartLines.Clear(); 
        }
    }

    public class CartLine
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }

}