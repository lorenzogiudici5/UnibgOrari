using OrariUnibg.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrariUnibg.Helpers
{
    public static class AlgoritmoCorsi
    {
        public static IEnumerable<CorsoCompleto> SuggerisciCorsi(List<CorsoCompleto> corsiObbligatori, List<CorsoCompletoAlgoritmo> corsiScelta, int numCorsi, int punteggioMezzora, int punteggioGiorno)
        {
            //  ho già  raggiunto il numero di corsi scelti dall'utente ritorno i corsi obbligatori
            if (corsiObbligatori.Count() >= numCorsi)
                return corsiObbligatori;

            //se somma corsi obbligatori + corsi a scelta minore o uguale del numero dei corsi, return
            if (corsiObbligatori.Count() + corsiScelta.Count() <= numCorsi)
            {
                corsiObbligatori.AddRange(corsiScelta);
                return corsiObbligatori;
            }
                

            // Le 5 liste successive serviranno per dividere le lezione dei corsi obbligatori in giorni
            List<Lezione> lunedi = new List<Lezione>();
            List<Lezione> martedi = new List<Lezione>();
            List<Lezione> mercoledi = new List<Lezione>();
            List<Lezione> giovedi = new List<Lezione>();
            List<Lezione> venerdi = new List<Lezione>();

            List<List<Lezione>> giorniLezioni = new List<List<Lezione>>()
            {
                lunedi, martedi, mercoledi, giovedi, venerdi
            };

            // popolo le liste dividendo le lezioni dei corsi obbligatori in diverse liste una per giorno
            foreach (var corso in corsiObbligatori)
            {
                foreach (var lezione in corso.Lezioni)
                    if (lezione.AulaOra == string.Empty)
                        continue;
                    else
                        giorniLezioni[lezione.day].Add(lezione);
            }

            while (corsiObbligatori.Count() != numCorsi)
            {
                //ora si aggiungono ai corsi obbligatori i corsiAScelta con punteggioScelta minore
                foreach (var scelta in corsiScelta)
                {
                    scelta.Punteggio = 0;                                   // setto i punteggi di ogni corso a zero

                    foreach (var lezione in scelta.Lezioni)
                    {
                        if (lezione.AulaOra == string.Empty)
                            continue;

                        var lezioniGiorno = giorniLezioni[lezione.day];     //prendo lista delle lezioni del giorno corrispondente
                        if (lezioniGiorno.Count == 0)                       //non ci sono lezioni obbligatorie quel giorno
                            scelta.Punteggio += punteggioGiorno;            //penalità punteggio giorno
                        else
                        {
                            foreach (var lezioneObbligatoria in lezioniGiorno)
                            {
                                var differenza = confrontaOrari(lezione.OraInizio, lezione.OraFine, lezioneObbligatoria.OraInizio, lezioneObbligatoria.OraFine);
                                scelta.Punteggio += (int)differenza * punteggioMezzora;

                                //if (lezione.OraInizio > lezioneObbligatoria.OraFine || //non si sovrappone
                                //    lezione.OraFine < lezioneObbligatoria.OraInizio)
                                //    scelta.Punteggio += 0;
                                //else //calcolo di quanto è la sovrapposizione
                                //{
                                //    var differenza = confrontaOrari(lezione.OraInizio, lezioneObbligatoria.OraFine);
                                //    scelta.Punteggio += (int)differenza * punteggioMezzora;
                                //}
                            }
                        }
                    }
                }

                corsiScelta = corsiScelta.OrderBy(x => x.Punteggio).ToList();              // Si ordina in base al punteggioScelta in modo crescente

                var corsoPrescelto = corsiScelta.First();
                corsiObbligatori.Add(corsoPrescelto);               //aggiungo ai corsi obbligatori il primo corso a scelta ordinato in base al punteggio

                foreach (var lezione in corsoPrescelto.Lezioni)      //aggiungo le lezioni del primo corso a scelta alle varie liste dei giorni
                {
                    if (lezione.AulaOra == string.Empty)
                        continue;
                    else
                        giorniLezioni[lezione.day].Add(lezione);
                }

                // tolgo dalla lista corsi a scelta il corso aggiunto ai corsi obbligatori, ripeto il tutto con un altro 
                // corso a scelta finchÃ¨ non raggiungo il numero di corsi scelti dall'utente.
                corsiScelta.Remove(corsoPrescelto);
            }

            return corsiObbligatori;
        }

        private static double confrontaOrari(DateTime? Scelta_oraInizio, DateTime? Scelta_oraFine, DateTime? Obb_oraInizio, DateTime? Obb_oraFine)
        {
            TimeSpan? span = new TimeSpan(0);

            // il corso a scelta inizia prima del corso obbligatorio ma c'è sovrapposizione
            if (Obb_oraInizio > Scelta_oraInizio && Scelta_oraFine > Obb_oraInizio && Obb_oraFine > Scelta_oraFine)
                span = Scelta_oraFine - Obb_oraInizio;
            // il corso a scelta inizia dopo il corso obbligatorio ma c'è sovrapposizione
            else if (Scelta_oraInizio > Obb_oraInizio && Obb_oraFine > Scelta_oraInizio && Scelta_oraFine > Obb_oraFine)
                span = Obb_oraFine - Scelta_oraInizio;
            // il corso obbligatorio ha orario inglobato nel corso a scelta
            else if (Obb_oraInizio > Scelta_oraInizio && Scelta_oraFine > Obb_oraFine )
                span = Obb_oraFine - Obb_oraInizio;
            // il corso a scelta ha orario inglobato nel corso obbligatorio
            else if (Scelta_oraInizio > Obb_oraInizio && Obb_oraFine > Scelta_oraFine)
                span = Scelta_oraFine - Scelta_oraInizio;

            return (span.Value.TotalMilliseconds / (60 * 1000)) / 30;
        }
    }
}

