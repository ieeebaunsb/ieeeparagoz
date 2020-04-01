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
    public partial class Kayit : ContentPage
    {
        FirebaseHelper fb;
        public Kayit()
        {
            InitializeComponent();
            fb = new FirebaseHelper();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            Users kullanici = new Users();
            bool ctrl = false ;
            if (ad.Text!=""&&soyad.Text!=""&&mail.Text!=""&&password.Text!=""&& ad.Text != null && soyad.Text != null && mail.Text != null && password.Text !=null)
            {
                kullanici.Ad = ad.Text.Trim();
                kullanici.Soyad = soyad.Text.Trim();
                kullanici.KullaniciMail = mail.Text.Trim();
                kullanici.Parola = password.Text.Trim();
                kullanici.Tarih = DateTime.Now;
                ctrl = true;
            }
            else
            {
                ctrl = false;
            }
            
            //var FBLogin = DependencyService.Get<ILogin>();
            //string token = await FBLogin.DoRegisterWithEP(kullanici.KullaniciMail,kullanici.Parola);
            //kullanici.KullaniciKey = token;
            if (ctrl)
            {
                if (await fb.AddUser(kullanici))
                {
                   
                    await DisplayAlert("Kayıt Başarılı", "Başarıyla kayıt oldunuz.","Tamam");

                    await Navigation.PopAsync();

                }

                else
                    await DisplayAlert("Hata", "Bu Kullanıcı Adı Kullanılıyor.", "Tamam");


            }
            else
            {
              
                await DisplayAlert("Hata", "Tüm Bilgileri Eksiksiz Doldurduğunuzdan Emin Olun!", "Tamam");
            }


        }
    }
}