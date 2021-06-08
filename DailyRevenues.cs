using System;

namespace TheFatDuckRestaurant
{
    public class DailyRevenues
    {
        public DailyRevenue[] Revenues { get; set; }
        public DailyRevenues() { }
        /// <summary>
        /// Laat de opbrengst van een gegeven datum zien
        /// </summary>
        /// <param name="reserveerlijst">Array van reserveringen</param>
        public void bekijkRevenue(TheFatDuckRestaurant.Reservering[] reserveerlijst)
        {
            string Datum = "";
            while (Datum != "0")
            {
                Console.Clear();
                Console.WriteLine(ASCIIART.OpbrengstenArt());
                Console.WriteLine("Voor welke dag wilt u de opbrengst bekijken? (21 juni 2021)\n\n0: Ga terug naar het startscherm");
                Datum = Console.ReadLine();
                Console.Clear();
                if (Datum != "0")
                {
                    Datum = CheckDatum.checkDatum(Datum, false);
                    if (Datum != null)
                    {
                        if (!oudeRevenue(Datum))
                        {
                            double Revenue = 0;
                            Console.OutputEncoding = System.Text.Encoding.UTF8;
                            foreach (TheFatDuckRestaurant.Reservering reservering in reserveerlijst)
                            {
                                if (Datum == reservering.Datum)
                                {
                                    for (int i = 0; i < reservering.Bestelling.Count; i++)
                                        Revenue += reservering.Bestelling[i].Prijs * reservering.Bestelling[i].Aantal;
                                }
                            }
                            string revenue = "" + Revenue;
                            revenue += (!revenue.Contains(',') ? ",-" : revenue[revenue.Length - 2] == ',' ? "0" : "");
                            Console.WriteLine(ASCIIART.OpbrengstenArt());
                            Console.Out.WriteLine($"De opbrengst van {Datum} is €{revenue}\n\nEnter: Ga terug naar het vorige scherm");
                            Console.ReadKey();
                        }
                    }
                    else
                    {
                        Console.WriteLine(ASCIIART.OpbrengstenArt());
                        Console.WriteLine("Deze datum bestaat niet!\nKlik op een toets om terug te gaan");
                        Console.ReadKey();
                    }
                }
            }
        }
        /// <summary>
        /// Checkt of de datum in het JSON-bestand staat
        /// Zoja, dan wordt de opbrengst van deze dag getoond
        /// </summary>
        /// <param name="Datum"></param>
        /// <returns>true als de datum bestaat in het JSON-bestand, anders false</returns>
        private bool oudeRevenue(string Datum)
        {
            if (Revenues != null)
            {
                Console.OutputEncoding = System.Text.Encoding.UTF8;
                foreach (DailyRevenue dailyrevenue in Revenues)
                {
                    if (dailyrevenue.Datum == Datum)
                    {
                        Console.Clear();
                        Console.WriteLine(ASCIIART.OpbrengstenArt());
                        string revenue = "" + dailyrevenue.Revenue;
                        revenue += (!revenue.Contains(',') ? ",-" : revenue[revenue.Length -2] == ',' ? "0" : "");
                        Console.Out.WriteLine($"De opbrengst van {Datum} is €{revenue}\n\nEnter: Ga terug naar het vorige scherm");
                        Console.ReadKey();
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Voegt de opbrengst van de reservering toe aan het JSON-bestand
        /// Verhoogt het bedrag als de datum al in het bestand staat, anders wordt een nieuw item aangemaakt voor het bestand
        /// </summary>
        /// <param name="Datum">Datum van de reservering</param>
        /// <param name="Revenue">Totale opbrengst van de reservering</param>
        public void Add(string Datum, double Revenue)
        {
            bool Exists = false;
            if(Revenues == null)
                Revenues = new DailyRevenue[0];
            foreach(DailyRevenue dailyrevenue in Revenues)
            {
                if(dailyrevenue.Datum == Datum && !Exists)
                {
                    Exists = true;
                    dailyrevenue.Revenue += Revenue;
                }
            }
            if (!Exists)
            {
                DailyRevenue[] newRevenues = new DailyRevenue[Revenues.Length + 1];
                for(int i = 0; i < Revenues.Length; i++)
                    newRevenues[i] = Revenues[i];
                newRevenues[Revenues.Length] = new DailyRevenue(Datum, Revenue);
                Revenues = newRevenues;
            }
        }
    }
    public class DailyRevenue
    {
        public string Datum { get; set; }
        public double Revenue { get; set; }
        public DailyRevenue() { }
        public DailyRevenue(string datum, double revenue)
        {
            Datum = datum;
            Revenue = revenue;
        }
    }
}