using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrariUnibg.Models;

namespace OrariUnibg.Helpers
{
    public static class LaureeDictionary
    {
        static Laurea laurea;
        public static Dictionary<string, int> getLauree(Facolta facolta)
        {
            List<Facolta> list = Facolta.facolta;
            var f = list.Where(x => x == facolta).First();
            int index = list.IndexOf(f);
            switch (index)
            {
                case 0 :
                    return getLaureeIngegneria();
                case 1:
                    return getLaureeLettFilo();
                case 2:
                    return getLaureeGiurisprudenza();
                case 3:
                    return getLaureeLingLettStranCom();
                case 4:
                    return getLaureeEconomia();
                case 5:
                    return getLaureeScienzUmane();
                //case 6:
                //    return getLaureeCis();
                default:
                    return null;  
            }
        }

        private static Dictionary<string, int> getLaureeIngegneria()
        {
            var dictionary = new Dictionary<string, int>();

            laurea = new Laurea() { Nome = "Generale", Id = 0 };
            dictionary.Add(laurea.Nome, laurea.Id);
            laurea = new Laurea() { Nome = "Ingegneria Edile", Id = 5, Anni = 3 };
            dictionary.Add(laurea.Nome, laurea.Id);
            laurea = new Laurea() { Nome = "Ingegneria Gestionale", Id = 1, Anni = 3 };
            dictionary.Add(laurea.Nome, laurea.Id);
            laurea = new Laurea() { Nome = "Ingegneria Informatica", Id = 3, Anni = 3 };
            dictionary.Add(laurea.Nome, laurea.Id);
            laurea = new Laurea() { Nome = "Ingegneria Meccanica", Id = 4, Anni = 3 };
            dictionary.Add(laurea.Nome, laurea.Id);
            laurea = new Laurea() { Nome = "LM - Ingegneria Edile", Id = 17, Anni = 2 };
            dictionary.Add(laurea.Nome, laurea.Id);
            laurea = new Laurea() { Nome = "LM - Ingegneria Gestionale", Id = 18, Anni = 2 };
            dictionary.Add(laurea.Nome, laurea.Id);
            laurea = new Laurea() { Nome = "LM - Ingegneria Informatica", Id = 19, Anni = 2 };
            dictionary.Add(laurea.Nome, laurea.Id);
            laurea = new Laurea() { Nome = "LM - Ingegneria Meccanica", Id = 20, Anni = 2 };
            dictionary.Add(laurea.Nome, laurea.Id);
            laurea = new Laurea() { Nome = "Management Engineering", Id = 21, Anni = 2 };
            dictionary.Add(laurea.Nome, laurea.Id);
			laurea = new Laurea() { Nome = "Tecnologie per la salute", Id = 22, Anni = 3 };
			dictionary.Add(laurea.Nome, laurea.Id);

            return dictionary;
        }

        private static Dictionary<string, int> getLaureeLettFilo()
        {
            var dictionary = new Dictionary<string, int>();
            laurea = new Laurea() { Nome = "Generale", Id = 0 };
            dictionary.Add(laurea.Nome, laurea.Id);
            laurea = new Laurea() { Nome = "Filosofia", Id = 59, Anni = 3 };
            dictionary.Add(laurea.Nome, laurea.Id);
            laurea = new Laurea() { Nome = "Lettere", Id = 48, Anni = 3 };
            dictionary.Add(laurea.Nome, laurea.Id);
            laurea = new Laurea() { Nome = "LM-Culture Moderne Comparate", Id = 49, Anni = 2 };
            dictionary.Add(laurea.Nome, laurea.Id);
            laurea = new Laurea() { Nome = "LM-Diritti uomo ed etica della coop. int.", Id = 50, Anni = 2 };
            dictionary.Add(laurea.Nome, laurea.Id);

            return dictionary;
        }

        private static Dictionary<string, int> getLaureeGiurisprudenza()
        {
            var dictionary = new Dictionary<string, int>();
            laurea = new Laurea() { Nome = "Generale", Id = 0 };
            dictionary.Add(laurea.Nome, laurea.Id);
            laurea = new Laurea() { Nome = "Magistrale", Id = 23, Anni = 5 };
            dictionary.Add(laurea.Nome, laurea.Id);
            laurea = new Laurea() { Nome = "Operatore Giuridico d'Impresa", Id = 31, Anni = 3 };
            dictionary.Add(laurea.Nome, laurea.Id);
            return dictionary;
        }

        private static Dictionary<string, int> getLaureeLingLettStranCom()
        {
            var dictionary = new Dictionary<string, int>();
            laurea = new Laurea() { Nome = "Generale", Id = 0 };
            dictionary.Add(laurea.Nome, laurea.Id);
            laurea = new Laurea() { Nome = "Lingue e Letterature Straniere Moderne", Id = 51, Anni = 3 };
            dictionary.Add(laurea.Nome, laurea.Id);
            laurea = new Laurea() { Nome = "Scienze della Comunicazione", Id = 57, Anni = 3 };
            dictionary.Add(laurea.Nome, laurea.Id);
            laurea = new Laurea() { Nome = "LM-Comunicazione, Informazione, Editoria", Id = 58, Anni = 2 };
            dictionary.Add(laurea.Nome, laurea.Id);
            laurea = new Laurea() { Nome = "LM-Lingue Moderne per la com. e coop. int.", Id = 53, Anni = 2 };
            dictionary.Add(laurea.Nome, laurea.Id);
            laurea = new Laurea() { Nome = "LM-Progett. e Gest. dei Sistemi Turistici", Id = 52, Anni = 2 };
            dictionary.Add(laurea.Nome, laurea.Id);
            laurea = new Laurea() { Nome = "LM-Lingue e Lett. Europee e panamericane", Id = 54, Anni = 2 };
            dictionary.Add(laurea.Nome, laurea.Id);

            return dictionary;
        }

        private static Dictionary<string, int> getLaureeEconomia()
        {
            var dictionary = new Dictionary<string, int>();
            laurea = new Laurea() { Nome = "Generale", Id = 0 };
            dictionary.Add(laurea.Nome, laurea.Id);
            laurea = new Laurea() { Nome = "Economia", Id = 38, Anni = 3 };
            dictionary.Add(laurea.Nome, laurea.Id);
            laurea = new Laurea() { Nome = "Economia aziendale", Id = 10, Anni = 3 };
            dictionary.Add(laurea.Nome, laurea.Id);
            laurea = new Laurea() { Nome = "Amm., contabilità e controllo aziende", Id = 42, Anni = 3 };
            dictionary.Add(laurea.Nome, laurea.Id);
            laurea = new Laurea() { Nome = "Comm. estero e mercati finanziari", Id = 43, Anni = 3 };
            dictionary.Add(laurea.Nome, laurea.Id);
            laurea = new Laurea() { Nome = "Direzione d'impresa", Id = 44, Anni = 3 };
            dictionary.Add(laurea.Nome, laurea.Id);
            laurea = new Laurea() { Nome = "LM-Economia aziendale, dir. amm. e prof.", Id = 39, Anni = 2 };
            dictionary.Add(laurea.Nome, laurea.Id);
            laurea = new Laurea() { Nome = "LM-Economics and Global Markets", Id = 40, Anni = 2 };
            dictionary.Add(laurea.Nome, laurea.Id);
            laurea = new Laurea() { Nome = "LM-Management, Finance and Int. Business", Id = 46, Anni = 2 };
            dictionary.Add(laurea.Nome, laurea.Id);
            laurea = new Laurea() { Nome = "LM-International Business and Financ)", Id = 30, Anni = 2 };
            dictionary.Add(laurea.Nome, laurea.Id);
            laurea = new Laurea() { Nome = "LM-Management, Leadership and Marketing ", Id = 45, Anni = 2 };
            dictionary.Add(laurea.Nome, laurea.Id);

            return dictionary;
        }

        private static Dictionary<string, int> getLaureeScienzUmane()
        {
            var dictionary = new Dictionary<string, int>();
            laurea = new Laurea() { Nome = "Generale", Id = 0 };
            dictionary.Add(laurea.Nome, laurea.Id);
            laurea = new Laurea() { Nome = "Scienze dell'Educazione", Id = 46, Anni = 3};
            dictionary.Add(laurea.Nome, laurea.Id);
            laurea = new Laurea() { Nome = "Scienze Psicologiche", Id = 44, Anni = 3};
            dictionary.Add(laurea.Nome, laurea.Id);
            laurea = new Laurea() { Nome = "LM - Psicologia Clinica", Id = 45, Anni = 2 };
            dictionary.Add(laurea.Nome, laurea.Id);
            laurea = new Laurea() { Nome = "LM - Scienze Pedagogiche", Id = 47, Anni = 2 };
            dictionary.Add(laurea.Nome, laurea.Id);
            laurea = new Laurea() { Nome = "Master's Degree", Id = 60, Anni = 2 };
            dictionary.Add(laurea.Nome, laurea.Id);

            return dictionary;
        }

        private static Dictionary<string, int> getLaureeCis()
        {
            var dictionary = new Dictionary<string, int>();
            laurea = new Laurea() { Nome = "Generale", Id = 0 };
            dictionary.Add(laurea.Nome, laurea.Id);

            return dictionary;
        }








        
    }
}
