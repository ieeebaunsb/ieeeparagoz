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
    public partial class Harcamalar : ContentPage
    {
        FirebaseHelper fb;
        List<Categories> categories = new List<Categories>();
        List<Wallets> wall = new List<Wallets>();
        Users kul = new Users();

        public Harcamalar()
        {
            InitializeComponent();
            fb = new FirebaseHelper();
            Start();
        }
        public async void Start()
        {
            kul = await fb.UserLogin(Giris._KullaniciMail);
            AddCatPicker();
            AddWalPicker();
        }
        public async void AddCatPicker()
        {
            kategori.Items.Clear();
            categories = await fb.GetCategoriesList(kul.KullaniciKey);
            foreach (var item in categories)
            {
                kategori.Items.Add(item.KategoriAdi);

            }

        }
        public async void AddWalPicker()
        {
            cuzdan.Items.Clear();
            cuzdangelir.Items.Clear();
            wall = await fb.GetWalletList(kul.KullaniciKey);
            foreach (var item in wall)
            {
                cuzdan.Items.Add(item.CuzdanAdi);
                cuzdangelir.Items.Add(item.CuzdanAdi);

            }
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                string secilenKategori = kategori.SelectedItem.ToString();
                string secilenCuzdan = cuzdan.SelectedItem.ToString();
                string catKey = "";

                foreach (var item in categories)
                {
                    if (secilenKategori == item.KategoriAdi)
                    {
                        catKey = item.KategoriKey;
                    }
                }

                if (catKey!=""&&harcamamiktarı.Text!=""&&takvim.Date!=null&& harcamamiktarı.Text != null)
                {
                    Expenses exp = new Expenses();
                    exp.KategoriKey = catKey;
                    exp.Miktar = Convert.ToDouble(harcamamiktarı.Text.Trim());
                    exp.Tarih = takvim.Date;
                    exp.Aciklama = (aciklama.Text==null?"":aciklama.Text.Trim());
                    Wallets wal = new Wallets();
                    foreach (var item in wall)
                    {

                        if (secilenCuzdan == item.CuzdanAdi)
                        {
                            wal = item;
                           
                        }
                    }

                    if (fb.CalculateExp(exp, wal, kul.KullaniciKey))
                    {
                        await fb.AddExpenses(exp,kul.KullaniciKey, catKey);
                        await DisplayAlert("Başarılı", "Gider Başarıyla Kaydedildi!", "Tamam");
                        aciklama.Text = "";
                        harcamamiktarı.Text = "";
                        
                    }
                    else
                    {
                        await DisplayAlert("Hata", "Yeterli Bakiye Yok", "Tamam");
                    }
                }
                else
                {
                    await DisplayAlert("Hata", "Tüm Bilgileri Eksiksiz Doldurduğunuzdan Emin Olun!", "Tamam");

                }

            }
            catch (Exception x)
            {

                await DisplayAlert("Hata", x.Message, "Tamam");
            }
            



        }

        private async void Button_Clicked_Gelir(object sender, EventArgs e)
        {
            try
            {
                Revenues rev = new Revenues();
                Wallets w = new Wallets();
                foreach (var item in wall)
                {
                    if (cuzdangelir.SelectedItem.ToString() == item.CuzdanAdi)
                    {
                        w = item;
                    }
                }
                if (gelirmiktar.Text.Trim() != null && gelirmiktar.Text.Trim() != "")
                {
                    rev.Miktar = Convert.ToDouble(gelirmiktar.Text.Trim());
                   // rev.Tarih = takvimgelir.Date;
                    rev.CuzdanKey = w.CuzdanKey;
                    //if (Duzen.IsToggled)
                    //    rev.DuzenliMi = true;
                    //else
                    //    rev.DuzenliMi = false;
                    rev.KullaniciKey = kul.KullaniciKey;
                    fb.CalculateRev(rev, w, kul.KullaniciKey);
                    await fb.AddRevenues(rev, kul.KullaniciKey);
                    await DisplayAlert("Bilgi", "Gelir Başarıyla Kaydedildi!", "Tamam");
                    gelirmiktar.Text = "";
                }
                else
                {
                    await DisplayAlert("Hata", "Tüm Bilgileri Eksiksiz Girdiğinizden Emin Olun!", "Tamam");

                }

            }
            catch (Exception s)
            {

                await DisplayAlert("Hata", s.Message, "Tamam");
            }
        }

        private void Button_Clicked_Refresh(object sender, EventArgs e)
        {
            AddCatPicker();
            AddWalPicker();
        }
    }
}