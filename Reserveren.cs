using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TheFatDuckRestaurant
{
    class Reserveren
    {
        public class ReserveerLijst
        {
            public Reservering[] Reserveringen { get; set; }
        }
        public class Reservering
        {
            public int Tijd { get; set; }
            public string Datum { get; set; }
            public int Personen { get; set; }
        }
        public static void Reserveer()
        {
            ReserveerLijst Reserveringen = JsonSerializer.Deserialize<ReserveerLijst>(File.ReadAllText("reserveringen.json"));
            char userInput;
            int Tijd = 0;
            string Datum = "";
            int Personen = 0;
            string TijdString = "";
            while (true)
            {
                Console.Clear();
                Console.WriteLine("1: Pas de datum aan "+ (Datum == "" ? "<Nog geen datum>" : $"({Datum})"));
                Console.WriteLine("2: Pas de tijd aan " + (Tijd == 0 ? "<Nog geen tijd>" : $"({TijdString})"));
                Console.WriteLine("3: Pas het aantal personen aan " + (Personen == 0 ? "<Nog geen aantal personen>" : $"({Personen})"));
                Console.WriteLine("4: Bevestig de reservering\x0a" + "0: Annuleer de reservering");
                userInput = Console.ReadKey().KeyChar;
                Console.Clear();
                switch (userInput)
                {
                    case '1':
                        Console.WriteLine((Datum == "" ? "Nog geen datum" : $"({Datum})") + "\x0aWelke datum wilt u reserveren? (21 juni)");
                        Tuple<bool, string> NieuweDatum = CheckDatum(Console.ReadLine());
                        Console.Clear();
                        if (NieuweDatum.Item1)
                        {
                            Datum = NieuweDatum.Item2;
                            Console.WriteLine($"De datum is aangepast naar {Datum}\x0a\x0a");
                        }
                        else
                        {
                            Console.WriteLine("Deze datum bestaat niet\x0a\x0a");
                        }
                        Console.WriteLine("Enter: Ga terug naar het vorige scherm");
                        Console.ReadKey();
                        break;
                    case '2':
                        Console.WriteLine((Tijd == 0 ? "Nog geen tijd" : $"({TijdString})") + "\x0aWelke tijd wilt u reserveren? (11:00 - 21:00)");
                        Tuple<bool, int> NieuweTijd = CheckTijd(Console.ReadLine(),Personen, Datum, Reserveringen.Reserveringen);
                        Console.Clear();
                        if (NieuweTijd.Item1)
                        {
                            Tijd = NieuweTijd.Item2;
                            TijdString = $"{ Tijd / 100}:{Tijd % 100}";
                            TijdString += TijdString.Length < 5 ? "0" : "";
                            Console.WriteLine($"De tijd is aangepast naar {Tijd}\x0a\x0a");
                        }
                        else
                        {
                            Console.WriteLine("Deze tijd valt niet binnen de openingsuren\x0a\x0a");
                        }
                        Console.WriteLine("Enter: Ga terug naar het vorige scherm");
                        Console.ReadKey();
                        break;
                    case '3':
                        int MaxPersonen = VrijePlaatsen(Datum, Tijd, Reserveringen.Reserveringen);
                        Console.WriteLine((Personen == 0 ? "Nog geen aantal personen" : $"({Personen})") + "\x0a" + $"Er zijn {MaxPersonen} plaatsen vrij\x0aVoor hoeveel personen wilt u reserveren?");
                        Tuple<bool, int> NieuwPersonen = CheckPersonen(Console.ReadLine(), MaxPersonen);
                        Console.Clear();
                        if (NieuwPersonen.Item1)
                        {
                            Personen = NieuwPersonen.Item2;
                            Console.WriteLine($"Het aantal personen is aangepast naar {Personen}\x0a\x0a");
                        }
                        Console.WriteLine("Enter: Ga terug naar het vorige scherm");
                        Console.ReadKey();
                        break;
                    case '4':
                        if(Tijd != 0 && Datum != "" && Personen != 0)
                        {
                            Reservering NieuweReservering = new Reservering
                            {
                                Tijd = Tijd,
                                Datum = Datum,
                                Personen = Personen
                            };
                            Reserveringen = AddReservering(NieuweReservering, Reserveringen);
                            Console.WriteLine("U heeft gereserveerd!\x0a\x0a" + "Enter: Ga terug naar het startscherm");
                            Console.ReadKey();
                            return;
                        }
                        string Message = "";
                        Message += Datum == "" ? "U heeft nog geen datum ingevuld\x0a" : "";
                        Message += Tijd == 0 ? "U heeft nog geen tijd ingevuld\x0a" : "";
                        Message += Personen == 0 ? "U heeft nog niet het aantal personen aangegeven\x0a" : "";
                        Console.WriteLine(Message + "\x0a" + "Enter: Ga terug naar het vorige scherm");
                        Console.ReadKey();
                        break;
                    case '0':
                        Console.WriteLine("De reservering is geannuleerd\x0a\x0a" + "Enter: Ga terug naar het startscherm");
                        Console.ReadKey();
                        return;
                    default:
                        Console.WriteLine("Dit is geen geldige input\x0a\x0a" + "Enter: Ga terug naar het vorige scherm");
                        Console.ReadKey();
                        break;
                }
            }
        }

        public static ReserveerLijst AddReservering(Reservering reservering, ReserveerLijst ReserveerLijst)
        {
            Reservering[] Reserveringen = ReserveerLijst.Reserveringen; 
            var JSONoptions = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            Reservering[] newReserveringen;
            if (Reserveringen != null)
            {
                newReserveringen = new Reservering[Reserveringen.Length + 1];
                for (int i = 0; i < Reserveringen.Length; i++)
                {
                    newReserveringen[i] = Reserveringen[i];
                }
                newReserveringen[Reserveringen.Length] = reservering;
            }
            else
            {
                newReserveringen = new Reservering[] {reservering};
            }

            ReserveerLijst.Reserveringen = newReserveringen;
            var jsonString = JsonSerializer.Serialize(ReserveerLijst, JSONoptions);
            File.WriteAllText("reserveringen.json", jsonString);
            ReserveerLijst = JsonSerializer.Deserialize<ReserveerLijst>(File.ReadAllText("reserveringen.json"));
            return ReserveerLijst;
        }
        public static int VrijePlaatsen(string Datum, int Tijd, Reservering[] Reserveringen)
        {
            int MaxPersonen = 100;
            if (Reserveringen != null)
            {
                for (int i = 0; i < Reserveringen.Length; i++)
                {
                    if (Reserveringen[i].Datum == Datum && Reserveringen[i].Tijd > Tijd - 200 && Reserveringen[i].Tijd < Tijd + 200)
                    {
                        MaxPersonen -= Reserveringen[i].Personen;
                    }
                }
            }
            return MaxPersonen;
        }

        public static Tuple<bool, int> CheckPersonen(string P, int Max)
        {
            string Personen = "";
            foreach (char sym in P)
            {
                if (Char.IsDigit(sym))
                {
                    Personen += sym;
                }
            }
            int PersInt = Personen != "" ? Int32.Parse(Personen) : 101;
            if (PersInt <= Max && PersInt > 0)
            {
                return Tuple.Create(true, PersInt);
            }
            Console.WriteLine("Er zijn niet genoeg plaatsen beschikbaar om deze tijd");
            return Tuple.Create(false, 0);
        }

        public static Tuple<bool, int> CheckTijd(string T, int Personen, string Datum, Reservering[] Reserveringen)
        {
            string Tijd = "";
            foreach (char sym in T)
            {
                if (Char.IsDigit(sym))
                {
                    Tijd += sym;
                }
            }
            int TijdInt = Tijd != "" ? Int32.Parse(Tijd) : 0;
            TijdInt *= TijdInt < 100 ? 100 : 1;
            if (TijdInt <= 900 && TijdInt >= 0)
            {
                TijdInt += 1200;
            }
            if (TijdInt <= 2100 && TijdInt >= 1100)
            {
                int MaxPersonen = VrijePlaatsen(Datum, TijdInt, Reserveringen);
                if (Personen <= MaxPersonen)
                {
                    return Tuple.Create(true, TijdInt);
                }
                else
                {
                    Console.WriteLine("Er zijn niet genoeg vrije plaatsen op dit tijdstip\x0a");
                }
            }
            else
            {
                Console.WriteLine("Deze tijd is ongeldig\x0a");
            }
            return Tuple.Create(false, 0);
        }
        public static Tuple<bool, string> CheckDatum(string Datum)
        {
            string Dag = "";
            string Maand = "";
            foreach (char sym in Datum)
            {
                if (Char.IsDigit(sym))
                {
                    Dag += sym;
                }
                else if (Char.IsLetter(sym))
                {
                    Maand += sym;
                }
            }
            int DagInt = Dag != "" ? Int32.Parse(Dag) : 0;
            if (CheckMaand(Maand) && DagInt > 0 && DagInt < 32)
            {
                if (CheckDag(DagInt, Maand.ToLower(), DateTime.Now.Year))
                {
                    return Tuple.Create(true, $"{Dag} {Maand}");
                }
            }
            return Tuple.Create(false, "");
        }

        public static bool CheckMaand(string Maand)
        {
            string[] Maanden = new string[] { "januari", "februari", "maart", "april", "mei", "juni", "juli", "augustus", "september", "oktober", "november", "december" };
            for (int i = 0; i < Maanden.Length; i++)
            {
                if (Maand.ToLower() == Maanden[i])
                {
                    return true;
                }
            }
            return false;
        }
        public static bool CheckDag(int Dag, string Maand, int Jaar)
        {
            if (Maand == "januari" || Maand == "maart" || Maand == "mei" || Maand == "juli" || Maand == "augustus" || Maand == "oktober" || Maand == "november")
            {
                return true;
            }
            if (Maand == "februari")
            {
                if (Jaar % 4 == 0 && (Jaar % 100 != 0 || Jaar % 400 == 0))
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
            for (int i = 0, j = 11; i < 11; i++, j++)
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

        /*public static Reservering Aanpassen(Reservering reservering)
        {
            char userInput = '0';
            while (userInput != '3')
            {
                char secondInput = '0';
                Console.Clear();
                Console.WriteLine("1: Bekijk de reserveringsdetails\x0a" + "2: Bekijk de gekozen gerechten\x0a" + "3: Ga terug naar het vorige scherm");
                userInput = Console.ReadKey().KeyChar;
                switch (userInput)
                {
                    case '1':
                        while(secondInput != '4')
                        {
                            Console.Clear();
                            Console.WriteLine($"Datum: {reservering.Datum}\x0aTijd: {reservering.Tijd}\x0a" + $"Aantal personen: {reservering.Personen}\x0a\x0a" + "1: Pas de datum aan\x0a" + "2: Pas de tijd aan\x0a" + "3: Pas het aantal personen aan\x0a" + "4: Ga terug naar het vorige scherm");
                            secondInput = Console.ReadKey().KeyChar;
                            Console.Clear();
                            switch (secondInput)
                            {
                                case '1':
                                    Console.WriteLine($"Welke datum wilt u reserveren? ({reservering.Datum})");
                                    Tuple<bool, string> Datum = CheckDatum(Console.ReadLine());
                                    //Check ook of er genoeg plaatsen zijn op deze datum en tijd
                                    Console.Clear();
                                    if (Datum.Item1)
                                    {
                                        reservering.Datum = Datum.Item2;
                                        Console.WriteLine($"De datum is verzet naar:\x0a{Datum.Item2}\x0a\x0a" +"Enter: Ga terug naar het voirge scherm");
                                    }
                                    else
                                    {
                                        Console.WriteLine($"{Datum.Item2} is geen beschikbare datum\x0a\x0a" + "Enter: Ga terug naar het vorige scherm");
                                    }
                                    Console.ReadKey();
                                    break;
                                case '2':
                                    Console.WriteLine("Hoe laat wilt u reserveren? (11:00 - 21:00)");
                                    Tuple<bool, int> Tijd = CheckTijd(Console.ReadLine());
                                    //Check ook of er genoeg plaatsen zijn op deze datum en tijd
                                    Console.Clear();
                                    if (Tijd.Item1)
                                    {
                                        reservering.Tijd = Tijd.Item2;
                                        string TijdString = $"{Tijd.Item2 / 100 }:{Tijd.Item2 % 100}";
                                        TijdString += TijdString.Length < 5 ? "0" : ""; 
                                        Console.WriteLine($"De tijd is verzet naar:\x0a{TijdString}\x0a\x0a" + "Enter: Ga terug naar het vorige scherm");
                                    }
                                    else
                                    {
                                        Console.WriteLine($"De gekozen tijd valt niet binnen de openingsuren\x0a\x0a" + "Enter: Ga terug naar het vorige scherm");
                                    }
                                    Console.ReadKey();
                                    break;
                                case '3':
                                    int MaxPersonen = VrijePlaatsen(reservering.Datum,reservering.Tijd, JsonSerializer.Deserialize<ReserveerLijst>(File.ReadAllText("reserveringen.json")).Reserveringen);
                                    Console.WriteLine($"Er zijn {MaxPersonen} plaatsen vrij\x0aMet hoeveel personen bent u?");
                                    Tuple<bool, int> Personen = CheckPersonen(Console.ReadLine(), MaxPersonen);
                                    Console.Clear();
                                    if (Personen.Item1)
                                    {
                                        reservering.Personen = Personen.Item2;
                                        Console.WriteLine($"Het aantal personen is aangepast naar {Personen.Item2}\x0a\x0a" + "1: Bekijk de gekozen gerechten\x0a"+"Enter: Ga terug naar het vorige scherm");
                                        if(Console.ReadKey().KeyChar == '1')
                                        {
                                            Console.WriteLine("<Gerechten laten zien en optie om aan te passen>\x0a\x0a" + "Enter: Ga terug naar het vorige scherm");
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Er zijn niet genoeg plaatsen vrij\x0a\x0a" + "Enter: Ga terug naar het vorig scherm");
                                    }
                                    Console.ReadKey();
                                    break;
                            }
                        }
                        break;
                    case '2':
                        Console.WriteLine("<Gerechten laten zien en optie om aan te passen>");
                        Console.ReadKey();
                        break;
                }
            }
            return reservering;
        }*/


        /*Console.Clear();
        Console.WriteLine("Welke datum wilt u reserveren? (20 juli)");
        Tuple<bool,string> Datum = CheckDatum(Console.ReadLine());
        if (Datum.Item1)
        {
            Console.Clear();
            Console.WriteLine(Datum.Item2 + "\x0aHoe laat wilt u reserveren? (11:00 - 21:00)");
            Tuple<bool,int> Tijd = CheckTijd(Console.ReadLine());
            //Check of er meer dan 0 plaatsen vrij zijn
            if (Tijd.Item1)
            {
                int MaxPersonen = VrijePlaatsen(Datum.Item2,Tijd.Item2,Reserveringen.Reserveringen);
                Console.Clear();
                if (MaxPersonen > 0)
                {
                    string TijdString = $"{Tijd.Item2 / 100 }:{Tijd.Item2 % 100}";
                    TijdString += TijdString.Length < 5 ? "0" : "";
                    Console.WriteLine(TijdString + "\x0a" + $"Er zijn {MaxPersonen} plaatsen vrij\x0aMet hoeveel personen bent u?");
                    Tuple<bool, int> Personen = CheckPersonen(Console.ReadLine(), MaxPersonen);
                    if (Personen.Item1)
                    {
                        Reservering NieuweReservering = new Reservering
                        {
                            Tijd = Tijd.Item2,
                            Datum = Datum.Item2,
                            Personen = Personen.Item2
                        };
                        userInput = '1';
                        while (userInput != '2' && userInput != '3')
                        {
                            Console.Clear();
                            Console.WriteLine("1: Bekijk de reservering of pas deze aan\x0a" + "2: Bevestig de reservering\x0a" + "3: Annuleer de reservering");
                            userInput = Console.ReadKey().KeyChar;
                            Console.Clear();
                            switch (userInput)
                            {
                                case '1':
                                    NieuweReservering = Aanpassen(NieuweReservering);
                                    break;
                                case '2':
                                    Reserveringen = AddReservering(NieuweReservering, Reserveringen);
                                    Console.WriteLine("U heeft gereserveerd");
                                    break;
                                case '3':
                                    Console.WriteLine("De reservering is geannuleerd");
                                    break;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Er zijn niet genoeg plaatsen beschikbaar om deze tijd");
                    }
                }
                else
                {
                    Console.WriteLine("Er zijn geen plaatsen beschikbaar om deze tijd");
                }
            }
            else
            {
                Console.WriteLine("Deze tijd valt niet binnen de openingsuren");
            }
        }
        else
        {
            Console.WriteLine("Deze datum bestaat niet\x0a\x0a" + "1: Probeer het opnieuw\x0a" + "0: Ga terug naar het startscherm");
        }
        Console.WriteLine("\x0a" + "Enter: Ga terug naar het startscherm");
        Console.ReadKey();*/
    }
}
