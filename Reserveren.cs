using System;
using System.Collections.Generic;
using System.Text;

namespace TheFatDuckRestaurant
{
    public class Reserveren
    {
        public class Reservering
        {
            public string Tijd { get; set; }
            public string Datum { get; set; }
            public int Personen { get; set; }
        }
        public static void Reserveer()
        {
            Console.Clear();
            Console.WriteLine(ASCIIART.ReserverenArt());
            Console.WriteLine("Welke datum wilt u reserveren? (20 juli)");
            Tuple<bool,string> Datum = CheckDatum(Console.ReadLine());
            if (Datum.Item1)
            {
                Console.Clear();
                Console.WriteLine(ASCIIART.ReserverenArt());
                Console.WriteLine(Datum.Item2 + "\x0aHoe laat wilt u reserveren? (11:00 - 21:00)");
                Tuple<bool,string> Tijd = CheckTijd(Console.ReadLine());
                if (Tijd.Item1)
                {
                    Console.Clear();
                    Console.WriteLine(ASCIIART.ReserverenArt());
                    Console.WriteLine(Tijd.Item2 + "\x0a"+"Er zijn 100 plaatsen vrij\x0aMet hoeveel personen bent u?");
                    Tuple<bool, int> Personen = CheckPersonen(Console.ReadLine());
                    if (Personen.Item1)
                    {
                        Console.Clear();
                        Console.WriteLine(ASCIIART.ReserverenArt());
                        Console.WriteLine("1: Bekijk de reservering\x0a" + "2: Bevestig de reservering\x0a" + "3: Annuleer de reservering");
                        ConsoleKeyInfo userInput = Console.ReadKey();
                        char userInputChar = userInput.KeyChar;
                        Console.Clear();
                        switch (userInputChar)
                        {
                            case '1':
                                Console.WriteLine($"{Datum.Item2}\x0a{Tijd.Item2}\x0a{Personen.Item2}\x0a<Menu items en rekening>\x0a\x0a" + "1: Pas de reservering aan\x0a" +"2: Ga terug naar het vorige scherm");
                                userInput = Console.ReadKey();
                                userInputChar = userInput.KeyChar;
                                switch (userInputChar)
                                {
                                    case '1':
                                        Console.WriteLine("<Reservering aanpassen>");
                                        break;
                                    case '2':
                                        Console.WriteLine("<Terug naar het vorige scherm");
                                        break;
                                }
                                break;
                            case '2':
                                Console.WriteLine("U heeft gereserveerd\x0a" );
                                break;
                            case '3':
                                Console.WriteLine("De reservering is geannuleerd\x0a");
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Er zijn niet genoeg plaatsen beschikbaar om deze tijd");
                    }
                }
                else
                {
                    Console.WriteLine("Deze tijd valt niet binnen de openingsuren");
                }
            }
            else
            {
                Console.WriteLine("Deze datum bestaat niet");
            }
            Console.WriteLine("Enter: Ga terug naar het startscherm");
            Console.ReadKey();
            /*
            Console.WriteLine("Reserveren\x0a");
            string[] Dagen = Datums(DateTime.Now);
            for (int i = 0; i < Dagen.Length; i++)
            {
                Console.WriteLine($"{i}: Reserveer voor {Dagen[i]}\x0a");
            }
            if (int.TryParse(Console.ReadLine(), out int Keuze)){
                Console.Clear();
                Console.WriteLine($"{Dagen[Keuze]}\x0a");
                string[] Beschikbaar = Tijden(Dagen[Keuze]);
                for(int i = 0; i < Beschikbaar.Length; i++)
                {
                    Console.WriteLine($"{i}: Reserveer voor {Beschikbaar[i]} ({Plaatsen(Dagen[Keuze],Beschikbaar[i])} vrije plekken)\x0a");
                }
                if (int.TryParse(Console.ReadLine(), out int Keuze2))
                {
                    Console.Clear();
                    Console.WriteLine($"{Dagen[Keuze]} om {Beschikbaar[Keuze2]}\x0a\x0aMet hoeveel personen bent u?");
                    if (int.TryParse(Console.ReadLine(), out int Personen) && Personen > 0)
                    {
                        Console.Clear();
                        Console.WriteLine($"U heeft gereserveerd voor {Dagen[Keuze]} om {Beschikbaar[Keuze2]} voor {Personen}" + (Personen == 1 ? " persoon" : " personen") + "\x0a\x0a" + "Enter: Ga terug naar het beginscherm");
                    }
                    else
                    {
                        Console.WriteLine("Verkeerde input\x0a" + "Enter: Ga terug naar het beginscherm");
                    }
                }
                else
                {
                    Console.WriteLine("Verkeerde input\x0a"+"Enter: Ga terug naar het beginscherm");
                }
            }
            else
            {
                Console.WriteLine("Verkeerde input\x0a"+"Enter: Ga terug naar het beginscherm");
            }
            Console.ReadKey();*/
        }
        
        public static Tuple<bool,int> CheckPersonen(string P)
        {
            string Personen = "";
            foreach(char sym in P)
            {
                if (Char.IsDigit(sym))
                {
                    Personen += sym;
                }
            }
            int PersInt = Personen != "" ? Int32.Parse(Personen) : 101;
            if(PersInt <= 100)
            {
                return Tuple.Create(true, PersInt);
            }
            Console.WriteLine("Er zijn niet genoeg plaatsen beschikbaar om deze tijd");
            return Tuple.Create(false, 0);
        }

        public static Tuple<bool,string> CheckTijd(string T)
        {
            string Tijd = "";
            foreach(char sym in T)
            {
                if (Char.IsDigit(sym))
                {
                    Tijd += sym;
                }
            }
            int TijdInt = Tijd != "" ? Int32.Parse(Tijd): 0;
            TijdInt *= TijdInt < 100 ? 100 : 1;
            if(TijdInt <= 2100 && TijdInt >= 1100)
            {
                string Minuten = TijdInt % 100 == 0 ? "00" : $"{TijdInt % 100}";
                return Tuple.Create(true, $"{TijdInt / 100}:{Minuten}");
            }
            Console.WriteLine("Deze tijd is ongeldig");
            return Tuple.Create(false, "");
        }
        public static Tuple<bool,string> CheckDatum(string Datum)
        {
            string Dag = "";
            string Maand = "";
            foreach(char sym in Datum)
            {
                if (Char.IsDigit(sym))
                {
                    Dag += sym;
                }
                else if(Char.IsLetter(sym))
                {
                    Maand += sym;
                }
            }
            int DagInt = Dag != "" ? Int32.Parse(Dag) : 0;
            if (CheckMaand(Maand) && DagInt > 0 && DagInt < 32)
            {
                if(CheckDag(DagInt, Maand.ToLower(),DateTime.Now.Year))
                {
                    return Tuple.Create(true,$"{Dag} {Maand}");
                }
            }
            return Tuple.Create(false,"");
        }

        public static bool CheckMaand(string Maand)
        {
            string[] Maanden = new string[] { "januari", "februari", "maart", "april", "mei", "juni", "juli", "augustus", "september", "oktober", "november", "december" };
            for(int i = 0; i < Maanden.Length; i++)
            {
                if(Maand.ToLower() == Maanden[i])
                {
                    return true;
                }
            }
            return false;
        }
        public static bool CheckDag(int Dag, string Maand, int Jaar)
        {
            if(Maand == "januari" || Maand == "maart" || Maand == "mei" || Maand == "juli" || Maand == "augustus" || Maand == "oktober" || Maand == "november")
            {
                return true;
            }
            if(Maand == "februari")
            {
                if(Jaar % 4 == 0 && (Jaar % 100 != 0 || Jaar % 400 == 0))
                {
                    return Dag < 30 ? true : false;
                }
                return Dag < 29 ? true : false;
            }
            return Dag < 31 ? true : false;
        }
        public static int Plaatsen(string Datum, string Tijd)
        {
            return 100;
        }
        public static string[] Tijden(string Datum)
        {
            string[] Tijdarr = new string[11];
            for(int i = 0, j = 11;i < 11; i++,j++)
            {
                Tijdarr[i] = $"{j}:00";
            }
            return Tijdarr;
        }
        public static string[] Datums(DateTime Date)
        {
            int Day = Date.Day, Month = Date.Month, Year = Date.Year;
            string[] Dagen = new string[14];
            Dagen[0] = $"{Day}/{Month}/{Year}";
            for (int i = 1; i < 14; i++)
            {
                Day++;
                if (Month == 1 || Month == 3 || Month == 5 || Month == 7 || Month == 8 || Month == 10 || Month == 12)
                {
                    if (Day > 31)
                    {
                        Day = 1;
                        if (Month == 12)
                        {
                            Month = 1;
                            Year++;
                        }
                        else
                        {
                            Month++;
                        }
                    }
                }
                else if (Month == 2)
                {
                    if (Year % 4 == 0 && (Year % 100 != 0 || Year % 400 == 0))
                    {
                        if (Day > 29)
                        {
                            Day = 1;
                            Month++;
                        }
                    }
                    else if (Day > 28)
                    {
                        Day = 1;
                        Month++;
                    }
                }
                else
                {
                    if (Day > 30)
                    {
                        Day = 1;
                        Month++;
                    }
                }
                Dagen[i] = $"{Day}/{Month}/{Year}";
            }
            return Dagen;
        }
    }
}
