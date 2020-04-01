using Paragoz.Database;
using Paragoz.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Paragoz.Model
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Gelir : ContentPage
	{
        FirebaseHelper fb;
        List<Wallets> wall = new List<Wallets>();
		public Gelir()
		{
			InitializeComponent();
            fb = new FirebaseHelper();
            AddWalPicker();
		}
	    
        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                Revenues rev = new Revenues();
                Wallets w = new Wallets();
                foreach (var item in wall)
                {
                    if (cuzdan.SelectedItem.ToString() == item.CuzdanAdi)
                    {
                        w = item;         
                    }
                }
                rev.Miktar = Convert.ToDouble(gelirmiktar.Text);
                rev.Tarih = takvim.Date;
                rev.CuzdanKey = w.CuzdanKey;
                if (Duzen.IsToggled)
                    rev.DuzenliMi = true;
                else
                    rev.DuzenliMi = false;
                rev.KullaniciKey = "-M0grWhIKpvum7ECwK2H";
                fb.CalculateRev(rev, w, "-M0grWhIKpvum7ECwK2H");
                await fb.AddRevenues(rev, "-M0grWhIKpvum7ECwK2H");
               await DisplayAlert("Bilgi", "Gelir Başarıyla Kaydedildi!", "Tamam");
            }
            catch (Exception s)
            {

              await  DisplayAlert("Hata", s.Message, "Tamam");
            }
           
        }
        public async void AddWalPicker()
        {
            wall = await fb.GetWalletList("-M0grWhIKpvum7ECwK2H");
            foreach (var item in wall)
            {
                cuzdan.Items.Add(item.CuzdanAdi);

            }
        }
    }
}