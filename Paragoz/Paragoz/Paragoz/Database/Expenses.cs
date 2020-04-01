using System;
using System.Collections.Generic;
using System.Text;

namespace Paragoz.Database
{
    public class Expenses
    {
        public string HarcamalarKey { get; set; }
        public DateTime Tarih { get; set; }
        public double Miktar { get; set; }
        public string Aciklama { get; set; }
        public string KategoriKey { get; set; }
        public string KullaniciKey { get; set; }



    }
}
