using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using static TheFatDuckRestaurant.Gebruikers;
using static TheFatDuckRestaurant.Menu;

namespace TheFatDuckRestaurant
{
    public class ReserveerLijst
    {
        public ReserveerLijst() { }
        public Reservering[] Reserveringen { get; set; }

        public ReserveerLijst BekijkReserveringenMedewerker(TafelArray tafels)
        {
            if (Reserveringen == null)
                Reserveringen = new Reservering[0];
            if (Reserveringen.Length == 0)
            {
                Console.Clear();
                Console.WriteLine("Er zijn nog geen reserveringen gemaakt\x0a\x0a" + "Enter: Ga terug naar het startscherm");
                Console.ReadKey();
                return this;
            }
            while (true)
            {
                Console.Clear();
                Console.WriteLine(ASCIIART.ReserveringenArt());
                Console.WriteLine("Voor welke datum wilt u de reserveringen bekijken? (21 juni)");
                string datum = Console.ReadLine();
                Console.Clear();
                string datumLower = "";
                foreach (char sym in datum)
                {
                    if (Char.IsLetter(sym))
                        Char.ToLower(sym);
                    datumLower += sym;
                }
                int Aantal = 0;
                foreach (Reservering reservering in Reserveringen)
                {
                    if (reservering.Datum == datumLower)
                        Aantal++;
                }

                if (Aantal > 0)
                {
                    int huidigePaginaNR = 0;
                    Reservering[] RelevanteReserveringen = new Reservering[Aantal];
                    int j = 0;
                    foreach (Reservering reservering in Reserveringen)
                    {
                        if (reservering.Datum == datumLower)
                            RelevanteReserveringen[j++] = reservering;
                    }
                    while (true)
                    {
                        int hoeveelheidPaginas = (int)Math.Ceiling(RelevanteReserveringen.Length / 7.0);
                        Console.Clear();
                        Console.WriteLine(ASCIIART.ReserveringenArt());
                        Console.WriteLine($"{datumLower}\nPagina {huidigePaginaNR + 1}/{hoeveelheidPaginas}\n");
                        for (int i = 0; i < 7 && i + huidigePaginaNR * 7 < RelevanteReserveringen.Length; i++)
                        {
                            Console.WriteLine($"{i + 1}: {RelevanteReserveringen[i + huidigePaginaNR * 7].TijdString()} {RelevanteReserveringen[i + huidigePaginaNR * 7].Bezoeker} ({RelevanteReserveringen[i + huidigePaginaNR * 7].Personen} personen)");
                        }
                        Console.WriteLine();
                        if (huidigePaginaNR + 1 < hoeveelheidPaginas)
                            Console.WriteLine("8: Volgende pagina");
                        if (huidigePaginaNR + 1 >= hoeveelheidPaginas && (hoeveelheidPaginas > 1))
                            Console.WriteLine("9: Vorige pagina");
                        Console.WriteLine("0: Ga terug naar het startscherm");
                        int Index = Int32.Parse(Console.ReadKey().KeyChar.ToString());
                        Console.Clear();
                        if (Index == 0)
                            return this;
                        if (Index > 0 && Index < 8)
                        {
                            RelevanteReserveringen[Index - 1].Info();
                            Console.WriteLine("\nA: Tafels koppelen aan reservering");
                            char userInput = Console.ReadKey().KeyChar;
                            if (userInput == 'A')
                                RelevanteReserveringen[Index - 1].AddTafels(tafels);
                        }
                        else if (Index == 8 && huidigePaginaNR + 1 < hoeveelheidPaginas)
                            huidigePaginaNR++;
                        else if (Index == 9 && huidigePaginaNR + 1 >= hoeveelheidPaginas && (hoeveelheidPaginas > 1))
                            huidigePaginaNR--;
                        else
                        {
                            Console.WriteLine("Dit is geen geldige input");
                            Console.WriteLine("\x0a" + "Enter: Ga terug naar het vorige scherm");
                            Console.ReadKey();
                        }
                    }
                }
                Console.WriteLine("Er zijn nog geen reserveringen gedaan voor deze datum\x0a");
                Console.WriteLine("Enter: Ga terug naar het startscherm");
                Console.ReadKey();
            }
        }
        public void BekijkReserveringenKlant(string klantNaam)
        {
            if (Reserveringen == null)
                Reserveringen = new Reservering[0];

            if (Reserveringen.Length == 0)
            {
                Console.Clear();
                Console.WriteLine("U heeft nog geen reserveringen gemaakt\x0a\x0a" + "Enter: Ga terug naar het startscherm");
                Console.ReadKey();
                return;
            }
            int huidigePaginaNR = 0;
            while (true)
            {
                int Aantal = 0;
                foreach (Reservering reservering in Reserveringen)
                {
                    if (reservering.Bezoeker == klantNaam)
                        Aantal++;
                }
                if (Aantal == 0)
                {
                    Console.Clear();
                    Console.WriteLine("U heeft nog geen reserveringen gemaakt\x0a\x0a" + "Enter: Ga terug naar het startscherm");
                    Console.ReadKey();
                    return;
                }
                TheFatDuckRestaurant.Reservering[] KlantReserveringen = new TheFatDuckRestaurant.Reservering[Aantal];
                int j = 0;
                for (int i = 0; i < Reserveringen.Length; i++)
                {
                    if (Reserveringen[i].Bezoeker == klantNaam)
                        KlantReserveringen[j++] = Reserveringen[i];
                }
                int hoeveelheidPaginas = (int)Math.Ceiling(KlantReserveringen.Length / 7.0);
                Console.Clear();
                Console.WriteLine($"Pagina {huidigePaginaNR + 1}/{hoeveelheidPaginas}\n");
                for (int i = 0; i < 7 && i + huidigePaginaNR * 7 < KlantReserveringen.Length; i++)
                {
                    Console.WriteLine($"{i + 1}: {KlantReserveringen[i + huidigePaginaNR * 7].Datum} om {KlantReserveringen[i + huidigePaginaNR * 7].TijdString()} ({KlantReserveringen[i + huidigePaginaNR * 7].Personen} personen)");
                }
                Console.WriteLine();
                if (huidigePaginaNR + 1 < hoeveelheidPaginas)
                    Console.WriteLine("8: Volgende pagina");
                if (huidigePaginaNR + 1 >= hoeveelheidPaginas && (hoeveelheidPaginas > 1))
                    Console.WriteLine("9: Vorige pagina");
                Console.WriteLine("0: Ga terug naar het startscherm");
                int Index = Int32.Parse(Console.ReadKey().KeyChar.ToString());
                Console.Clear();
                if (Index == 0)
                    return;
                if (Index < 7 && Index > 0)
                    changeReservering(KlantReserveringen[Index - 1]); //TODO: Opties om reserveringen aan te passen die zijn gemaakt.
                else if (Index == 8 && huidigePaginaNR + 1 < hoeveelheidPaginas)
                    huidigePaginaNR++;
                else if (Index == 9 && huidigePaginaNR + 1 >= hoeveelheidPaginas && (hoeveelheidPaginas > 1))
                    huidigePaginaNR--;
                else
                {
                    Console.WriteLine("Dit is geen geldige input\x0a\x0a" + "Enter: Ga terug naar het vorige scherm");
                    Console.ReadKey();
                }
            }
        }



        public void changeReservering(Reservering reservering)
        {
            //removeReservering(reservering);
            //GERECHTEN
            //createReservering(reservering.Bezoeker, reservering.Tijd, reservering.Datum, reservering.Personen, null, "Verwijder");
        }
        private void removeReservering(Reservering reservering)
        {
            Reservering[] newReserveringen = new Reservering[this.Reserveringen.Length - 1];
            for (int i = 0, j = 0; i < this.Reserveringen.Length; i++)
            {
                if (this.Reserveringen[i] != reservering)
                    newReserveringen[j++] = this.Reserveringen[i];
            }
            this.Reserveringen = newReserveringen;
        }
        public bool createReservering(string klant, Menu menu, int tijd = 0, string datum = "", int personen = 0, List<Bestelling> bestelling = null, string changeItem = "Annuleer")
        {
            //GERECHTEN
            Reservering NieuweReservering = new Reservering(tijd, datum, personen, klant, bestelling);
            while (true)
            {
                switch (NieuweReservering.Create(changeItem))
                {
                    case '1':
                        string NieuweDatum = NieuweReservering.changeDatum();
                        if (NieuweDatum != null)
                        {
                            if (NieuweReservering.Personen <= VrijePlaatsen(NieuweReservering.Tijd, NieuweDatum))
                                NieuweReservering.Datum = NieuweDatum;
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
                                NieuweReservering.Tijd = NieuweTijd;
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
                        NieuweReservering.changeGerechten(menu);
                        break;
                    case '5':
                        if (AddReservering(NieuweReservering))
                            return true;
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
            if (reservering.Tijd != 0 && reservering.Datum != "" && reservering.Personen != 0 && (reservering.Bestelling.Count != 0 && reservering.Bestelling != null))
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
                    newReserveringen = new Reservering[] { reservering };
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
            Message += (reservering.Bestelling == null) ? "U heeft nog geen gerechten gekozen\x0a" : "";
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
                    if (Reserveringen[i].Personen % 2 == 1)
                        MaxPersonen--;
                    MaxPersonen -= Reserveringen[i].Personen;
                }
            }
            return MaxPersonen;
        }
    }

    public class Reservering //Nieuwe file
    {
        public int Tijd { get; set; }
        public string Datum { get; set; }
        public int Personen { get; set; }
        public string Bezoeker { get; set; }
        public List<Bestelling> Bestelling { get; set; }

        public List<Tafel> Tafels { get; set; }
        public Reservering() { }
        public Reservering(int tijd, string datum, int personen, string bezoeker, List<Bestelling> bestelling) //Tafels worden niet doorgegeven bij deze constructor omdat een klant bij het reserveren zelf geen keuze zal hebben. Tafels worden later gekoppeld door medewerkers.
        {
            this.Tijd = tijd;
            this.Datum = datum;
            this.Personen = personen;
            this.Bezoeker = bezoeker;
            this.Bestelling = bestelling;
            this.Tafels = null;
        }
        public void changeGerechten(Menu menu)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine(TheFatDuckRestaurant.ASCIIART.ReserverenArt());
                Console.WriteLine("1: Bekijk het menu\n2: Bekijk uw gekozen gerechten\n\n0: Ga terug naar het vorige scherm");
                char Input = Console.ReadKey().KeyChar;
                switch (Input)
                {
                    case '1':
                        addGerechten(menu);
                        break;

                    case '2':
                        if (this.Bestelling == null || this.Bestelling.Count == 0)
                        {
                            Console.Clear();
                            Console.WriteLine(ASCIIART.ReserverenArt());
                            Console.WriteLine("U heeft nog geen gerechten gekozen\nBekijk het menu om gerechten toe te voegen");
                            Console.WriteLine("\n\n0: Ga terug naar het vorige scherm");
                            Console.ReadKey();
                        }
                        else
                        {
                            int huidigePaginaNR = 0;
                            while (true)
                            {
                                int hoeveelheidPaginas = (int)Math.Ceiling(this.Bestelling.Count / 7.0);
                                Console.Clear();
                                Console.WriteLine(ASCIIART.ReserverenArt());
                                Console.WriteLine("Dit zijn de huidige gerechten die u heeft besteld\nToets op het getal naast het gerecht om het te verwijderen uit uw reservering\n");
                                Console.WriteLine($"Pagina {huidigePaginaNR + 1}/{hoeveelheidPaginas}\n");
                                for (int i = 0; i < 7 && i + huidigePaginaNR * 7 < this.Bestelling.Count; i++)
                                {
                                    Console.WriteLine($"{i + 1}: {this.Bestelling[i + huidigePaginaNR * 7].Naam}: {this.Bestelling[i + huidigePaginaNR * 7].Aantal}x, {this.Bestelling[i + huidigePaginaNR * 7].TotaalPrijs} euro");
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
                                    return;
                                if (Index < 8 && Index > 0)
                                {
                                    string tempName = this.Bestelling[Index - 1 + huidigePaginaNR * 7].Naam;
                                    Console.Clear();
                                    Console.WriteLine(ASCIIART.ReserverenArt());
                                    Console.WriteLine($"Weet u zeker dat u {tempName} wilt verwijderen?\n\n1: Verwijder het gerecht uit uw reservering\n0: Ga terug naar het vorige scherm");
                                    char userInput = Console.ReadKey().KeyChar;
                                    Console.Clear();
                                    switch (userInput)
                                    {
                                        case '1':
                                            this.Bestelling.RemoveAt(Index - 1 + huidigePaginaNR * 7);
                                            Console.WriteLine(ASCIIART.ReserverenArt());
                                            Console.WriteLine($"{tempName} is verwijderd uit uw reservering!\n\n0: Ga terug naar het vorige scherm");
                                            Console.ReadKey();
                                            break;
                                    }
                                }

                                else if (Index == 8 && huidigePaginaNR + 1 < hoeveelheidPaginas)
                                    huidigePaginaNR++;
                                else if (Index == 9 && huidigePaginaNR + 1 >= hoeveelheidPaginas && (hoeveelheidPaginas > 1))
                                    huidigePaginaNR--;
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
                }
            }
        }
        private void addGerechten(Menu menu)
        {
            this.Bestelling = menu.BekijkMenuKlant(this.Bestelling);
        }

        public void AddTafels(TafelArray tafels)
        {
            bool wrongInput = false;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Koppel Tafels aan de reservering\n");
                Console.WriteLine("1: Bekijk alle vrije tafels\n");
                Console.WriteLine("2: Koppel tafels via ID\n");
                Console.WriteLine("3: Automatisch koppelen\n");
                Console.WriteLine("0: Terug\n");
                if (wrongInput)
                    Console.WriteLine("Wrong input! Probeer 1, 2, 3 of 0");
                char userInput = Console.ReadKey().KeyChar;
                switch (userInput)
                {
                    case '1':
                        tafels.BekijkVrijeTafels();
                        char specifiekeInput = Console.ReadKey().KeyChar;
                        break;
                    case '2':
                        this.Tafels = tafels.KoppelenDoorMedewerker(this.Personen, this.Tafels); //Personen -> aantal personen, Tafels -> gereserveerde tafels die al gekoppeld zijn.
                        break;
                    case '3':
                        this.Tafels = tafels.AutomatischKoppelen(this.Personen, this.Tafels); //Personen -> aantal personen, Tafels -> gereserveerde tafels die al gekoppeld zijn.
                        break;
                    case '0':
                        return;
                    default:
                        wrongInput = true;
                        break;
                }
                return;
            }
        }
        
       /* private void removeGerecht(Gerechten gerecht)
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
        } */
        
                                public void Info()
                                {
                                    Console.WriteLine(ASCIIART.ReserverenArt());
                                    Console.WriteLine("Klant:\t\t" + this.Bezoeker);
                                    Console.WriteLine("Tijd:\t\t" + TijdString());
                                    Console.WriteLine("Datum:\t\t" + this.Datum);
                                    Console.WriteLine("Personen:\t" + this.Personen);
                                    Console.WriteLine("Gerechten:");
                                    foreach (var bestellingItem in this.Bestelling)
                                        Console.WriteLine($"- {bestellingItem.Naam}: ({bestellingItem.Aantal}x), {bestellingItem.TotaalPrijs} euro");
                                    Console.WriteLine("Tafels:");
                                    if (this.Tafels == null)
                                        Console.WriteLine("Nog geen tafels gekoppeld aan deze reservering.");
                                    else
                                    {
                                    foreach (Tafel tafel in this.Tafels)
                                        Console.WriteLine($"- {tafel.ID}: {tafel.Plekken} plekken");
                                    }

                                }
                                public char Create(string addition)
                                {
                                    Console.Clear();
                                    Console.WriteLine(TheFatDuckRestaurant.ASCIIART.ReserverenArt());
                                    Console.WriteLine("1: Datum\t\t" + (this.Datum == "" ? "U heeft nog geen datum gekozen" : $"({this.Datum})"));
                                    Console.WriteLine("2: Tijd\t\t\t" + (this.Tijd == 0 ? "U heeft nog geen tijd gekozen" : $"({TijdString()})"));
                                    Console.WriteLine("3: Aantal personen\t" + (this.Personen == 0 ? "U heeft nog niet het aantal personen aangegeven" : $"({this.Personen} personen)"));
                                    Console.WriteLine("4: Gerechten\t\t" + ((this.Bestelling == null || this.Bestelling.Count == 0) ? "U heeft nog geen gerechten gekozen" : $"{this.Bestelling.Count} verschillende gerechten"));
                                    Console.WriteLine($"\n5: Bevestig de reservering\n0: {addition} de reservering");
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
                                            PersonenString += sym;
                                    }
                                    int PersonenInt = PersonenString != "" ? Int32.Parse(PersonenString) : 101;
                                    if (PersonenInt <= MaxPersonen && PersonenInt > 0)
                                        this.Personen = PersonenInt;
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
                                    foreach (char sym in Input)
                                    {
                                        if (Char.IsDigit(sym))
                                            Tijdstring += sym;
                                    }
                                    int TijdInt = Tijdstring != "" ? Int32.Parse(Tijdstring) : 0;
                                    TijdInt *= TijdInt < 100 ? 100 : 1;
                                    if (TijdInt <= 900 && TijdInt >= 0)
                                        TijdInt += 1200;
                                    if (TijdInt <= 2100 && TijdInt >= 1100 && TijdInt % 100 < 60)
                                        return TijdInt;
                                    Console.WriteLine(TheFatDuckRestaurant.ASCIIART.ReserverenArt());
                                    Console.WriteLine("Deze tijd is ongeldig\x0a\x0a");
                                    Console.WriteLine("Enter: Ga terug naar het vorige scherm");
                                    Console.ReadKey();
                                    return 0;
                                }
        public string checkDatum(string Datum, bool reserveren = true)
        {
            string Dag = "";
            string Maand = "";
            foreach (char sym in Datum)
            {
                if (Char.IsDigit(sym))
                {
                    Dag += sym;
                }
                else if (Char.IsLetter(sym) && Dag != "")
                {
                    Maand += sym;
                }
            }
            int DagInt = Dag != "" ? Int32.Parse(Dag) : 0;
            int Jaar = DateTime.Now.Year;
            if (!reserveren)
            {
                bool Passed = false;
                while (!Passed)
                {
                    Console.WriteLine("Voor welk jaar wilt u de opbrengsten bekijken?");
                    if (int.TryParse(Console.ReadLine(), out Jaar))
                    {
                        Passed = true;
                    }
                    else
                    {
                        Console.WriteLine("U heeft geen getal ingevoerd\n\nEnter: Ga terug naar het vorige scherm");
                        Console.ReadKey();
                    }
                }
                Console.Clear();
            }
            if (CheckMaand(Maand.ToLower()) && DagInt > 0 && DagInt < 32)
            {
                if (CheckDag(DagInt, Maand.ToLower(), Jaar))
                {
                    return $"{WeekDag(DagInt, Maand, Jaar, reserveren)} {Dag} {Maand} {Jaar}";
                }
            }
            Console.WriteLine(TheFatDuckRestaurant.ASCIIART.ReserverenArt());
            Console.WriteLine("Deze datum bestaat niet\x0a\x0a");
            Console.WriteLine("Enter: Ga terug naar het vorige scherm");
            Console.ReadKey();
            return null;
        }
        public string changeDatum()
        {
            Console.WriteLine(TheFatDuckRestaurant.ASCIIART.ReserverenArt());
            Console.WriteLine((this.Datum == "" ? "Nog geen datum" : $"({this.Datum})") + "\x0aWelke datum wilt u reserveren? (21 juni)");
            string NieuweDatum = Console.ReadLine();
            Console.Clear();
            return checkDatum(NieuweDatum);
        }
        private string WeekDag(int Dag, string maand, int Jaar, bool reserveren)
        {
            int Maand = MaandInt(Dag, maand);
            int HuidigeDag = DateTime.Now.Day;
            int HuidigeMaand = DateTime.Now.Month;
            DateTime date;
            if ((Maand < HuidigeMaand || (Maand == HuidigeMaand && Dag < HuidigeDag)) && reserveren)
            {
                date = new DateTime(Jaar + 1, Maand, Dag);
            }
            else
            {
                date = new DateTime(Jaar, Maand, Dag);
            }
            string WeekDay = "" + date.DayOfWeek;
            return DaytoDag(WeekDay.ToLower());
        }
        private string DaytoDag(string Day)
                                {
                                    return Day == "monday" ? "Maandag" :
                                        Day == "tuesday" ? "Dinsdag" :
                                        Day == "wednesday" ? "Woensdag" :
                                        Day == "thursday" ? "Donderdag" :
                                        Day == "friday" ? "Vrijdag" :
                                        Day == "saturday" ? "Zaterdag" : "Zondag";
                                }
                                private int MaandInt(int dag, string maand)
                                {
                                    string[] Maanden = new string[] { "januari", "februari", "maart", "april", "mei", "juni", "juli", "augustus", "september", "oktober", "november", "december" };
                                    for (int i = 0; i < Maanden.Length; i++)
                                    {
                                        if (maand == Maanden[i])
                                            return i + 1;
                                    }
                                    return 0;
                                }
                                private bool CheckMaand(string maand)
                                {
                                    string[] Maanden = new string[] { "januari", "februari", "maart", "april", "mei", "juni", "juli", "augustus", "september", "oktober", "november", "december" };
                                    for (int i = 0; i < Maanden.Length; i++)
                                    {
                                        if (maand == Maanden[i])
                                            return true;
                                    }
                                    return false;
                                }
                                private bool CheckDag(int Dag, string Maand, int Jaar)
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
                                public string TijdString()
                                {
                                    string tstring = this.Tijd / 100 + ":" + this.Tijd % 100;
                                    return tstring.Length < 5 ? tstring + "0" : tstring;
                                }
                                private string showTafels()
                                {
                                    if (this.Tafels == null)
                                        return "Nog geen tafels";

                string returnString = "";
                foreach(Tafel tafel in Tafels)
                {
                    returnString += tafel.ID + "\n";
                }
                return returnString;
            }
        }

    public class Bestelling
    {
        public string Naam { get; set; }
        public double Prijs { get; set; }
        public int Aantal { get; set; }

        public Bestelling() { }
        public Bestelling(string naam, double prijs, int aantal)
        {
            this.Naam = naam;
            this.Prijs = prijs;
            this.Aantal = aantal;
        }
        
        [JsonIgnore]
        public double TotaalPrijs
        {
            get => this.Prijs * this.Aantal;
        }
    }
}