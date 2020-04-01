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
    public partial class Anasayfa : TabbedPage
    {
        FirebaseHelper fb;
        List<Categories> categories = new List<Categories>();
        List<Wallets> wall = new List<Wallets>();
        Users kul = new Users();
        int count = 0;
        string key = "";
        public Anasayfa()
        {
            InitializeComponent();          
            fb = new FirebaseHelper();
            Start();
            
            

        }
        #region Toolbar


        async private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Menu());
        }
       
        void cuzdan(object sender, EventArgs e)
        {
            ToolbarItem item = (ToolbarItem)sender;
            Navigation.PushAsync(new Cuzdan());
        }

        void ayarlar(object sender, EventArgs e)
        {
            ToolbarItem item = (ToolbarItem)sender;
            Navigation.PushAsync(new Giris());
        }


        #endregion
        public async void Start()
        {
            kul= await fb.UserLogin(Giris._KullaniciMail);
            lstCuzdan.ItemsSource = fb.GetWallet(kul.KullaniciKey);
            ListExpenses(0);
        }
        private  void  Button_Clicked(object sender, EventArgs e)
        {
            string keyctrl="";
            if (count >= categories.LongCount() - 1)
            {
                count = 0;
            }
            keyctrl = (categories.Count > 0 ? categories[count].KategoriKey : "");


            if (key == keyctrl)
            {
                try
                {
                    ListExpenses(1);
                }
                catch (Exception)
                {

                     DisplayAlert("Hata", "Kategoriye Sahip Değilsiniz.", "Tamam");

                }

            }
            else
            {
                ListExpenses(0);
            }
        }

        public async void ListExpenses(int i)
        {
            try
            {
               

                categories = await fb.GetCategoriesList(kul.KullaniciKey);
                if (categories.Count<=0)
                {
                    lblHarcama.Text="Şuan da bir kategoriye sahip değilsiniz!";
                    lstHarcama.ItemsSource = null;
                }
                else
                {
                    count += i;
                    key = categories[count].KategoriKey;
                    
                    lblHarcama.Text = categories[count].KategoriAdi + " Kategorisinde Harcamalar";
                    lstHarcama.ItemsSource = fb.GetExpenses(kul.KullaniciKey, key);
                }
              

            }
            catch (Exception )
            {

                //await DisplayAlert("Hata", "Kategori Listeleme Sırasında Bir Hata Oluştu!", "Tamam");
            }
            


        }

        private void MenuItem_Clicked_Update(object sender, EventArgs e)
        {

        }

        private async void MenuItem_Clicked_Delete(object sender, EventArgs e)
        {
            var mItem = sender as MenuItem;
            var data = (Wallets)mItem.CommandParameter;
           


                if (await fb.DeleteWallet(kul.KullaniciKey, data.CuzdanAdi))
                {
                    await DisplayAlert("Silme İşlemi", data.CuzdanAdi + "\n Başarıyla silindi.", "Tamam");
                }
                else
                {
                    await DisplayAlert("Silme İşlemi", "Silme işlemi Başarısız!", "Tamam");
                }
            }
           
            
        

        private async void MenuItem_Clicked_Delete_Harcama(object sender, EventArgs e)
        {
            var mItem = sender as MenuItem;
            var data = (Expenses)mItem.CommandParameter;



            if (await fb.DeleteExpenses(kul.KullaniciKey, data.KategoriKey, data.HarcamalarKey))
            {
                await DisplayAlert("Silme İşlemi", data.Miktar + " TL miktarlı harcamanız başarıyla silindi.", "Tamam");
               lstHarcama.ItemsSource= fb.GetExpenses(kul.KullaniciKey, data.KategoriKey);
            }

            else
                await DisplayAlert("Silme İşlemi", "Silme işlemi Başarısız!", "Tamam");

        }
    }
}