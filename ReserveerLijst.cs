using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

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
                Console.WriteLine(ASCIIART.ReserveringenArt());
                Console.WriteLine("Er zijn nog geen reserveringen gemaakt\x0a\x0a" + "Klik op een toets om terug te gaan");
                Console.ReadKey();
                return this;
            }
            while (true)
            {
                Console.Clear();
                Console.WriteLine(ASCIIART.ReserveringenArt());
                Console.WriteLine("Voor welke datum wilt u de reserveringen bekijken? (21 juni)\n\nEnter: Ga terug naar het vorige scherm");
                string datum = Console.ReadLine();
                datum.ToLower();
                Console.Clear();
                if (datum == "")
                    return this;
                datum = CheckDatum.checkDatum(datum);
                int AantalRelevanteReserveringen = BerekenRelevanteReserveringen(datum);
                if (AantalRelevanteReserveringen <= 0)
                {
                    Console.WriteLine(ASCIIART.ReserveringenArt());
                    Console.WriteLine("Er zijn nog geen reserveringen gedaan voor deze datum\x0a");
                    Console.WriteLine("Klik op een toets om terug te gaan");
                    Console.ReadKey();
                }
                else
                {
                    Reservering[] RelevanteReserveringen = new Reservering[AantalRelevanteReserveringen];
                    int j = 0;
                    foreach (Reservering reservering in Reserveringen)
                    {
                        if (reservering.Datum == datum)
                            RelevanteReserveringen[j++] = reservering;
                    }
                    BekijkSpecifiekePaginaMedewerker(RelevanteReserveringen, tafels, datum);
                }
            }
        }

        public void BekijkSpecifiekePaginaMedewerker(Reservering[] RelevanteReserveringen, TafelArray tafels, string datum)
        {
            int huidigePaginaNR = 0;
            bool wrongInput = false;
            while (true)
            {
                int hoeveelheidPaginas = (int)Math.Ceiling(RelevanteReserveringen.Length / 7.0);
                Console.Clear();
                Console.WriteLine(ASCIIART.ReserveringenArt());
                Console.WriteLine($"{datum}\nPagina {huidigePaginaNR + 1}/{hoeveelheidPaginas}\n");
                for (int i = 0; i < 7 && i + huidigePaginaNR * 7 < RelevanteReserveringen.Length; i++)
                    Console.WriteLine($"{i + 1}: {RelevanteReserveringen[i + huidigePaginaNR * 7].TijdString()} {RelevanteReserveringen[i + huidigePaginaNR * 7].Bezoeker} ({RelevanteReserveringen[i + huidigePaginaNR * 7].Personen} personen)");
                Console.WriteLine();
                if (huidigePaginaNR + 1 < hoeveelheidPaginas)
                    Console.WriteLine("8: Volgende pagina");
                if (huidigePaginaNR + 1 >= hoeveelheidPaginas && (hoeveelheidPaginas > 1))
                    Console.WriteLine("9: Vorige pagina");
                Console.WriteLine("0: Ga terug naar het startscherm");
                if (wrongInput)
                {
                    Console.WriteLine("Verkeerde input!");
                    wrongInput = false;
                }
                try
                {
                    int Index = Int32.Parse(Console.ReadKey().KeyChar.ToString());
                    if (Index == 0)
                        return;
                    if (Index > 0 && Index < 8)
                    {
                        try
                        {
                            BekijkSpecifiekeReserveringMedewerker(RelevanteReserveringen[Index - 1], tafels);
                        }
                        catch (IndexOutOfRangeException)
                        {
                            wrongInput = true;
                        }
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
                catch (FormatException)
                {
                    wrongInput = true;
                }
            }
        }

        public void BekijkSpecifiekeReserveringMedewerker(Reservering reservering, TafelArray tafels) //Laat een specifieke reservering zien, met de opties om tafels toe te voegen of te verwijderen, als dit mogelijk is.
        {
            bool wrongInput = false;
            while (true)
            {
                Console.Clear();
                reservering.Info();
                bool heeftTafelsNodig = reservering.HeeftTafelsNodig();
                bool heeftTafels = reservering.HeeftTafels();
                if (heeftTafelsNodig)
                    Console.WriteLine("\nA: Tafels koppelen");
                if (heeftTafels)
                    Console.WriteLine("\nB: Tafels ontkoppelen");
                Console.WriteLine("0: Terug");
                if (wrongInput)
                    Console.WriteLine("Verkeerde Input!");
                char userInput = Console.ReadKey().KeyChar;
                if (userInput == '0')
                    return;
                else if (userInput == 'A' && heeftTafelsNodig)
                    reservering.AddTafels(tafels);
                else if (userInput == 'B' && heeftTafels)
                    reservering.RemoveTafels(tafels);
                else
                    wrongInput = true;
            }
        }

        public int BerekenRelevanteReserveringen(string datum) //Neemt als input een datum string en returnt de hoeveelheid gereserveringen die op die datum een reservering hebben.
        {
            int AantalRelevanteReserveringen = 0;
            foreach (Reservering reservering in Reserveringen)
            {
                if (reservering.Datum == datum)
                    AantalRelevanteReserveringen++;
            }
            return AantalRelevanteReserveringen;
        }

        public void BekijkReserveringenKlant(string klantNaam, TafelArray tafels)
        {
            if (Reserveringen == null)
                Reserveringen = new Reservering[0];

            if (Reserveringen.Length == 0)
            {
                Console.Clear();
                Console.WriteLine(ASCIIART.ReserverenArt());
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
                Console.WriteLine(ASCIIART.ReserveringenArt());
                Console.WriteLine($"Pagina {huidigePaginaNR + 1}/{hoeveelheidPaginas}\n");
                for (int i = 0; i < 7 && i + huidigePaginaNR * 7 < KlantReserveringen.Length; i++)
                    Console.WriteLine($"{i + 1}: {KlantReserveringen[i + huidigePaginaNR * 7].Datum} om {KlantReserveringen[i + huidigePaginaNR * 7].TijdString()} ({KlantReserveringen[i + huidigePaginaNR * 7].Personen} personen)");

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
                    changeReservering(KlantReserveringen[Index - 1], tafels); //TODO: Opties om reserveringen aan te passen die zijn gemaakt.
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



        public void changeReservering(Reservering reservering, TafelArray tafels)
        {

            while (true)
            {
                Console.Clear();
                reservering.Info();
                Console.WriteLine("\nR: Verwijder reservering\n0: Terug");
                ConsoleKeyInfo toetsUser = Console.ReadKey();
                char toetsUserChar = toetsUser.KeyChar;
                if (toetsUserChar == '0')
                    return;
                if (toetsUserChar == 'R' || toetsUserChar == 'r')
                {
                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine(ASCIIART.ReserverenArt());
                        Console.WriteLine($"Weet u zeker dat u uw reservering voor {reservering.Datum} wil verwijderen?\n\nR: Verwijder reservering\n0: Terug");

                        ConsoleKeyInfo toetsUserBevestig = Console.ReadKey();
                        char toetsUserBevestigChar = toetsUserBevestig.KeyChar;

                        if (toetsUserBevestigChar == '0')
                            break;
                        if (toetsUserBevestigChar == 'r' || toetsUserBevestigChar == 'R')
                        {
                            removeReservering(reservering, tafels);
                            Console.Clear();
                            Console.WriteLine(ASCIIART.ReserverenArt());
                            Console.WriteLine("Uw reservering is succesvol verwijderd\n\n0: Terug");
                            Console.ReadKey();
                            return;
                        }
                    }
                }
            }
            //createReservering(reservering.Bezoeker, reservering.Tijd, reservering.Datum, reservering.Personen, null, "Verwijder");
        }
        public void removeReservering(Reservering reservering, TafelArray tafels)
        {
            reservering.RemoveTafels(tafels, true);
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
            if (reservering.Tijd != 0 && reservering.Datum != "" && reservering.Personen != 0 && (reservering.Bestelling != null))
            {
                Reservering[] newReserveringen;
                if (this.Reserveringen != null)
                {
                    newReserveringen = new Reservering[this.Reserveringen.Length + 1];
                    for (int i = 0; i < Reserveringen.Length; i++)
                        newReserveringen[i] = Reserveringen[i];
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
}