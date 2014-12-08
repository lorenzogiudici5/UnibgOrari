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
        #region private Fields
        SQLiteConnection db;
        #endregion 
        public DbSQLite(){
            db = DependencyService.Get<ISQLite> ().GetConnection ();
            db.CreateTable<Favourite>();
        }

        public IEnumerable<Favourite> GetItems()
        {
            return (from i in db.Table<Favourite>() select i).ToList();
        }
        public Favourite GetItem(int id)
        {
            return db.Table<Favourite>().FirstOrDefault(x => x.Id == id);
        }

        public void Insert(Favourite item)
        {
            db.Insert(item);
        }
        public int DeleteItem(int id)
        {
            return db.Delete<Favourite>(id);
        }
    }

    [Table("Favourite")]
    public class Favourite
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        public string Codice { get; set; }
        public string Insegnamento { get; set; }
        public string Docente { get; set; }
    }
}
