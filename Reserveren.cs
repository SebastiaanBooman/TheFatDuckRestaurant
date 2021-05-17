using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using static TheFatDuckRestaurant.Gebruikers;

namespace TheFatDuckRestaurant
{
    public class Reserveren
    {
        public class ReserveerLijst
        {
            public ReserveerLijst() { }
            public Reservering[] Reserveringen { get; set; }
            public bool changeReservering(Reservering reservering)
            {
                removeReservering(reservering);
                //GERECHTEN
                return createReservering(reservering.Bezoeker, reservering.Tijd, reservering.Datum, reservering.Personen, null, "Verwijder");
            }
            private void removeReservering(Reservering reservering)
            {
                Reservering[] newReserveringen = new Reservering[this.Reserveringen.Length - 1];
                for (int i = 0, j = 0; i < this.Reserveringen.Length; i++)
                {
                    if (this.Reserveringen[i] != reservering)
                    {
                        newReserveringen[j++] = this.Reserveringen[i];
                    }
                }
                this.Reserveringen = newReserveringen;
            }
            public bool createReservering(Klant klant, int tijd = 0, string datum = "", int personen = 0,Gerechten[] gerechten = null, string changeItem = "Annuleer")
            {
                //GERECHTEN
                Reservering NieuweReservering = new Reservering(tijd,datum,personen,klant,gerechten);
                while (true)
                {
                    switch (NieuweReservering.Create(changeItem))
                    {
                        case '1':
                            string NieuweDatum = NieuweReservering.changeDatum();
                            if(NieuweDatum != null)
                            {
                                if(NieuweReservering.Personen <= VrijePlaatsen(NieuweReservering.Tijd, NieuweDatum))
                                {
                                    NieuweReservering.Datum = NieuweDatum;
                                }
                                else
                                {
                                    Console.WriteLine(TheFatDuckRestaurant.ASCIIART.ReserverenArt());
                                    Console.WriteLine("Er zijn niet genoeg vrije plaatsen op deze dag op dit tijdstip\x0a\x0a" + "Enter: Ga terug naar het vorige scherm");
                                    Console.ReadKey();
                                }
                            }
                            break;
                        case '2':
                            int NieuweTijd = NieuweReservering.changeTijd();
                            if (NieuweTijd != 0)
                            {
                                if (NieuweReservering.Personen <= VrijePlaatsen(NieuweTijd, NieuweReservering.Datum))
                                {
                                    NieuweReservering.Tijd = NieuweTijd;
                                }
                                else
                                {
                                    Console.WriteLine(TheFatDuckRestaurant.ASCIIART.ReserverenArt());
                                    Console.WriteLine("Er zijn niet genoeg vrije plaatsen op deze dag op dit tijdstip\x0a\x0a" + "Enter: Ga terug naar het vorige scherm");
                                    Console.ReadKey();
                                }
                            }
                            break;
                        case '3':
                            NieuweReservering.changePersonen(VrijePlaatsen(NieuweReservering.Tijd, NieuweReservering.Datum));
                            break;
                        case '4':
                            Console.WriteLine(TheFatDuckRestaurant.ASCIIART.ReserverenArt());
                            Console.WriteLine("<Gerechten>");
                            Console.ReadKey();
                            break;
                        case '5':
                            if (AddReservering(NieuweReservering))
                            {
                                klant.AantalReserveringen += 1;
                                return true;
                            }
                            break;
                        case '0':
                            Console.WriteLine(TheFatDuckRestaurant.ASCIIART.ReserverenArt());
                            Console.WriteLine(changeItem == "Verwijder" ? "De reservering is verwijderd\x0a" : "De reservering is geannuleerd\x0a");
                            Console.WriteLine("Enter: Ga terug naar het startscherm");
                            Console.ReadKey();
                            return false;
                        default:
                            Console.WriteLine(TheFatDuckRestaurant.ASCIIART.ReserverenArt());
                            Console.WriteLine("Dit is geen geldige input\x0a\x0a" + "Enter: Ga terug naar het vorige scherm");
                            Console.ReadKey();
                            break;
                    }
                }
            }
            private bool AddReservering(Reservering reservering)
            {
                if (reservering.Tijd != 0 && reservering.Datum != "" && reservering.Personen != 0)
                {
                    Reservering[] newReserveringen;
                    if (this.Reserveringen != null)
                    {
                        newReserveringen = new Reservering[this.Reserveringen.Length + 1];
                        for (int i = 0; i < Reserveringen.Length; i++)
                        {
                            newReserveringen[i] = Reserveringen[i];
                        }
                        newReserveringen[Reserveringen.Length] = reservering;
                    }
                    else
                    {
                        newReserveringen = new Reservering[] { reservering };
                    }
                    this.Reserveringen = newReserveringen;
                    Console.WriteLine(TheFatDuckRestaurant.ASCIIART.ReserverenArt());
                    Console.WriteLine("U heeft gereserveerd!\x0a\x0a" + "Enter: Ga terug naar het startscherm");
                    Console.ReadKey();
                    return true;
                }
                string Message = "";
                Message += reservering.Datum == "" ? "U heeft nog geen datum ingevuld\x0a" : "";
                Message += reservering.Tijd == 0 ? "U heeft nog geen tijd ingevuld\x0a" : "";
                Message += reservering.Personen == 0 ? "U heeft nog niet het aantal personen aangegeven\x0a" : "";
                Console.WriteLine(TheFatDuckRestaurant.ASCIIART.ReserverenArt());
                Console.WriteLine(Message + "\x0a" + "Enter: Ga terug naar het vorige scherm");
                Console.ReadKey();
                return false;
            }
            private int VrijePlaatsen(int tijd, string datum)
            {
                int MaxPersonen = 100;
                for (int i = 0; i < this.Reserveringen.Length; i++)
                {
                    if (this.Reserveringen[i].Datum == datum && this.Reserveringen[i].Tijd > tijd - 200 && this.Reserveringen[i].Tijd < tijd + 200)
                    {
                        MaxPersonen -= Reserveringen[i].Personen;
                    }
                }
                return MaxPersonen;
            }
        }
        public static void updateReserveerlijst(ReserveerLijst reserveerlijst)
        {
            var JSONoptions = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            File.WriteAllText("reserveringen.json", JsonSerializer.Serialize(reserveerlijst, JSONoptions));
        }
        public static void updateGebruikers(Gebruikers gebruikers)
        {
            var JSONoptions = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            File.WriteAllText("gebruikers.json", JsonSerializer.Serialize(gebruikers, JSONoptions));
        }
        public class Reservering //Nieuwe file
        {
            public int Tijd { get; set; }
            public string Datum { get; set; }
            public int Personen { get; set; }
            public Klant Bezoeker { get; set; }
            public Gerechten[] Gerechten { get; set; }
            public Reservering() { }
            public Reservering(int tijd, string datum, int personen, Klant bezoeker, Gerechten[] gerechten)
            {
                this.Tijd = tijd;
                this.Datum = datum;
                this.Personen = personen;
                this.Bezoeker = bezoeker;
                this.Gerechten = gerechten;
            }
            public void changeGerechten(Menu menu)
            {
                if (this.Gerechten == null)
                {
                    this.Gerechten = new Gerechten[0];
                }
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine(TheFatDuckRestaurant.ASCIIART.ReserverenArt());
                    Console.WriteLine("1: Bekijk het menu\n2: Bekijk uw gekozen gerechten\n\n0: Ga terug naar het vorige scherm");
                    char Input = Console.ReadKey().KeyChar;
                    Console.Clear();
                    Console.WriteLine(TheFatDuckRestaurant.ASCIIART.ReserverenArt());
                    switch (Input)
                    {
                        case '1':
                            addGerechten(menu);
                            break;
                        case '2':
                            if (this.Gerechten.Length == 0)
                            {
                                Console.WriteLine("U heeft nog geen gerechten gekozen\nBekijk het menu om gerechten toe te voegen");
                                Console.WriteLine("\nEnter: Ga terug naar het vorige scherm");
                                Console.ReadKey();
                            }
                            else
                            {
                                int huidigePaginaNR = 0;
                                while (true)
                                {
                                    int hoeveelheidPaginas = (int)Math.Ceiling(this.Gerechten.Length / 7.0);
                                    Console.Clear();
                                    Console.WriteLine($"Pagina {huidigePaginaNR + 1}/{hoeveelheidPaginas}\n");
                                    for (int i = 0; i < 7 && i + huidigePaginaNR * 7 < this.Gerechten.Length; i++)
                                    {
                                        Console.WriteLine($"{i + 1}: {this.Gerechten[i + huidigePaginaNR * 7].naam}\t{this.Gerechten[i + huidigePaginaNR * 7].prijs} euro");
                                    }
                                    Console.WriteLine();
                                    if (huidigePaginaNR + 1 < hoeveelheidPaginas)
                                        Console.WriteLine("8: Volgende pagina");
                                    if (huidigePaginaNR + 1 >= hoeveelheidPaginas && (hoeveelheidPaginas > 1))
                                        Console.WriteLine("9: Vorige pagina");
                                    Console.WriteLine("0: Ga terug naar het vorige scherm");
                                    int Index = Int32.Parse(Console.ReadKey().KeyChar.ToString());
                                    Console.Clear();
                                    if (Index == 0)
                                    {
                                        return;
                                    }
                                    if (Index < 8 && Index > 0)
                                    {
                                        //Laat informatie (ingrediënten, prijs, etc.) zien over het gekozen gerecht (this.Gerechten[Index - 1 + huidigePaginaNR * 7])
                                        Console.WriteLine("<Bekijk het gekozen gerecht>");
                                        Console.WriteLine("\n1: Verwijder het gerecht van de bestelling\n0: Ga terug naar het vorige scherm");
                                        char userInput = Console.ReadKey().KeyChar;
                                        Console.Clear();
                                        switch(userInput)
                                        {
                                            case '1':
                                                removeGerecht(this.Gerechten[Index - 1 + huidigePaginaNR * 7]);
                                                Console.WriteLine("Het gerecht is verwijderd!\n\nEnter: Ga terug naar het vorige scherm");
                                                break;
                                        }
                                    }
                                    else if (Index == 8 && huidigePaginaNR + 1 < hoeveelheidPaginas)
                                    {
                                        huidigePaginaNR++;
                                    }
                                    else if (Index == 9 && huidigePaginaNR + 1 >= hoeveelheidPaginas && (hoeveelheidPaginas > 1))
                                    {
                                        huidigePaginaNR--;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Dit is geen geldige input\x0a\x0a" + "Enter: Ga terug naar het vorige scherm");
                                        Console.ReadKey();
                                    }
                                }
                            }
                            break;
                        case '0':
                            return;
                        default:
                            Console.WriteLine("Dit is geen geldige input");
                            Console.WriteLine("\nEnter: Ga terug naar het vorige scherm");
                            Console.ReadKey();
                            break;
                    }
                }
            }
            private void addGerechten(Menu menu)
            {
                Console.Clear();
                Console.WriteLine(TheFatDuckRestaurant.ASCIIART.ReserverenArt());
                    /*Gerechten gekozenGerecht = laat het menu een 'list of Gerechten' returnen
                    if(gekozenGerecht.Length != 0)
                    {
                        addGerecht(gekozenGerechten);
                    }
                    */
            }
            private void addGerecht(Gerechten[] gerechten)
            {
                Gerechten[] nieuwegerechten = new Gerechten[this.Gerechten.Length + gerechten.Length];
                int i = 0;
                foreach(Gerechten gerecht in this.Gerechten)
                {
                    nieuwegerechten[i++] = gerecht;
                }
                for(int j = 0; j < gerechten.Length; j++)
                {
                    nieuwegerechten[i++] = gerechten[j];
                }
                this.Gerechten = nieuwegerechten;
            }
            private void removeGerecht(Gerechten gerecht)
            {
                Gerechten[] nieuwegerechten = new Gerechten[this.Gerechten.Length - 1];
                bool Removed = false;
                int i = 0;
                foreach(Gerechten Gerecht in this.Gerechten)
                {
                    if(gerecht != Gerecht || Removed)
                    {
                        nieuwegerechten[i++] = Gerecht;
                    }
                    else
                    {
                        Removed = true;
                    }
                }
                this.Gerechten = nieuwegerechten;
            }
            public void Info()
            {
                Console.WriteLine("Klant:\t\t" + this.Bezoeker.Naam);
                Console.WriteLine("Tijd:\t\t" + TijdString());
                Console.WriteLine("Datum:\t\t" + this.Datum);
                Console.WriteLine("Personen:\t" + this.Personen);
                Console.WriteLine("Menu:\t\t" + "<gerechten>");
            }
            public char Create(string addition)
            {
                Console.Clear();
                Console.WriteLine(TheFatDuckRestaurant.ASCIIART.ReserverenArt());
                Console.WriteLine("1: Datum\t\t" + (this.Datum == "" ? "U heeft nog geen datum gekozen" : $"({this.Datum})"));
                Console.WriteLine("2: Tijd\t\t\t" + (this.Tijd == 0 ? "U heeft nog geen tijd gekozen" : $"({TijdString()})"));
                Console.WriteLine("3: Aantal personen\t" + (this.Personen == 0 ? "U heeft nog niet het aantal personen aangegeven" : $"({this.Personen})"));
                Console.WriteLine("4: Gerechten\t\t"+ (this.Gerechten == null ? "U heeft nog geen gerechten gekozen" : this.Gerechten.Length == 0 ? "U heeft nog geen gerechten gekozen" : $"{this.Gerechten.Length} gerechten"));
                Console.WriteLine($"5: Bevestig de reservering\n0: {addition} de reservering");
                char Input = Console.ReadKey().KeyChar;
                Console.Clear();
                return Input;
            }
            public void changePersonen(int MaxPersonen)
            {
                Console.WriteLine(TheFatDuckRestaurant.ASCIIART.ReserverenArt());
                Console.WriteLine((this.Personen == 0 ? "Nog geen aantal personen" : $"({this.Personen})") + "\x0a" + $"Er zijn {MaxPersonen} plaatsen vrij\x0aVoor hoeveel personen wilt u reserveren?");
                string Input = Console.ReadLine();
                string PersonenString = "";
                foreach (char sym in Input)
                {
                    if (Char.IsDigit(sym))
                    {
                        PersonenString += sym;
                    }
                }
                int PersonenInt = PersonenString != "" ? Int32.Parse(PersonenString) : 101;
                if(PersonenInt <= MaxPersonen && PersonenInt > 0)
                {
                    this.Personen = PersonenInt;
                }
                else
                {
                    Console.WriteLine(TheFatDuckRestaurant.ASCIIART.ReserverenArt());
                    Console.WriteLine("Er zijn niet genoeg plaatsen beschikbaar om deze tijd\x0a\x0a");
                    Console.WriteLine("Enter: Ga terug naar het vorige scherm");
                    Console.ReadKey();
                }
            }
            public int changeTijd()
            {
                Console.WriteLine(TheFatDuckRestaurant.ASCIIART.ReserverenArt());
                Console.WriteLine((Tijd == 0 ? "Nog geen tijd" : $"({TijdString()})") + "\x0aWelke tijd wilt u reserveren? (11:00 - 21:00)");
                string Input = Console.ReadLine();
                Console.Clear();
                string Tijdstring = "";
                foreach(char sym in Input)
                {
                    if (Char.IsDigit(sym))
                    {
                        Tijdstring += sym;
                    }
                }
                int TijdInt = Tijdstring != "" ? Int32.Parse(Tijdstring) : 0;
                TijdInt *= TijdInt < 100 ? 100 : 1;
                if (TijdInt <= 900 && TijdInt >= 0)
                {
                    TijdInt += 1200;
                }
                if (TijdInt <= 2100 && TijdInt >= 1100 && TijdInt % 100 < 60)
                {
                    return TijdInt;
                }
                Console.WriteLine(TheFatDuckRestaurant.ASCIIART.ReserverenArt());
                Console.WriteLine("Deze tijd is ongeldig\x0a\x0a");
                Console.WriteLine("Enter: Ga terug naar het vorige scherm");
                Console.ReadKey();
                return 0;
            }
            public string changeDatum()
            {
                Console.WriteLine(TheFatDuckRestaurant.ASCIIART.ReserverenArt());
                Console.WriteLine((this.Datum == "" ? "Nog geen datum" : $"({this.Datum})") + "\x0aWelke datum wilt u reserveren? (21 juni)");
                string NieuweDatum = Console.ReadLine();
                Console.Clear();
                string Dag = "";
                string Maand = "";
                foreach (char sym in NieuweDatum)
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
                if (CheckMaand(Maand.ToLower()) && DagInt > 0 && DagInt < 32)
                {
                    if (CheckDag(DagInt, Maand.ToLower(), DateTime.Now.Year))
                    {
                        return $"{Dag} {Maand}";
                    }
                }
                Console.WriteLine(TheFatDuckRestaurant.ASCIIART.ReserverenArt());
                Console.WriteLine("Deze datum bestaat niet\x0a\x0a");
                Console.WriteLine("Enter: Ga terug naar het vorige scherm");
                Console.ReadKey();
                return null;
            }
            private bool CheckMaand(string maand)
            {
                string[] Maanden = new string[] { "januari", "februari", "maart", "april", "mei", "juni", "juli", "augustus", "september", "oktober", "november", "december" };
                for (int i = 0; i < Maanden.Length; i++)
                {
                    if (maand == Maanden[i])
                    {
                        return true;
                    }
                }
                return false;
            }
            private bool CheckDag(int Dag, string Maand, int Jaar)
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
            public string TijdString()
            {
                string tstring = this.Tijd / 100 + ":" + this.Tijd % 100;
                return tstring.Length < 5 ? tstring + "0" : tstring;
            }
        }
    }
}
