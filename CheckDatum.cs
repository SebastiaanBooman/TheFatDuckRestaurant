using System;

namespace TheFatDuckRestaurant
{
    static class CheckDatum
    {
        /// <summary>
        /// Checkt of de gegeven datum bestaat en voegt de weekdag en eventueel het jaar toe aan de string
        /// </summary>
        /// <param name="Datum">Datum die gecheckt moet worden</param>
        /// <param name="reserveren">Geeft aan of het voor een reservering gebeurt of niet</param>
        /// <returns>null als de datum ongeldig is, anders een geldige datum-string voor deze applicatie</returns>
        public static string checkDatum(string Datum, bool reserveren = true)
        {
            string Dag = "";
            string Maand = "";
            string jaar = "";
            foreach (char sym in Datum)
            {
                if (Char.IsDigit(sym) && Maand == "")
                    Dag += sym;
                else if (Char.IsDigit(sym) && Maand != "")
                    jaar += sym;
                else if (Char.IsLetter(sym) && Dag != "")
                    Maand += sym;
            }
            int DagInt = Dag != "" ? Int32.Parse(Dag) : 0;
            int.TryParse(jaar, out int Jaar);
            if (Jaar <= 0) { Jaar = DateTime.Now.Year; }
            if (CheckMaand(Maand.ToLower()) && DagInt > 0 && DagInt < 32)
            {
                if (reserveren)
                {
                    if (MaandInt(Maand.ToLower()) < DateTime.Now.Month || (MaandInt(Maand.ToLower()) == DateTime.Now.Month && DagInt < DateTime.Now.Day))
                        Jaar += 1;
                }
                if (CheckDag(DagInt, Maand.ToLower(), Jaar))
                    return $"{WeekDag(DagInt, Maand, Jaar, reserveren)} {Dag} {Maand} {Jaar}";
            }
            return null;
        }
        /// <summary>
        /// Verkrijgt mbv de dag, maand en het jaar de dag van de week
        /// </summary>
        /// <param name="Dag">Dag van de maand</param>
        /// <param name="maand">Maand</param>
        /// <param name="Jaar">Jaar</param>
        /// <param name="reserveren">Geeft aan of het voor een reservering gebeurt of niet</param>
        /// <returns>De dag van de week als string</returns>
        private static string WeekDag(int Dag, string maand, int Jaar, bool reserveren)
        {
            int Maand = MaandInt(maand);
            int HuidigeDag = DateTime.Now.Day;
            int HuidigeMaand = DateTime.Now.Month;
            DateTime date;
            if ((Maand < HuidigeMaand || (Maand == HuidigeMaand && Dag < HuidigeDag)) && reserveren)
                date = new DateTime(Jaar + 1, Maand, Dag);
            else
                date = new DateTime(Jaar, Maand, Dag);
            string WeekDay = "" + date.DayOfWeek;
            return DaytoDag(WeekDay.ToLower());
        }
        /// <summary>
        /// Zet de Engelse naam van de dag van de week om naar de Nederlandse
        /// </summary>
        /// <param name="Day">Engelse dag van de week</param>
        /// <returns>Nederlandse dag van de week</returns>
        private static string DaytoDag(string Day)
        {
            return Day == "monday" ? "Maandag" :
                Day == "tuesday" ? "Dinsdag" :
                Day == "wednesday" ? "Woensdag" :
                Day == "thursday" ? "Donderdag" :
                Day == "friday" ? "Vrijdag" :
                Day == "saturday" ? "Zaterdag" : "Zondag";
        }
        /// <summary>
        /// Zoekt op de hoeveelste maand de parameter is
        /// </summary>
        /// <param name="maand">Naam van de maand</param>
        /// <returns>De hoeveelste maand het is</returns>
        private static int MaandInt(string maand)
        {
            string[] Maanden = new string[] { "januari", "februari", "maart", "april", "mei", "juni", "juli", "augustus", "september", "oktober", "november", "december" };
            for (int i = 0; i < Maanden.Length; i++)
            {
                if (maand == Maanden[i])
                    return i + 1;
            }
            return 0;
        }
        /// <summary>
        /// Checkt of de gegeven maand een geldige maand is
        /// </summary>
        /// <param name="maand">Naam van de maand</param>
        /// <returns>true als het een geldige maand is, anders false</returns>
        private static bool CheckMaand(string maand)
        {
            string[] Maanden = new string[] { "januari", "februari", "maart", "april", "mei", "juni", "juli", "augustus", "september", "oktober", "november", "december" };
            for (int i = 0; i < Maanden.Length; i++)
            {
                if (maand == Maanden[i])
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Checkt of de gegeven dag bestaat voor de gegeven maand en het gegeven jaar
        /// </summary>
        /// <param name="Dag">Dag van de maand</param>
        /// <param name="Maand">Naam van de maand</param>
        /// <param name="Jaar">Jaar</param>
        /// <returns>true als als de dag bestaat, anders false</returns>
        private static bool CheckDag(int Dag, string Maand, int Jaar)
        {
            if (Maand == "januari" || Maand == "maart" || Maand == "mei" || Maand == "juli" || Maand == "augustus" || Maand == "oktober" || Maand == "november")
                return true;
            if (Maand == "februari")
            {
                if (Jaar % 4 == 0 && (Jaar % 100 != 0 || Jaar % 400 == 0))
                    return Dag < 30 ? true : false;
                return Dag < 29 ? true : false;
            }
            return Dag < 31 ? true : false;
        }
        /// <summary>
        /// Checkt of de gegeven datum al geweest is
        /// </summary>
        /// <param name="Datum">Datum die gecheckt moet worden</param>
        /// <returns>true als de datum al geweest is, anders false</returns>
        public static bool DatumGeweest(string Datum) //checkt of een datum geweest is
        {
            string Dag = "";
            string Maand = "";
            string Jaar = "";
            foreach (char sym in Datum)
            {
                if (Char.IsDigit(sym) && Maand == "")
                    Dag += sym;
                else if (Char.IsDigit(sym))
                    Jaar += sym;
                else if (Char.IsLetter(sym) && Dag != "")
                    Maand += sym;
            }
            if (Int32.Parse(Jaar) < DateTime.Now.Year)
                return true;

            string[] Maanden = new string[] { "januari", "februari", "maart", "april", "mei", "juni", "juli", "augustus", "september", "oktober", "november", "december" };
            int Maandint = 0;
            for (int i = 0; i < Maanden.Length; i++)
            {
                if (Maanden[i] == Maand)
                    Maandint = i + 1;
            }
            if (Int32.Parse(Jaar) == DateTime.Now.Year)
            {
                if (Maandint < DateTime.Now.Month || (Maandint == DateTime.Now.Month && Int32.Parse(Dag) < DateTime.Now.Day))
                    return true;
            }
            return false;
        }
    }
}
