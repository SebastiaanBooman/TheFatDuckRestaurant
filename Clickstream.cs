using System;

namespace TheFatDuckRestaurant
{
    public class Clickstream
    {
        public CSDag[] Dagen { get; set; }
        public CSTijd[] Tijden { get; set; }
        public Clickstream() { }
        public void addClickstream(string Datum, int tijd) //voegt de clickstream toe wanneer een reservering gemaakt is
        {
            string DigitCheck = "";
            string dag = "";
            foreach(char sym in Datum) //haalt de dag van de week uit de datumstring
            {
                if (Char.IsDigit(sym))
                    DigitCheck += sym;
                if(Char.IsLetter(sym) && DigitCheck == "")
                    dag += sym;
            }
            addClicks(dag.ToLower(), tijd);
        }
        private void addClicks(string dag, int tijd) //verhoogt de juiste clickstream qua dag en tijd
        {
            foreach(CSDag Dag in Dagen)
            {
                if(Dag.Naam == dag)
                    Dag.Clicks++;
            }
            foreach(CSTijd Tijd in Tijden)
            {
                if(tijd > Tijd.Naam - 70 && tijd <= Tijd.Naam + 30) //voegt toe aan het dichtstbijzijnde uur
                    Tijd.Clicks++;
            }
        }
        public void bekijkClicks() //laat de clickstream per dag vd week of tijd zien
        {
            char Input = '1';
            while (Input != '0')
            {
                Console.Clear();
                Console.WriteLine(ASCIIART.ReserveringenArt());
                Console.WriteLine("1: Bekijk de clickstream per dag van de week\n2: Bekijk de clickstream per uur\n\n0: Ga terug naar het vorige scherm");
                Input = Console.ReadKey().KeyChar;
                switch (Input)
                {
                    case '1':
                        bekijkClicksD();
                        break;
                    case '2':
                        bekijkClicksT();
                        break;
                    case '0':
                        break;
                    default:
                        Console.WriteLine("Dit is geen geldige input\x0a\x0a" + "Enter: Ga terug naar het vorige scherm");
                        Console.ReadKey();
                        break;
                }
            }
        }
        private void bekijkClicksD() //laat de clickstream per dag van de week zien
        {
            Console.Clear();
            Console.WriteLine(ASCIIART.ReserveringenArt());
            int NietAanwezig = 0;
            foreach (CSDag Dag in Dagen)
            {
                if (Dag.Clicks > 0)
                    Console.WriteLine(Dag.Naam + $": {Dag.Clicks} reserveringen\n");
                else
                    NietAanwezig++;
            }
            if (NietAanwezig == Dagen.Length)
                Console.WriteLine("Er zijn nog geen reserveringen gemaakt");
            Console.WriteLine("\nEnter: Ga terug naar het vorige scherm");
            Console.ReadKey();
        }
        private void bekijkClicksT() //laat de clickstream per uur zien
        {
            Console.Clear();
            Console.WriteLine(ASCIIART.ReserveringenArt());
            int NietAanwezig = 0;
            foreach (CSTijd Tijd in Tijden)
            {
                if (Tijd.Clicks > 0)
                    Console.WriteLine(Tijd.Naam + $": {Tijd.Clicks} reserveringen\n");
                else { NietAanwezig++; }
            }
            if(NietAanwezig == Tijden.Length)
                Console.WriteLine("Er zijn nog geen reserveringen gemaakt");
            Console.WriteLine("\nEnter: Ga terug naar het vorige scherm");
            Console.ReadKey();
        }

    }
    public class CSDag
    {
        public string Naam { get; set; }
        public int Clicks { get; set; }
        public CSDag() { }
    }
    public class CSTijd
    {
        public int Naam { get; set; }
        public int Clicks { get; set; }
        public CSTijd() { }
    }
}