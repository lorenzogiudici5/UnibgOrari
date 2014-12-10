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
        public DbSQLite()
        {
            db = DependencyService.Get<ISQLite>().GetConnection();
            db.CreateTable<MieiCorsi>();
        }
        #endregion

        #region Private Fields
        SQLiteConnection db;
        #endregion 

        #region MieiCorsi
        public IEnumerable<MieiCorsi> GetItems()
        {
            return (from i in db.Table<MieiCorsi>() select i).ToList();
        }
        public MieiCorsi GetItem(int id)
        {
            return db.Table<MieiCorsi>().FirstOrDefault(x => x.Id == id);
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
    }

    [Table("MieiCorsi")]
    public class MieiCorsi
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        public string Codice { get; set; }
        public string Insegnamento { get; set; }
        public string Docente { get; set; }
    }
}
