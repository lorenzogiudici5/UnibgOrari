using Microsoft.WindowsAzure.MobileServices;
using OrariUnibg.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrariUnibg.Services.Authentication
{
    public interface IAuthentication
    {
        Task<SocialLoginResult> LoginAsync(MobileServiceClient client, MobileServiceAuthenticationProvider provider);
        void ClearCookies();
    }
}
