using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Paragoz.Interfaces
{
   public interface ILogin
    {
        Task<string> DoLoginWithEP(string email, string password);
        Task<string> DoRegisterWithEP(string email, string password);
    }
}
