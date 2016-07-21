using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using OrariUnibg.Helpers;
using OrariUnibg.Services.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OrariUnibg.Services.Authentication
{
    public class AuthHandler : DelegatingHandler
    {
        //#region Private Fields
        //private AzureDataService _service;
        //#endregion

        //#region Properties
        //public AzureDataService Service
        //{
        //    get { return _service; }
        //    set { _service = value; }
        //}
        //#endregion
        public IMobileServiceClient Client { get; set; }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (this.Client == null)
            {
                throw new InvalidOperationException("Make sure to set the 'Client' property in this handler before using it.");
            }

            // Cloning the request, in case we need to send it again
            var clonedRequest = await CloneRequest(request);
            var response = await base.SendAsync(clonedRequest, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                // User is not logged in – we got a 401

                //***VERSIONE FUNZIONANTE (funziona il refresh del token per 2-3 giorni, dopodiche non rientra più)
                //try
                //{
                //    MobileServiceUser user = Client.CurrentUser;

                //    if (user == null)
                //    {
                //        // prompt with login UI
                //        await DependencyService.Get<IAuthentication>().LoginAsync((MobileServiceClient)Client, MobileServiceAuthenticationProvider.Google);
                //        //user = await Client.LoginAsync(MobileServiceAuthenticationProvider.Google, null);
                //    }
                //    else
                //    {
                //        // Refresh Tokens
                //        try
                //        {
                //            //EngagementAgent.Instance.StartActivity("RefreshToken");

                //            // Calling /.auth/refresh will update the tokens in the token store
                //            // and will also return a new mobile authentication token.
                //            JObject refreshJson = (JObject)await this.Client.InvokeApiAsync("/.auth/refresh",
                //                HttpMethod.Get,
                //                null);

                //            string newToken = refreshJson["authenticationToken"].Value<string>();
                //            user.MobileServiceAuthenticationToken = newToken;
                //            Settings.AuthToken = newToken;
                //        }
                //        catch
                //        {
                //            //EngagementAgent.Instance.StartActivity("Login");
                //            await DependencyService.Get<IAuthentication>().LoginAsync((MobileServiceClient)Client, MobileServiceAuthenticationProvider.Google);
                //            return response;
                //        }
                //    }

                //    // we're now logged in again.

                //    // Save the user to the app settings
                //    //saveUserDelegate(user);

                //    // Clone the request
                //    clonedRequest = await CloneRequest(request);

                //    clonedRequest.Headers.Remove("X-ZUMO-AUTH");

                //    // Set the authentication header
                //    clonedRequest.Headers.Add("X-ZUMO-AUTH", user.MobileServiceAuthenticationToken);

                //    // Resend the request
                //    response = await base.SendAsync(clonedRequest, cancellationToken);
                //}
                //catch (InvalidOperationException)
                //{
                //    // user cancelled auth, so lets return the original response
                //    return response;
                //}


                //****NUOVA VERSIONE SDK AGGIORANTA (da testare)
                //Forum Xamarin https://forums.xamarin.com/discussion/comment/210298#Comment_210298 
                try
                {
                    var user = await this.Client.RefreshUserAsync();
                    //var user = await this.Client.LoginAsync(MobileServiceAuthenticationProvider.Google, null);
                    //var user = await this.Client.LoginAsync()
                    // we're now logged in again.

                    // Clone the request
                    clonedRequest = await CloneRequest(request);


                    Settings.UserId = user.UserId;
                    Settings.AuthToken = user.MobileServiceAuthenticationToken;

                    clonedRequest.Headers.Remove("X-ZUMO-AUTH");
                    // Set the authentication header
                    clonedRequest.Headers.Add("X-ZUMO-AUTH", user.MobileServiceAuthenticationToken);

                    // Resend the request
                    response = await base.SendAsync(clonedRequest, cancellationToken);
                }
                catch (InvalidOperationException)
                {
                    // user cancelled auth, so lets return the original response
                    return response;
                }
            }

            return response;
        }

        private async Task<HttpRequestMessage> CloneRequest(HttpRequestMessage request)
        {
            var result = new HttpRequestMessage(request.Method, request.RequestUri);
            foreach (var header in request.Headers)
            {
                result.Headers.Add(header.Key, header.Value);
            }

            if (request.Content != null && request.Content.Headers.ContentType != null)
            {
                var requestBody = await request.Content.ReadAsStringAsync();
                var mediaType = request.Content.Headers.ContentType.MediaType;
                result.Content = new StringContent(requestBody, Encoding.UTF8, mediaType);
                foreach (var header in request.Content.Headers)
                {
                    if (!header.Key.Equals("Content-Type", StringComparison.OrdinalIgnoreCase))
                    {
                        result.Content.Headers.Add(header.Key, header.Value);
                    }
                }
            }

            return result;
        }
    }
}
