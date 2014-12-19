using OrariUnibg.Models;
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
            db.CreateTable<MieiCorsi>();
            db.CreateTable<Orari>();
            db.CreateTable<Utenza>();
        }
        public DbSQLite()
        {
            db = DependencyService.Get<ISQLite>().GetConnection();
            db.CreateTable<MieiCorsi>();
            db.CreateTable<Orari>();
        }
        #endregion

        #region Private Fields
        SQLiteConnection db;
        #endregion 

        #region MieiCorsi
        public IEnumerable<MieiCorsi> GetItemsMieiCorsi()
        {
            return (from i in db.Table<MieiCorsi>() select i).ToList();
        }
        public MieiCorsi GetItem(int id)
        {
            return db.Table<MieiCorsi>().FirstOrDefault(x => x.Id == id);
        }

        public void Update(MieiCorsi item)
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
            var list = db.Table<MieiCorsi>().Where(x => x.Insegnamento == item.Insegnamento).ToList();
            if (list.Count > 0)
                return true;
            else
                return false;
        }

        public void Insert(MieiCorsi item)
        {
            db.Insert(item);
        }
        public int DeleteItem(int id)
        {
            return db.Delete<MieiCorsi>(id);
        }
        #endregion


        #region Orari
        public IEnumerable<Orari> GetAllOrari()
        {
            return (from i in db.Table<Orari>() select i).ToList();
        }
        public bool AppartieneOrari(CorsoGiornaliero item)
        {
            var x = (from i in db.Table<Orari>() where i.Insegnamento == item.Insegnamento && i.Date == item.Date select i).ToList();
            if (x.Count > 0)
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
        public int DeleteSingleOrari(int id)
        {
            return db.Delete<Orari>(id);
        }
        #endregion

        #region Utenza
        public void Insert(Utenza item)
        {
            db.Insert(item);
        }

        public IEnumerable<Utenza> GetAllUtenze()
        {
            return (from i in db.Table<Utenza>() select i).ToList();
        }
        #endregion
    }

    [Table("MieiCorsi")]
    public class MieiCorsi : Corso
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
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
            set 
            {
                if (value == null)
                    _notify = false;
                else
                    _notify = value;
                } 
        }
        //public string Codice { get; set; }
        //public string Insegnamento { get; set; }
        //public string Docente { get; set; }

    }
}
