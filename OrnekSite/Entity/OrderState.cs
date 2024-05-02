using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrnekSite.Entity
{
    public enum OrderState
    {
        Bekleniyor, //0
        Tamamlandı, //1
        Paketlendi, //2
        Kargolandı  //3
    }
}