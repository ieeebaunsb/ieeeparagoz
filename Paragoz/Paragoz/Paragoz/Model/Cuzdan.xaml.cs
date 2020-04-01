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
	public partial class Cuzdan : ContentPage
	{
        FirebaseHelper fb;
        Users kul = new Users();
        public Cuzdan ()
		{
			InitializeComponent ();
            fb = new FirebaseHelper();
            Start();
        }
        public async void Start()
        {
            kul = await fb.UserLogin(Giris._KullaniciMail);
            lstCuzdan.ItemsSource = fb.GetWallet(kul.KullaniciKey);
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            if(cuzdanAdi.Text!=""&&miktar.Text!=""&& cuzdanAdi.Text != null && miktar.Text != null)
            {
                try
                {
                    Wallets wal = new Wallets();
                    wal.CuzdanAdi = cuzdanAdi.Text.Trim();
                    wal.CuzdanPara = Convert.ToDouble(miktar.Text.Trim());
                   if( !(await fb.AddWallet(wal, kul.KullaniciKey)))
                    {
                        await DisplayAlert("Hata", "Bu cüzdan zaten var!", "Tamam");
                    }
                    else
                    {
                        cuzdanAdi.Text = "";
                        miktar.Text = "";
                    }
                }
                catch (Exception x)
                {

                    await DisplayAlert("Hata",x.Message, "Tamam");

                }

            }
            else
            {
                await DisplayAlert("Hata", "Tüm Bilgileri Doldurduğunuzdan Emin Olun!", "Tamam");

            }

        }
     

        private async void MenuItem_Clicked_Delete(object sender, EventArgs e)
        {
            try
            {
                var mItem = sender as MenuItem;
                var data = (Wallets)mItem.CommandParameter;
                if (await fb.DeleteWallet(kul.KullaniciKey, data.CuzdanKey))
                {
                    await DisplayAlert("Silme İşlemi", data.CuzdanAdi + "\n Başarıyla silindi.", "Tamam");
                }
                else
                {
                    await DisplayAlert("Silme İşlemi", "Silme işlemi Başarısız!", "Tamam");
                }
            }
            catch (Exception)
            {
              await  DisplayAlert("Hata", "Bu cüzdan silinemiyor.", "Tamam");
            }
       
            
        }
    }
}