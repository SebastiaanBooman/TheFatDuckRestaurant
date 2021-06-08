using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TheFatDuckRestaurant
{
    public class Reservering
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
                            Console.OutputEncoding = System.Text.Encoding.UTF8;
                            while (true)
                            {
                                int hoeveelheidPaginas = (int)Math.Ceiling(this.Bestelling.Count / 7.0);
                                Console.Clear();
                                Console.WriteLine(ASCIIART.ReserverenArt());
                                Console.WriteLine("Dit zijn de huidige gerechten die u heeft besteld\nToets op het getal naast het gerecht om het te verwijderen uit uw reservering\n");
                                Console.WriteLine($"Pagina {huidigePaginaNR + 1}/{hoeveelheidPaginas}\n");
                                for (int i = 0; i < 7 && i + huidigePaginaNR * 7 < this.Bestelling.Count; i++)
                                {
                                    string totaalprijs = "" + this.Bestelling[i + huidigePaginaNR * 7].TotaalPrijs;
                                    totaalprijs += (!totaalprijs.Contains(',') ? ",-" : totaalprijs[totaalprijs.Length - 2] == ',' ? "0" : "");
                                    Console.Out.WriteLine($"{i + 1}: {this.Bestelling[i + huidigePaginaNR * 7].Naam}: {this.Bestelling[i + huidigePaginaNR * 7].Aantal}x, €{totaalprijs}");
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
                Console.WriteLine(ASCIIART.TafelsArt());
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
                        tafels.BekijkVrijeTafels($"{this.Tijd}{this.Datum}");
                        break;
                    case '2':
                        this.Tafels = tafels.KoppelenDoorMedewerker(this.Personen, this.Tafels, $"{this.Tijd}{this.Datum}"); //Personen -> aantal personen, Tafels -> gereserveerde tafels die al gekoppeld zijn.
                        break;
                    case '3':
                        this.Tafels = tafels.AutomatischKoppelen(this.Personen, this.Tafels, $"{this.Tijd}{this.Datum}"); //Personen -> aantal personen, Tafels -> gereserveerde tafels die al gekoppeld zijn.
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

        public void RemoveTafels(TafelArray tafels, bool klantCall = false) //klantCall is een variabele die op true wordt gezet als er vanuit een klant een reservering wordt verwijderd.
        {
            bool wrongInput = false;
            while (true)
            {
                if (klantCall)
                {
                    this.Tafels = tafels.AllesAutomatischOntkoppelen(this.Tafels, $"{this.Tijd}{this.Datum}", true);
                    return;
                }
                Console.Clear();
                Console.WriteLine(ASCIIART.TafelsOntkoppelenArt());
                Console.WriteLine("1: Alles ontkoppelen");
                Console.WriteLine("2: Ontkoppelen met tafel ID");
                Console.WriteLine("0: Terug");
                if (wrongInput)
                    Console.WriteLine("Wrong input! Probeer 1, 2 of 0");
                char userInput = Console.ReadKey().KeyChar;
                switch (userInput)
                {
                    case '1':
                        this.Tafels = tafels.AllesAutomatischOntkoppelen(this.Tafels, $"{this.Tijd}{this.Datum}");
                        break;
                    case '2':
                        this.Tafels = tafels.OntKoppelenMetID(this.Tafels, $"{this.Tijd}{this.Datum}");
                        break;
                    case '0':
                        return;
                    default:
                        wrongInput = true;
                        break;

                }
            }
        }
        /// <summary>
        /// Laat de gegevens van de reservering zien
        /// </summary>
        public void Info()
        {
            Console.WriteLine(ASCIIART.ReserverenArt());
            Console.WriteLine("Klant:\t\t" + this.Bezoeker);
            Console.WriteLine("Tijd:\t\t" + TijdString());
            Console.WriteLine("Datum:\t\t" + this.Datum);
            Console.WriteLine("Personen:\t" + this.Personen);
            Console.WriteLine("Gerechten:");
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            foreach (var bestellingItem in this.Bestelling)
            {
                string totaalprijs = "" + bestellingItem.TotaalPrijs;
                totaalprijs += (!totaalprijs.Contains(',') ? ",-" : totaalprijs[totaalprijs.Length - 2] == ',' ? "0" : "");
                Console.Out.WriteLine($"- {bestellingItem.Naam}: ({bestellingItem.Aantal}x), €{totaalprijs}");
            }
            Console.WriteLine("Tafels:");
            if (this.Tafels == null)
                Console.WriteLine("Nog geen tafels gekoppeld aan deze reservering.");
            else
            {
                foreach (Tafel tafel in this.Tafels)
                    Console.WriteLine($"- {tafel.ID}: {tafel.Plekken} plekken");
            }

        }

        public bool HeeftTafelsNodig() //returnt true als de hoeveelheid personen groter is dan de aantal plekken van de tafels van de reservering.
        {
            int aantalPlekkenAlGekoppeld = 0;
            if (this.Tafels == null)
                return true;
            foreach (Tafel tafel in this.Tafels)
                aantalPlekkenAlGekoppeld += tafel.Plekken;
            return aantalPlekkenAlGekoppeld >= this.Personen ? false : true;
        }

        public bool HeeftTafels() //returnt true als er 1+ tafels in de reservering aanwezig zijn.
        {
            if (this.Tafels == null)
                return false;
            foreach (Tafel tafel in this.Tafels)
                return true;
            return false;
        }
        /// <summary>
        /// Laat info over de reservering zien en geeft de mogelijke opties weer
        /// </summary>
        /// <returns>De keuze van de klant</returns>
        public char Create()
        {
            Console.Clear();
            Console.WriteLine(TheFatDuckRestaurant.ASCIIART.ReserverenArt());
            Console.WriteLine("1: Datum\t\t" + (this.Datum == "" ? "U heeft nog geen datum gekozen" : $"({this.Datum})"));
            Console.WriteLine("2: Tijd\t\t\t" + (this.Tijd == 0 ? "U heeft nog geen tijd gekozen" : $"({TijdString()})"));
            Console.WriteLine("3: Aantal personen\t" + (this.Personen == 0 ? "U heeft nog niet het aantal personen aangegeven" : $"({this.Personen} personen)"));
            Console.WriteLine("4: Gerechten\t\t" + ((this.Bestelling == null || this.Bestelling.Count == 0) ? "U heeft nog geen gerechten gekozen" : (this.Bestelling.Count == 1) ? $"({this.Bestelling.Count} gerecht)" : $"({this.Bestelling.Count} verschillende gerechten)"));
            Console.WriteLine($"\n5: Bevestig de reservering\n0: Annuleer de reservering");
            char Input = Console.ReadKey().KeyChar;
            Console.Clear();
            return Input;
        }
        /// <summary>
        /// Verandert het aantal personen, mits deze groter is dan nul en kleiner dan de max
        /// </summary>
        /// <param name="MaxPersonen">Maximale aantal personen</param>
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
        /// <summary>
        /// Verandert de tijd, mits deze binnen de beschikbare uren valt
        /// </summary>
        /// <returns>0 als de tijd buiten de uren valt, anders de gegeven tijd</returns>
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
        /// <summary>
        /// Verandert de datum, mits het een geldige datum is
        /// </summary>
        /// <returns>De nieuwe datum-string</returns>
        public string changeDatum()
        {
            Console.WriteLine(TheFatDuckRestaurant.ASCIIART.ReserverenArt());
            Console.WriteLine((this.Datum == "" ? "Nog geen datum" : $"({this.Datum})") + "\x0aWelke datum wilt u reserveren? (21 juni)");
            string NieuweDatum = Console.ReadLine();
            Console.Clear();
            return CheckDatum.checkDatum(NieuweDatum);
        }
        /// <summary>
        /// Zet tijd om van string naar int
        /// </summary>
        /// <returns>De tijd in int</returns>
        public string TijdString() // zet tijd in int om naar tijd in string
        {
            string tstring = this.Tijd / 100 + ":" + this.Tijd % 100;
            return tstring.Length < 5 ? tstring + "0" : tstring;
        }
        private string showTafels()
        {
            if (this.Tafels == null)
                return "Nog geen tafels";

            string returnString = "";
            foreach (Tafel tafel in Tafels)
                returnString += tafel.ID + "\n";
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

        private Func<int, Func<double, double>> CalculatePrice = x => y => x * y;
        [JsonIgnore]
        public double TotaalPrijs
        {
            get => CalculatePrice(this.Aantal)(this.Prijs);
        }
    }
}
