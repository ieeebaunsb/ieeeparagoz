using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Auth;
using Paragoz.Droid.Interfaces;
using Paragoz.Interfaces;
using Xamarin.Forms;

[assembly:Dependency(typeof(AndroAuth))]
namespace Paragoz.Droid.Interfaces
{
    public class AndroAuth : ILogin
    {
        public async Task<string> DoLoginWithEP(string email, string password)
        {
            try
            {
                var user = await FirebaseAuth.Instance.SignInWithEmailAndPasswordAsync(email, password);
                var token = await user.User.GetIdTokenAsync(false);
                return token.Token;
            }
            catch (FirebaseAuthInvalidUserException notFound)
            {
                return notFound.Message;
            }
            catch (Exception x)
            {
                return x.Message;
                
            }
           
           
        }

        public async Task<string> DoRegisterWithEP(string email, string password)
        {
            try
            {
                var user = await FirebaseAuth.Instance.CreateUserWithEmailAndPasswordAsync(email, password);
                var token = await user.User.GetIdTokenAsync(false);
                return token.Token;
            }
          
            catch (Exception x)
            {
                return x.Message;

            }
        }
    }
}