using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OrnekSite.Models
{
    public class Login
    {
        [Required] // kullanıcının doldurması zorunlu alanları belirler.
        [DisplayName("Adı")] // bu degiskenleri turkçe olarak yaz.
        public string Username { get; set; }
        [Required]
        [DisplayName("Şifre")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}