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
	public partial class Kategoriler : ContentPage
	{
        FirebaseHelper db;
        Users kul = new Users();
        public Kategoriler()
        {
            InitializeComponent();
            db = new FirebaseHelper();
            Start();
           
        }
        public async void Start()
        {
           
                kul = await db.UserLogin(Giris._KullaniciMail);
                lstKategori.ItemsSource = db.GetCategories(kul.KullaniciKey);
            
           
        }
        public async Task<List<Categories>> Limit()
        {
            var list = await db.GetCategoriesList(kul.KullaniciKey);
            List<Categories> LimitCat = new List<Categories>();

            foreach (var item in list)
            {
              if(await db.CalculateCategoryLimit(kul.KullaniciKey, item.KategoriKey))
                {
                    LimitCat.Add(item);
                }
            }
            return LimitCat;
        }

        private void MenuItem_Clicked_Update(object sender, EventArgs e)
        {
            var mItem = sender as MenuItem;
        }

        private async void MenuItem_Clicked_Delete(object sender, EventArgs e)
        {
            try
            {
                var mItem = sender as MenuItem;
                var data = (Categories)mItem.CommandParameter;

                await db.DeleteCategory(kul.KullaniciKey, data.KategoriAdi);
                await DisplayAlert("Silme işlemi ", data.KategoriAdi +"Kategorisi Başarıyla Silindi!", "Tamam");

            }
            catch (Exception s)
            {
                await DisplayAlert("Hata", s.Message, "Tamam");
                
            }
            
        }

       

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            var list = await Limit();

            string sinirKategoriler = "";

            if (list == null)
            {
                await DisplayAlert("Sınır Değerini Aşan Kategoriler", "Sınır Değerinizi Aşan Kategori Yok!", "Tamam");

            }
            else
            {


                foreach (var item in list)
                {
                    sinirKategoriler += item.KategoriAdi + "\n";
                }

                await DisplayAlert("Sınır Değerini Aşan Kategoriler", sinirKategoriler, "Tamam");
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (kategoriAdi.Text!=""&&maxmiktar.Text!=""&& kategoriAdi.Text != null && maxmiktar.Text != null )
            {
                try
                {
                    if (!(await db.AddCategory(new Database.Categories() { KategoriAdi = kategoriAdi.Text, SinirDegeri = Convert.ToDouble(maxmiktar.Text) }, kul.KullaniciKey)))
                    {
                        await DisplayAlert("Hata", "Böyle Bir Kategori Zaten Var", "Tamam");
                    }
                    else
                    {
                        kategoriAdi.Text = "";
                        maxmiktar.Text = "";
                        
                    }

                }
                catch (Exception)
                {
                    await DisplayAlert("Hata", "Tüm bilgileri doğru girdiğinizden emin olun!", "Tamam");
                }

            }
            else
            {
                await DisplayAlert("Hata", "Tüm bilgileri eksiksiz girdiğinizden emin olun!", "Tamam");

            }
        }
    }
}