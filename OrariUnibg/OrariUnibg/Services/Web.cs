using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using OrariUnibg.Models;
using OrariUnibg.Services.Database;

namespace OrariUnibg.Services
{
    public static class Web
    {
        //public async Task<Earthquake[]> GetEarthquakes()
        //{
        //    var client = new HttpClient();
        //    client.BaseAddress = new Uri("http://api.geonames.org/");
        //    var response = await client.GetAsync("earthquakesJSON?north=44.1&south=-9.9&east=-22.4&west=55.2&username=bertt", HttpCompletionOption.ResponseHeadersRead);
        //    var earthquakesJson = response.Content.ReadAsStringAsync().Result;
        //    var rootobject = JsonConvert.DeserializeObject<Rootobject>(earthquakesJson);
        //    return rootobject.earthquakes;
        //}

        public static async Task<string> GetOrarioGiornaliero(string db, int fac, int laurea, string data)
        {
            string tipo = laurea == 0 ? "LCSDIPRXE" : "LCSDR";
            string s = null;
            String uri = string.Format("http://www03.unibg.it/orari/orario_giornaliero.php?db={0}&idfacolta={1}&idlaurea={2}&data={3}&tipo={4}", db, fac, laurea, data, tipo);
            try
            {
                var httpClient = new HttpClient();
                Task<string> contentsTask = httpClient.GetStringAsync(new Uri(uri)); // async method!
                s = await contentsTask;
                s = s.Replace("&deg;", "°");
                s = s.Replace("&nbsp;", "");
                s = s.Replace("&agrave;", "à");
                s = s.Replace("&egrave;", "è");
            }
            catch(Exception ex)
            {
				System.Diagnostics.Debug.WriteLine(ex.Message);
                return String.Empty;
            }

            return s;
        }

        public static async Task<string> GetOrarioCompleto(string semestre, string db, int fac, int laurea, int anno)
        {
            string s = null;
            string uri = string.Format("http://www03.unibg.it/orari/orario_{0}.php?db={1}&idfacolta={2}&idlaurea={3}&anno={4}", semestre, db, fac, laurea, anno);
            try
            {
                var httpClient = new HttpClient();
                Task<string> contentsTask = httpClient.GetStringAsync(new Uri(uri)); // async method!
                s = await contentsTask;
                s = s.Replace("&deg;", "°");
                s = s.Replace("&nbsp;", "");
                s = s.Replace("&agrave;", "à");
                s = s.Replace("&egrave;", "è");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return String.Empty;
            }

            return s;
        }

        public static List<CorsoGiornaliero> GetSingleOrarioGiornaliero(string html, int order, DateTime date)
        {
//            DbSQLite _db = new DbSQLite();
            List<CorsoGiornaliero> listaCorso = new List<CorsoGiornaliero>();
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            List<HtmlNode> row = doc.DocumentNode.Descendants().Where
                (x => (x.Name == "tr")).Skip(1).ToList();
            String[] corsi = new String[row.Count];

            foreach (var item in row)
            {
                int debug = 0;
                HtmlNode[] col = item.Descendants().Where(x => (x.Name == "td")).ToArray();
                if (col.Count() >= 4)
                {
                    CorsoGiornaliero orario = new CorsoGiornaliero()
                    {
                        Insegnamento = col[0].InnerText.Trim(),
                        Codice = col[1].InnerText.Trim(),
                        Docente = col[2].InnerText.Trim(),
                        AulaOra = col[3].InnerText.Trim(),
                        Note = col[4].InnerText.Trim(),
                        Date = date,
                    };
                    //if (_db.CheckAppartieneMieiCorsi(orario))
                    //    orario.MioCorso = true;
                    listaCorso.Add(orario);
                }
                else
                {
                    debug++;
                }
               
            }

            switch (order)
            {
                case 1:
                    return listaCorso.OrderBy(x => x.Ora.Split('-')[0]).ToList();
                default:
                    return listaCorso;
            }

            //return listaCorso;
        }

        public static List<CorsoCompleto> GetSingleOrarioCompleto(string html)
        {
            List<CorsoCompleto> listaCorso = new List<CorsoCompleto>();
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            List<HtmlNode> row = doc.DocumentNode.Descendants().Where
                (x => (x.Name == "tr")).Skip(1).ToList();
            String[] corsi = new String[row.Count];

            foreach (var item in row)
            {
                HtmlNode[] col = item.Descendants().Where(x => (x.Name == "td")).ToArray();
                //DA IMPLEMENTARE I GIORNI E GLI ORARI
                CorsoCompleto corso = new CorsoCompleto()
                {
                    Insegnamento = col[0].InnerText.Trim(),
                    Codice = col[1].InnerText.Trim(),
                    Docente = col[2].InnerText.Trim(),
                    //Lunedi = col[3].InnerText.Trim(),
                    //Martedi = col[4].InnerText.Trim(),
                    //Mercoledi = col[5].InnerText.Trim(),
                    //Giovedi = col[6].InnerText.Trim(),
                    //Venerdi = col[7].InnerText.Trim(),
                    //Sabato = col[8].InnerText.Trim(),
                    InizioFine = col[9].InnerText.Trim(),
                };
                for (int i = 3; i <= 8; i++)
                {
                    Lezione.Day _giorno;
                    switch (i-3)
                    {
                        case 0:
                            _giorno = Lezione.Day.Lunedi;
                            break;
                        case 1:
                            _giorno = Lezione.Day.Martedi;
                            break;
                        case 2:
                            _giorno = Lezione.Day.Mercoledi;
                            break;
                        case 3:
                            _giorno = Lezione.Day.Giovedi;
                            break;
                        case 4:
                            _giorno = Lezione.Day.Venerdi;
                            break;
                        default:
                            _giorno = Lezione.Day.Sabato;
                            break;
                    }
                    corso._lezioni.Add(new Lezione() { Giorno =  _giorno, AulaOra = col[i].InnerText.Trim() });
                    //corso.Giorni[i - 3] = col[i].InnerText.Trim();
                }

                listaCorso.Add(corso);
            }

            //var list = from corso in listaCorso
            //           from lez in corso.Lezioni
            //           where lez.AulaOra != string.Empty
            //           group corso by lez.Giorno;
            //var list = from corso in listaCorso
            //           group corso by corso.Docente;
            //return list;



            return listaCorso;
        }
        
    }
}
