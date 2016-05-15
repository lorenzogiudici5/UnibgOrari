using Microsoft.WindowsAzure.MobileServices;
using OrariUnibg.Helpers;
using OrariUnibg.Models;
using OrariUnibg.Services.Authentication;
using OrariUnibg.Services.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OrariUnibg.Services.Azure
{
    public class AzureDataService
    {
        #region Properties
        public MobileServiceClient MobileService { get; set; }
        public User User;
        #endregion

        #region Private Fields
        IMobileServiceTable<User> userTable;
        IMobileServiceTable<Corso> corsoTable;
        IMobileServiceTable<Preferiti> preferitiTable;
        bool isInitialized;
        #endregion

        #region Public Methods
        public async Task Initialize()
        {
            if (isInitialized)
                return;

            var handler = new AuthHandler();
            //Create our client
            MobileService = new MobileServiceClient("https://tazzecaffe.azurewebsites.net", handler);

            handler.Client = MobileService;

            if (!string.IsNullOrWhiteSpace(Settings.AuthToken) && !string.IsNullOrWhiteSpace(Settings.UserId))
            {
                MobileService.CurrentUser = new MobileServiceUser(Settings.UserId);
                MobileService.CurrentUser.MobileServiceAuthenticationToken = Settings.AuthToken;
            }

            this.userTable = MobileService.GetTable<User>();
            this.corsoTable = MobileService.GetTable<Corso>();
            this.preferitiTable = MobileService.GetTable<Preferiti>();

            isInitialized = true;
        }

        #region User
        public async Task<bool> AddUser()
        {
            await Initialize();

            checkAuthentication();

            var existUser = await ExistsUser();

            if(User == null && !existUser)
            {
                //create and insert user
                User = new User()
                {
                    Email = Settings.Email,
                    Id = Settings.UserId,
                    SocialId = Settings.SocialId,
                    Name = Settings.Name,
                    Picture = Settings.Picture,

                    //Matricola = Settings.Matricola,
                    //FacoltaId = Settings.FacoltaId,
                    //LaureaId = Settings.LaureaId,
                    //AnnoIndex = Settings.AnnoIndex,
                };

                await userTable.InsertAsync(User);
                return true;
            }
            else
            {
                Settings.Matricola = User.Matricola;
                Settings.FacoltaId = User.FacoltaId;
                Settings.LaureaId = User.LaureaId;
                Settings.AnnoIndex = User.AnnoIndex;
                Settings.FacoltaDB = User.FacoltaDB;
                await userTable.UpdateAsync(User);
                return false;
            }
            
            //var users = await userTable.Where(x => x.Id == User.Id).ToEnumerableAsync();
            //if (users.Count() == 0) //se non esiste già un utente con lo stesso id
            //    await userTable.InsertAsync(User);
            //else //DA ELIMINARE SE NON SI VUOLE PERMETTERE L'UPDATE all'autenticazione
            //    await userTable.UpdateAsync(User);
        }

        public async Task UpdateUser()
        {
            await Initialize();

            await userTable.UpdateAsync(User);
        }

        public async Task<List<User>> GetUsers()
        {
            await Initialize();

            checkAuthentication();

            var users = await userTable.OrderBy(c => c.Email).ToEnumerableAsync();
            //return new ObservableCollection<TazzaDiCaffe>(coffes);
            return users.ToList();
        }

        public async Task<bool> ExistsUser()
        {
            await Initialize();

            var users = await userTable.Where(x => x.Id == Settings.UserId).ToEnumerableAsync();
            if (users.Count() > 0)
            {
                User = users.FirstOrDefault();
                return true;
            }
            else
            {
                User = null;
                return false;

            }
        }
        #endregion

        #region Corso
        public async Task AddCorso(Corso corso)
        {
            await Initialize();

            checkAuthentication();

            if(!await ExistsCorso(corso)) //se il corso non esiste, lo aggiungo
                await corsoTable.InsertAsync(corso);
            else
            {
                var c = await GetCorso(corso);
                if(c.Docente != corso.Docente) //magari è cambiato l'insegnante??
                {
                    corso.Id = c.Id;           //devo passargli l'id!
                    await corsoTable.UpdateAsync(corso);
                }

            }


            //else
            //{
            //    await corsoTable.UpdateAsync(corso);
            //}

            //var users = await userTable.Where(x => x.Id == User.Id).ToEnumerableAsync();
            //if (users.Count() == 0) //se non esiste già un utente con lo stesso id
            //    await userTable.InsertAsync(User);
            //else //DA ELIMINARE SE NON SI VUOLE PERMETTERE L'UPDATE all'autenticazione
            //    await userTable.UpdateAsync(User);
        }

        public async Task<List<Corso>> GetCorsi()
        {
            await Initialize();

            checkAuthentication();

            var corsi = await corsoTable.OrderBy(c => c.Insegnamento).ToEnumerableAsync();
            //return new ObservableCollection<TazzaDiCaffe>(coffes);
            return corsi.ToList();
        }

        public async Task<bool> ExistsCorso(Corso corso)
        {
            await Initialize();

            var corsi = await corsoTable.Where(x => x.Insegnamento == corso.Insegnamento && x.Codice == corso.Codice).ToEnumerableAsync();
            if (corsi.Count() > 0)
                return true;
            else
                return false;
        }

        public async Task<Corso> GetCorso(Corso corso)
        {
            await Initialize();

            var c = await corsoTable.Where(x => (x.Id == corso.Id) || (x.Insegnamento == corso.Insegnamento && x.Codice == corso.Codice)).ToEnumerableAsync();
            return c.ToList().FirstOrDefault();
        }

        public async Task DeleteCorso (Corso corso)
        {
            await Initialize();

            var c = await GetCorso(corso);
            if(c != null)
                await corsoTable.DeleteAsync(c);
        }
        #endregion

        #region Preferiti
        public async Task AddPreferito(Preferiti preferito)
        {
            await Initialize();

            checkAuthentication();

            if(! await ExistsPreferito(preferito))
                await preferitiTable.InsertAsync(preferito);

            //ho aggiunto un nuovo corso quindi devo aggiornare
            Settings.ToUpdate = true;

            //if (!await ExistsPreferito(preferito)) //se il corso non esiste, lo aggiungo
            //    await corsoTable.InsertAsync(preferito);
            //else
            //    await corsoTable.UpdateAsync(preferito); //magari è cambiato l'insegnante??

            //else
            //{
            //    await corsoTable.UpdateAsync(corso);
            //}

            //var users = await userTable.Where(x => x.Id == User.Id).ToEnumerableAsync();
            //if (users.Count() == 0) //se non esiste già un utente con lo stesso id
            //    await userTable.InsertAsync(User);
            //else //DA ELIMINARE SE NON SI VUOLE PERMETTERE L'UPDATE all'autenticazione
            //    await userTable.UpdateAsync(User);
        }

        public async Task<List<Preferiti>> GetAllPreferiti()
        {
            await Initialize();

            checkAuthentication();

            var preferiti = await preferitiTable.OrderBy(c => c.Id).ToEnumerableAsync();
            //return new ObservableCollection<TazzaDiCaffe>(coffes);
            var list = preferiti.ToList();
            return list;
        }

        public async Task<List<Preferiti>> GetMieiPreferiti()
        {
            await Initialize();

            checkAuthentication();

            //solo i miei corsi
            var preferiti = await preferitiTable.Where(c => c.UserId == Settings.UserId).OrderBy(c => c.Id).ToEnumerableAsync();
            
            return preferiti.ToList();
        }

        public async Task<bool> ExistsPreferito(Preferiti preferito)
        {
            await Initialize();

            var preferiti = await preferitiTable.Where(x => x.UserId == Settings.UserId && x.IdCorso == preferito.IdCorso).ToEnumerableAsync();
            if (preferiti.Count() > 0)
                return true;
            else
                return false;
        }

        public async Task DeletePreferito(Preferiti preferito)
        {
            await Initialize();

            var preferiti = await preferitiTable.Where(x => x.UserId == Settings.UserId && x.IdCorso == preferito.IdCorso).ToEnumerableAsync();


            if (preferiti.Count() > 0)
            {
                try
                {
                    await preferitiTable.DeleteAsync(preferiti.FirstOrDefault());
                }
                catch(Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }


            //TO DO try to delete Corso
            //devo verificare se ci sono altri utenti che hanno quel corso tra i preferiti, altrimenti lo "potrei" cancellare
            //if preferitiTable.Where(x => x.IdCorso == preferito.IdCorso).ToEnumerableAsync().Count() == 0) 
            //allora cancello anche dalla tabella corso
        }
        #endregion

        #endregion

        #region Private Methods
        private async void checkAuthentication()
        {
            if (!Settings.IsLoggedIn)
            {
                await Initialize();
                var user = await DependencyService.Get<IAuthentication>().LoginAsync(MobileService, MobileServiceAuthenticationProvider.MicrosoftAccount);
                if (user == null)
                    return;

                //pull latest data from server:
                var users = await GetUsers();

                //Coffees.ReplaceRange(coffees);
                //SortCoffees();
            }
        }
        #endregion






    }
}
