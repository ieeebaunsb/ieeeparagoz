using Firebase.Database;
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
	public partial class KategoriEkleme : ContentPage
	{
        FirebaseHelper fb;
		public KategoriEkleme()
		{
			InitializeComponent();
            fb = new FirebaseHelper();
		}
		private async void Button_Clicked(object eder, EventArgs asd)
		{
            await fb.AddCategory(new Database.Categories() {KategoriAdi = kategoriAdi.Text , SinirDegeri= Convert.ToDouble(maxmiktar.Text)}, "-M0grWhIKpvum7ECwK2H");
			await Navigation.PopAsync();
		}
	}
}