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
    public partial class Menu : ContentPage
    {
        public Menu()
        {
            InitializeComponent();
            kategoriEkleme.GestureRecognizers.Add(new TapGestureRecognizer((view) => OnLabelClicked(new KategoriEkleme())));
            kategori.GestureRecognizers.Add(new TapGestureRecognizer((view) => OnLabelClicked(new Kategoriler())));
            harcama.GestureRecognizers.Add(new TapGestureRecognizer((view) => OnLabelClicked(new Harcamalar())));
            cuzdan.GestureRecognizers.Add(new TapGestureRecognizer((view) => OnLabelClicked(new Cuzdan())));
            gelir.GestureRecognizers.Add(new TapGestureRecognizer((view) => OnLabelClicked(new Gelir())));
            ayarlar.GestureRecognizers.Add(new TapGestureRecognizer((view) => OnLabelClicked(new Giris())));


        }

        async private void OnLabelClicked(Page p)
        {
            await Navigation.PushAsync(p);
        }
    }
}