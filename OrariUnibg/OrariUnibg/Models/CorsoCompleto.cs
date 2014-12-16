﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrariUnibg.Models
{
    public class CorsoCompleto : Corso
    {
        private string _inizio;
        private string _fine;
        private string _inizioFine;
        public CorsoCompleto()
        {

        }
       
        public List<Lezione> Lezioni { get { return _lezioni; } }
        public List<Lezione> _lezioni = new List<Lezione>(); 
        
        public string InizioFine 
        {
            get { return _inizioFine; }
            set 
            { 
                _inizio = value.Substring(0, 10);
                _fine = value.Substring(10, 10);
                _inizioFine = _inizio + " - " + _fine;
            }
        }

        public DateTime Inizio
        {
            get { return new DateTime(int.Parse(_inizio.Split('/')[2]), int.Parse(_inizio.Split('/')[1]), int.Parse(_inizio.Split('/')[0])); }
        }

        public DateTime Fine
        {
            get { return new DateTime(int.Parse(_fine.Split('/')[2]), int.Parse(_fine.Split('/')[1]), int.Parse(_fine.Split('/')[0])); }
        }

        public int Semestre
        {
            get 
            {
                if (Inizio.Month > 8)
                    return 1;
                else return 2;
            }
        }
    }
}
