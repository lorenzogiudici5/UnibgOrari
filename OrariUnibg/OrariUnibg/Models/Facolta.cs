using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrariUnibg.Models
{
    public class Facolta
    {
        public Facolta() {}
        public string Nome { get; set; }
        public string DB { get; set; }

        public int IdFacolta
        {
            get;
            set;
            //get 
            //{
            //    switch (Nome)
            //    {
            //        case "Ingegneria":
            //            this.DB = "IN";
            //            return 1;
            //        case "Lettere e Filosofia":
            //            this.DB = "UM";
            //            return 10;
            //        case "Giurisprudenza":
            //            this.DB = "EC";
            //            return 4;
            //        case "Lingue, Letterature Straniere e Comunicazione":
            //            this.DB = "LL";
            //            return 1;
            //        case "Scienze Aziendali Economiche e Metodi Quantitativi":
            //            this.DB = "EC";
            //            return 1;
            //        case "Scienze Umane e Sociali":
            //            this.DB = "UM";
            //            return 2;
            //        default:
            //            this.DB = "LL";
            //            return 3;
            //    }
            //}
            //set { }
        }

        public static List<Facolta> facolta = new List<Facolta>() 
        { 
            new Facolta() {Nome = "Ingegneria", DB="IN", IdFacolta = 1},
            new Facolta() {Nome = "Lettere e Filosofia", DB="UM", IdFacolta = 10},
            new Facolta() {Nome = "Giurisprudenza", DB="EC", IdFacolta = 4},
            new Facolta() {Nome = "Lingue, Lett. Straniere e Comunic.", DB="LL", IdFacolta = 1},
            new Facolta() {Nome = "Economia", DB="EC", IdFacolta = 1},
            new Facolta() {Nome = "Scienze Umane e Sociali", DB="UM", IdFacolta = 2},
            //new Facolta() {Nome = "CIS - Corsi Italiano per Stranieri" , DB="LL", IdFacolta = 3},
        };
    }
}
