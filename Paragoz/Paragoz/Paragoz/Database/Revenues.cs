using System;
using System.Collections.Generic;
using System.Text;

namespace Paragoz.Database
{
   public  class Revenues
    {
        public string GelirKey { get; set; }
        public DateTime Tarih { get; set; }
        public double Miktar { get; set; }
        public bool DuzenliMi { get; set; }
        public string CuzdanKey { get; set; }
        public string KullaniciKey { get; set; }

    }
}
