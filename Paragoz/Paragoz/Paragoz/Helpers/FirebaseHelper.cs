using Firebase.Database;
using Paragoz.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database.Query;
using System.Linq;


namespace Paragoz.Helpers
{
    public class FirebaseHelper
    {
        FirebaseClient fb;
        public FirebaseHelper()
        {
            fb = new FirebaseClient("https://paragoz-16448.firebaseio.com/");

        }




        public async Task<bool> DeleteExpenses(string user, string catKey, string expKey)
        {
            try
            {
                await fb.Child("Kullanici/" + user + "/Kategoriler/" + catKey + "/Harcamalar/" + expKey).DeleteAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> DeleteCategory(string user, string catName)
        {
            try
            {

                var a = (await fb.Child("Kullanici/" + user + "/Kategoriler")
                .OnceAsync<Categories>())
                .Select(item => new Categories
                {
                    KategoriAdi = item.Object.KategoriAdi,
                    KategoriKey = item.Key,
                    KullaniciID = item.Object.KullaniciID,
                    SinirDegeri = item.Object.SinirDegeri

                }).Where(item => item.KategoriAdi == catName).ToList();

                foreach (var item in a)
                {
                    await fb.Child("Kullanici/" + user + "/Kategoriler/" + item.KategoriKey).DeleteAsync();

                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<bool> DeleteWallet(string user, string wallName)
        {
            try
            {
                var a = (await fb.Child("Kullanici/" + user + "/Cuzdan")
               .OnceAsync<Wallets>())
               .Select(item => new Wallets
               {
                   CuzdanAdi = item.Object.CuzdanAdi,
                   CuzdanKey = item.Key,
                   CuzdanPara=item.Object.CuzdanPara,
                   KullaniciKey=item.Object.KullaniciKey



               }).Where(item => item.CuzdanAdi == wallName).ToList();

                foreach (var item in a)
                {
                    await fb.Child("Kullanici/" + user + "/Cuzdan/" + item.CuzdanKey).DeleteAsync();

                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<bool> CalculateCategoryLimit(string user, string catKey)
        {
            try
            {
                var list = await GetExpensesList(user, catKey);
                double sum = 0;
                foreach (var item in list)
                {
                    if (item.Tarih >= DateTime.Now.AddMonths(-1))
                    {
                        sum += item.Miktar;

                    }
                }
                var a = (await fb.Child("Kullanici/" + user + "/Kategoriler")
                    .OnceAsync<Categories>())
                    .Select(item => new Categories
                    {
                        KategoriAdi = item.Object.KategoriAdi,
                        KategoriKey = item.Key,
                        KullaniciID = item.Object.KullaniciID,
                        SinirDegeri = item.Object.SinirDegeri

                    }).Where(item => item.KategoriKey == catKey).ToList();
                foreach (var item in a)
                {
                    if (sum >= item.SinirDegeri)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            }
            catch (Exception)
            {

                return false;
            }


        }
        public bool CalculateExp(Expenses e, Wallets w, string user)
        {
            try
            {
                if (!(w.CuzdanPara < e.Miktar))
                {
                    w.CuzdanPara = w.CuzdanPara - e.Miktar;
                    fb.Child("Kullanici/" + user + "/Cuzdan/" + w.CuzdanKey).PutAsync(w);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                return false;
            }

        }
        public bool CalculateRev(Revenues r, Wallets w, string user)
        {
            try
            {
                w.CuzdanPara = w.CuzdanPara + r.Miktar;
                fb.Child("Kullanici/" + user + "/Cuzdan/" + w.CuzdanKey).PutAsync(w);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public async Task<Users> UserLogin(string kullaniciAdi)
        {
            try
            {
                var list = (await fb.Child("Kullanici")
               .OnceAsync<Users>())
               .Select(item => new Users
               {
                   KullaniciKey = item.Key,
                   Parola = item.Object.Parola,
                   KullaniciMail = item.Object.KullaniciMail,
                   Ad = item.Object.Ad,
                   Soyad = item.Object.Soyad,
                   Tarih = item.Object.Tarih
               }).Where(x => x.KullaniciMail == kullaniciAdi).ToList();

                Users kullanici = new Users();

                foreach (var item in list)
                {
                    kullanici = item;
                }


                return kullanici;
            }
            catch (Exception)
            {
                return null;
            }

        }
        public async Task<List<Users>> GetUserList()
        {
            try
            {
                var list = (await fb.Child("Kullanici")
               .OnceAsync<Users>())
               .Select(item => new Users
               {
                   KullaniciKey = item.Key,
                   Parola = item.Object.Parola,
                   KullaniciMail = item.Object.KullaniciMail,
                   Ad = item.Object.Ad,
                   Soyad = item.Object.Soyad,
                   Tarih = item.Object.Tarih
               }).ToList();



                return list;
            }
            catch (Exception)
            {
                return null;
            }

        }
        public async Task<List<Categories>> GetCategoriesList(string user)
        {
            try
            {
                var list = (await fb.Child("Kullanici/" + user + "/Kategoriler")
                .OnceAsync<Categories>())
                .Select(item => new Categories
                {
                    KategoriAdi = item.Object.KategoriAdi,
                    KategoriKey = item.Key,
                    KullaniciID = item.Object.KullaniciID,
                    SinirDegeri = item.Object.SinirDegeri

                }).ToList();



                return list;
            }
            catch (Exception)
            {

                return null;
            }

        }
        public async Task<List<Expenses>> GetExpensesList(string user, string cat)
        {
            try
            {
                var list = (await fb.Child("Kullanici/" + user + "/Kategoriler/" + cat + "/Harcamalar")
               .OnceAsync<Expenses>())
               .Select(item => new Expenses
               {
                   Aciklama = item.Object.Aciklama,
                   HarcamalarKey = item.Key,
                   KategoriKey = item.Object.KategoriKey,
                   KullaniciKey = item.Object.KullaniciKey,
                   Miktar = item.Object.Miktar,
                   Tarih = item.Object.Tarih
               }).ToList();



                return list;
            }
            catch (Exception)
            {

                return null;
            }

        }
        public async Task<List<Wallets>> GetWalletList(string user)
        {
            try
            {
                var list = (await fb.Child("Kullanici/" + user + "/Cuzdan")
               .OnceAsync<Wallets>())
               .Select(item => new Wallets
               {
                   CuzdanAdi = item.Object.CuzdanAdi,
                   CuzdanPara = item.Object.CuzdanPara,
                   CuzdanKey = item.Key

               }).ToList();



                return list;
            }
            catch (Exception)
            {
                return null;
            }

        }
        public ObservableCollection<Categories> GetCategories(string user)
        {
            try
            {
                var Categories = fb.Child("Kullanici/" + user + "/Kategoriler").AsObservable<Categories>().AsObservableCollection();
                if (Categories != null)
                    return Categories;
                return null;
            }
            catch (Exception)
            {
                return null;
            }


        }
        public ObservableCollection<Wallets> GetWallet(string user)
        {
            try
            {
                var Wallet = fb.Child("Kullanici/" + user + "/Cuzdan").AsObservable<Wallets>().AsObservableCollection();
                if (Wallet != null)
                    return Wallet;
                return null;
            }
            catch (Exception)
            {

                return null;
            }


        }
        public ObservableCollection<Revenues> GetRevenues(string user, string wall)
        {
            try
            {
                var Rev = fb.Child("Kullanici/" + user + "/Gelirler").AsObservable<Revenues>().AsObservableCollection();
                return Rev;
            }
            catch (Exception)
            {

                return null;
            }


        }
        public ObservableCollection<Expenses> GetExpenses(string user, string cat)
        {
            try
            {
                var Exp = fb.Child("Kullanici/" + user + "/Kategoriler/" + cat + "/Harcamalar").AsObservable<Expenses>().AsObservableCollection();
                if (Exp != null)
                    return Exp;
                return null;
            }
            catch (Exception)
            {
                return null;
            }


        }
        public async Task AddExpenses(Expenses exp, string user, string category)
        {
            try
            {
                await fb.Child("Kullanici/" + user + "/Kategoriler/" + category + "/Harcamalar").PostAsync(exp);

            }
            catch (Exception)
            {

            }
        }
        public async Task AddRevenues(Revenues rev, string user)
        {
            try
            {
                await fb.Child("Kullanici/" + user + "/Gelirler").PostAsync(rev);

            }
            catch (Exception)
            {

            }
        }
        public async Task<bool> CategoryControl(Categories cat, string user)
        {
            var list = await GetCategoriesList(user);
            foreach (var item in list)
            {
                if (cat.KategoriAdi == item.KategoriAdi)
                {
                    return false;
                }
            }
            return true;
        }
        public async Task<bool> AddCategory(Categories kategori, string user)
        {
            try
            {
                if (await CategoryControl(kategori, user))
                {
                    await fb.Child("Kullanici/" + user + "/Kategoriler").PostAsync(kategori);
                    return true;
                }
                else
                    return false;

            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> WalletControl(Wallets wal, string user)
        {
            var list = await GetWalletList(user);
            foreach (var item in list)
            {
                if (wal.CuzdanAdi == item.CuzdanAdi)
                {
                    return false;
                }
            }
            return true;
        }
        public async Task<bool> AddWallet(Wallets cuzdan, string user)
        {
            try
            {
                if (await WalletControl(cuzdan, user))
                {
                    await fb.Child("Kullanici/" + user + "/Cuzdan").PostAsync(cuzdan);
                    return true;
                }
                else
                    return false;


            }
            catch (Exception)
            {

                return false;
            }
        }
        public async Task<bool> UserControl(Users kullanici)
        {
            var list = await GetUserList();
            foreach (var item in list)
            {
                if (kullanici.KullaniciMail == item.KullaniciMail)
                {
                    return false;
                }
            }
            return true;
        }
        public async Task<bool> AddUser(Users kullanici)
        {
            try
            {
                if (await UserControl(kullanici))
                {
                    await fb.Child("Kullanici").PostAsync(kullanici);
                    return true;
                }
                else
                    return false;

            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
