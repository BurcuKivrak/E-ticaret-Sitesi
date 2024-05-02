using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
 
// Veritabanı ile iletişim sağlayacak context nesnesini oluşturucak. 
namespace OrnekSite.Entity
{
    public class DataContext:DbContext // db context den türemesi gerekir.
    {
        public DataContext():base("dataConnection") // veritabanı aderesi. webconfigdeki oluşturulan data conenction adresi yazılamlı.
        {
            Database.SetInitializer(new DataInitializer()); // initializer yapısı görülmüş ooldu. 
        }
        // context in kullandığı entityleri eklemek gerekir.
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categoris { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }
    }
    
}