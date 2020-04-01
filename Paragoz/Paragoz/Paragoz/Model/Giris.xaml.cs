using Paragoz.Database;
using Paragoz.Helpers;
using Paragoz.Interfaces;
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
	public partial class Giris : ContentPage
	{
        FirebaseHelper fb;
        public static string _KullaniciMail;
		public Giris ()
		{
			InitializeComponent ();
            fb = new FirebaseHelper();
		}

        private async void Button_Clicked(object sender, EventArgs e)
        {
            string email = kullaniciAdi.Text.Trim();
            string pass = password.Text.Trim() ;
            bool ctrl = false;
            var allUser = await fb.GetUserList();

           
            foreach (var item in allUser)
            {
                if (item.KullaniciMail==email&& item.Parola==pass)
                {

                    _KullaniciMail = email;
                    ctrl = true;
                    await Navigation.PushAsync(new Anasayfa());

                }
            }
            if(!ctrl)
                await DisplayAlert("Giriş Hatası", "Kullanıcı adı veya şifreniz yanlış!", "Tamam");
           
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Kayit());

            //var FBLogin = DependencyService.Get<ILogin>();
            //string token = await FBLogin.DoRegisterWithEP(email, pass);
            
        }
    }
}