using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using OrariUnibg.Services.Authentication;
using OrariUnibg.Droid.Services.Authentication;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using System.Globalization;
using Xamarin.Forms;
using OrariUnibg.Models;
using OrariUnibg.Helpers;

[assembly: Xamarin.Forms.Dependency(typeof(Authentication))]
namespace OrariUnibg.Droid.Services.Authentication
{
    public class Authentication : IAuthentication
    {
        public async Task<SocialLoginResult> LoginAsync(MobileServiceClient client, MobileServiceAuthenticationProvider provider)
        {
            try
            {
                //Settings.LoginAttempts++;
                var user = await client.LoginAsync(Forms.Context, provider, new Dictionary<string, string> { { "access_type", "offline" }, { "prompt", "consent" } });
                //var user = await client.LoginAsync(Forms.Context, provider);
                Settings.AuthToken = user?.MobileServiceAuthenticationToken ?? string.Empty;
                Settings.UserId = user?.UserId ?? string.Empty;

                // Creates a TextInfo based on the "en-US" culture.
                TextInfo myTI = new CultureInfo("en-US", false).TextInfo;

                var userData = await GetUserData(client);

                Settings.Email = userData.Message.Email;
                Settings.Username = Settings.Email.Split('@')[0];
                Settings.GivenName = myTI.ToTitleCase(userData.Message.Given_name.ToLower());
                Settings.Surname = myTI.ToTitleCase(userData.Message.Family_name.ToLower());
                Settings.Name = string.Format("{0} {1}", Settings.GivenName, Settings.Surname);
                Settings.SocialId = userData.Message.SocialId;
                Settings.Picture = userData.Message.Picture;

                return userData;

                //return new SocialLoginResult();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                e.Data["method"] = "LoginAsync";
            }

            return null;
        }

        private static async Task<SocialLoginResult> GetUserData(MobileServiceClient client)
        {
            var x = await client.InvokeApiAsync<SocialLoginResult>(
                    "getextrauserinfo",
                    HttpMethod.Get, null);

            var email = x.Message.Email;

            return x;


            //return new SocialLoginResult() { Message = x.Message };
            //return await client.InvokeApiAsync<SocialLoginResult>(
            //        "getextrauserinfo",
            //        HttpMethod.Get, null);
        }

        public void ClearCookies()
        {
            try
            {
                if ((int)global::Android.OS.Build.VERSION.SdkInt >= 21)
                    global::Android.Webkit.CookieManager.Instance.RemoveAllCookies(null);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

    }
}