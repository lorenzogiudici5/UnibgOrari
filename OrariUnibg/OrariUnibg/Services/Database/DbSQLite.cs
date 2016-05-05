﻿using OrariUnibg.Models;
using SQLite.Net;
using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OrariUnibg.Services.Database
{
    public class DbSQLite
    {
        #region Constructor
        public DbSQLite(SQLiteConnection sqliteConnection)
        {
            db = sqliteConnection;
           // db = DependencyService.Get<ISQLite>().GetConnection();
            //db = App.SQLite.GetConnection();
            db.CreateTable<Preferiti>();
            db.CreateTable<Orari>();
            db.CreateTable<Utenze>();
            db.CreateTable<LogTb>();
        }

        public DbSQLite()
        {
            db = DependencyService.Get<ISQLite>().GetConnection();
            db.CreateTable<Preferiti>();
            db.CreateTable<Orari>();
        }
        #endregion


        #region Private Fields
        SQLiteConnection db;
        #endregion

        #region Azure
        public void SynchronizeAzureDb()
        {

        }
        #endregion

        #region MieiCorsi
        public IEnumerable<Preferiti> GetAllMieiCorsi()
        {
            return (from i in db.Table<Preferiti>() select i).ToList();
        }
        public Preferiti GetItem(int id)
        {
            return db.Table<Preferiti>().FirstOrDefault(x => x.IdPref == id);
        }

        public void Update(Preferiti item)
        {
            db.Update(item);
        }
        //public bool CheckAppartiene(Orari item)
        //{
        //    var x = (from i in db.Table<Orari>() where i.Insegnamento == item.Insegnamento && i.Codice == item.Codice && i.Date == item.Date select i).ToList();
        //    if (x.Count > 0)
        //        return true;
        //    else
        //        return false;
        //}
        public bool CheckAppartieneMieiCorsi(Corso item)
        {
			Logcat.Write ("dentro il check appartiene");

			return CheckAppartieneMieiCorsi (item.Insegnamento);
        }

		public bool CheckAppartieneMieiCorsi(string insegnamento)
		{
//			var list = GetAllMieiCorsi().Where(x => x.Insegnamento == item.Insegnamento).ToList();
			//          var list = db.Table<MieiCorsi>().Where(x => x.Insegnamento == item.Insegnamento).ToList();
			var list = GetAllMieiCorsi().Where(x => x.Insegnamento == insegnamento).ToList();

			if (list.Count > 0)
				return true;
			else
				return false;
		}

        public void Insert(Preferiti item)
        {
            db.Insert(item);
        }
        public int DeleteMieiCorsi(Preferiti corso)
        {
            DeleteOrari(corso);
            return db.Delete<Preferiti>(corso.IdPref);
        }
        #endregion


        #region Orari
        public IEnumerable<Orari> GetAllOrari()
        {
			var o = (from i in db.Table<Orari>() select i).ToList();
			return o;
        }
        
        public bool AppartieneOrari(CorsoGiornaliero item)
        {
//			var count = db.Table<Orari> ().ToList ();
//			var x = (from i in db.Table<Orari> ()
//					 where i.Insegnamento == item.Insegnamento && i.Date == item.Date
//			         select i).ToList ();
			
			var y = GetAllOrari().Where (c=> c.Insegnamento == item.Insegnamento && c.Date.Date == item.Date.Date && item.AulaOra == c.AulaOra).FirstOrDefault ();

			if (y != null)
                return true;
            else
                return false;
        }

        //public Orari GetSingleOrari(int id)
        //{
        //    return db.Table<Orari>().FirstOrDefault(x => x.Id == id);
        //}

        public void Update(Orari item)
        {
            db.Update(item);
        }

        public void Insert(Orari item)
        {
			item.Date = item.Date.AddDays (1); //quando aggiungo un orario me lo sposta indietro di un giorno. Così, aggiungendo prima un giorno, sembra funzionare . . . 
            db.Insert(item);
        }

        public void InsertUpdate(CorsoGiornaliero item)
        {
            if (AppartieneOrari(item))
            {
                var i = db.Table<Orari>()
                .Where(x => x.Insegnamento == item.Insegnamento && x.Date == item.Date)
                .FirstOrDefault();

                i.Note = item.Note;
                i.AulaOra = item.AulaOra;
                //string query = "UPDATE Orari" + 
                //    " SET Note=" + item.Note + ", AulaOra=" + item.AulaOra
                //     + " WHERE Codice=" + item.Codice + " && Date=" + item.Date;
                db.Update(i);
            }

            else
            db.Insert(new Orari() 
            { 
                Insegnamento = item.Insegnamento, Codice = item.Codice, AulaOra = item.AulaOra, Note = item.Note, Date = item.Date, Docente = item.Docente
            });
        }
        private void DeleteOrari(Preferiti corso)
        {
            var list = GetAllOrari().Where(x => x.Insegnamento == corso.Insegnamento);
            foreach (var x in list)
                db.Delete<Orari>(x.Id);
        }

        public int DeleteSingleOrari(int id)
        {
            return db.Delete<Orari>(id);
        }
        #endregion

        #region Utenza
		public void CheckUtenzeDoppioni(){
			var utenze = GetAllUtenze ();

			for (int i = 0; i < utenze.Count(); i++) {
				var u = utenze.ElementAt (i);

				for (int h = i+1; h < utenze.Count(); h++) {
					var u2 = utenze.ElementAt (h);
					if (u.DateString == u2.DateString && u.AulaOra == u2.AulaOra)
						db.Delete<Utenze> (u.Id);
				}
			}

			utenze = GetAllUtenze ();
		}
		public void Insert(Utenze item)
        {
			item.Data = item.Data.AddDays (1);
            db.Insert(item);
        }

		public bool AppartieneUtenze(Utenze item)
        {
			var x = GetAllUtenze ().Where (u => u.Data.Date == item.Data.Date && u.AulaOra == item.AulaOra).FirstOrDefault ();
			if (x == null)
				return false;
			else
				return true;
        }

        public IEnumerable<Utenze> GetAllUtenze()
        {
            return (from i in db.Table<Utenze>() select i).ToList();
        }
        #endregion

        #region Logcat
        public void InsertLog(String log)
        {
            var logcat = new LogTb() {Date = DateTime.Now, Log = log };
            db.Insert(logcat);
        }

        public IEnumerable<LogTb> GetAllLogs()
        {
            return (from i in db.Table<LogTb>() select i).ToList();
        }

        public int ClearLog()
        {
            return db.DeleteAll<LogTb>();
        }

        #endregion

    }

    [Table("Preferiti")]
    public class Preferiti : Corso
    {
        [Newtonsoft.Json.JsonIgnore]
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int IdPref { get; set; } //Id accoppiata corso-preferito DB LOCAL

        [Newtonsoft.Json.JsonProperty("Id")] 
        public String IdPreferito { get; set; } //Id accoppiata corso-preferito AZURE

        //HO GIA' ID EREDITATO DEL CORSO
        [Newtonsoft.Json.JsonProperty("idCorso")]
        public string IdCorso { get; set; }

        [Newtonsoft.Json.JsonProperty("userId")]
        public string UserId { get; set; } //Id utente che inserisce il corso preferito 

        #region Serializer Json
        public override bool ShouldSerializeId()
        {
            return false;
        }
        public override bool ShouldSerializeInsegnamento()
        {
            return false;
        }
        public override bool ShouldSerializeCodice()
        {
            return false;
        }
        public override bool ShouldSerializeDocente()
        {
            return false;
        }
        #endregion
    }

    [Table("Orari")]
    public class Orari : CorsoGiornaliero
    {
        private bool _notify;
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        public bool Notify 
        {
            get { return _notify; }
            set {
				_notify = value;
			}
        }
        //public string Codice { get; set; }
        //public string Insegnamento { get; set; }
        //public string Docente { get; set; }

    }
	
	[Table("Utenze")]
	public class Utenze : Utenza
	{
		[PrimaryKey, AutoIncrement, Column("_id")]
		public int Id { get; set; }
	}

    [Table("LogTb")]
    public class LogTb
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        public DateTime Date { get; set; }

        public String Log { get; set; }
    }
}
