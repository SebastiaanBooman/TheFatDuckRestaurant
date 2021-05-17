using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using static TheFatDuckRestaurant.MainClass;
using static TheFatDuckRestaurant.Reserveren;
using static TheFatDuckRestaurant.ASCIIART;
using static TheFatDuckRestaurant.Menu;

namespace TheFatDuckRestaurant
{
    public class Gebruikers
    {
        public Klant[] Klanten { get; set; }
        public Medewerker[] Medewerkers { get; set; }
        public Eigenaar eigenaar { get; set; }

        public Gebruikers() { } //Empty constructor for json deserialisen.
        public Gebruikers(Klant[] klanten, Medewerker[] medewerkers, Eigenaar eigenaar)
        {
            this.Klanten = klanten;
            this.Medewerkers = medewerkers;
            this.eigenaar = eigenaar;
        }

        public Gebruiker accountManager(Gebruiker gebruiker)
        {
            bool verkeerdeInput = false;
            bool passed = false;
            while (!passed)
            {
                Console.Clear();
                Console.WriteLine(ASCIIART.LoginArt());
                Console.WriteLine("1: Login als klant\x0a");
                Console.WriteLine("2: Login als medewerker\n");
                Console.WriteLine("3: Registreer een nieuw account als klant\x0a");
                Console.WriteLine("0: Terug\x0a");

                if (verkeerdeInput)
                {
                    Console.WriteLine("Verkeerde input, probeer 1,2,3");
                    verkeerdeInput = false;
                }

                ConsoleKeyInfo userInput = Console.ReadKey();
                char userInputChar = userInput.KeyChar;
                switch (userInputChar)
                {
                    case '1':
                        gebruiker = logIn("Klant");//return logIn();
                        if (gebruiker as Klant != null)
                            return gebruiker;
                        break;
                    case '2':
                        gebruiker = logIn("Medewerker");
                        if (gebruiker as Medewerker != null)
                            return gebruiker;
                        break;
                    case '3':
                        gebruiker = registreer(gebruiker);
                        if(gebruiker as Klant != null)
                            return gebruiker;
                        break;
                    case '0':
                        return gebruiker;
                    default:
                        verkeerdeInput = true;
                        break;
                }
            }
            return null;
        }
        public Gebruiker logIn(string gebruikerType)
        {
            Func< Gebruiker, Tuple<bool, string>> CheckWachtwoord = (gebruikerObject) =>
            {
                string Input = Console.ReadLine();
                return Tuple.Create(Input == gebruikerObject.Wachtwoord || Input == "0", Input);
            };
            bool passed = false;
            while (!passed)
            {
                //returnt een tuple die aangeeft of de input het juiste wachtwoord of 'terug' is en de input als een string
                bool NaamBestaat = false;
                Klant klantObject = null;
                Medewerker medewerkerObject = null;
                Console.Clear();
                Console.WriteLine(ASCIIART.LoginArt());
                Console.WriteLine("Voer uw gebruikersnaam in\x0A\x0A" + "0: Terug");
                string GegevenNaam = Console.ReadLine();
                if(GegevenNaam == "0")
                    return new Gebruiker("", "", "", "");
                
                if(gebruikerType == "Klant")
                {
                    foreach (Klant klant in Klanten)
                    {
                        if (GegevenNaam == klant.Naam)
                        {
                            NaamBestaat = true;
                            klantObject = klant;
                        }
                    }
                }
                else
                {
                    foreach (Medewerker medewerker in Medewerkers)
                    {
                        if (GegevenNaam == medewerker.Naam)
                        {
                            NaamBestaat = true;
                            medewerkerObject = medewerker;

                        }
                    }
                }
                if (NaamBestaat)
                {
                    Tuple<bool, string> Password = Tuple.Create(false, "");
                    Console.Clear();
                    Console.WriteLine(ASCIIART.LoginArt());
                    Console.WriteLine($"Gebruikersnaam: {GegevenNaam}\x0A\x0AVoer uw wachtwoord in");
                    if(gebruikerType == "Klant")
                       Password = CheckWachtwoord(klantObject);
                    else
                    {
                       Password = CheckWachtwoord(medewerkerObject);
                    }
                    while (!Password.Item1) //blijft om het wachtwoord vragen totdat het juiste wachtwoord voor de gebruikersnaam wordt gegeven of er 'terug' wordt getypt
                    {
                        Console.Clear();
                        Console.WriteLine(ASCIIART.LoginArt());
                        Console.WriteLine("Verkeerd wachtwoord\x0A\x0A\x0AVoer uw wachtwoord in\x0A\x0A" + "0: Ga terug naar het vorige scherm");
                        if(klantObject != null) //Als klantObject geen null is, betekent dat de gebruiker in wilt loggen als klant
                            Password = CheckWachtwoord(klantObject);
                        else
                        {
                            Password = CheckWachtwoord(medewerkerObject);
                        }
                    }
                    if (Password.Item2 != "0") //sluit het inlogscherm af wanneer 'terug' was getypt
                    {
                        Console.Clear();
                        Console.WriteLine(ASCIIART.LoginArt());
                        Console.WriteLine("U bent ingelogd!\x0A\x0A" + "Enter: Ga terug naar het sartscherm");
                        Console.ReadLine();
                        if(klantObject != null)
                            return klantObject;
                        return medewerkerObject;
                    }
                }
                else //reset het inlogscherm wanneer een nog niet geregistreerde gebruikersnaam wordt gegeven of sluit het inlogscherm af wanneer '0' is ingevoerd
                {
                    Console.Clear();
                    Console.WriteLine(ASCIIART.LoginArt());
                    Console.WriteLine("Verkeerde gebruikersnaam.\x0A\x0A" + "Enter: Probeer opnieuw in te loggen\x0A\x0A" + "0: Terug");
                    if (Console.ReadKey().KeyChar == '0')
                    {
                        return new Gebruiker("", "", "", "");
                    }
                    }
                }
            return null;
        }

        public Gebruiker registreer(Gebruiker gebruiker)
        {
            var jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            Console.Clear();
            Console.WriteLine(ASCIIART.RegistrerenArt());
            Console.WriteLine("Voer uw gebruikers naam in\n0: Terug");
            var naamInput = Console.ReadLine();
            Console.Clear();

            if (naamInput != "0")
            {
                foreach (var klant in Klanten)
                {
                    while (klant.Naam == naamInput)
                    {
                        Console.Clear();
                        Console.WriteLine(ASCIIART.RegistrerenArt());
                        Console.WriteLine("Deze naam bestaat al in het systeem! Probeer een andere");
                        naamInput = Console.ReadLine();
                    }
                }
                Console.Clear();
                Console.WriteLine(ASCIIART.RegistrerenArt());
                Console.WriteLine("Voer uw wachtwoord in:");
                string password = Console.ReadLine(); //TODO: Check voor password met requirements

                Console.Clear();
                Console.WriteLine(ASCIIART.RegistrerenArt());
                Console.WriteLine("Voer uw adres in:");
                string adres = Console.ReadLine(); //TODO: Check voor adres met requirements

                Console.Clear();
                Console.WriteLine(ASCIIART.RegistrerenArt());
                Console.WriteLine("Voer uw woonplaats in:");
                string woonplaats = Console.ReadLine(); //TODO: Check voor woonplaats met requirements


                Klant[] nieuweKlantenLijst = new Klant[Klanten.Length + 1];

                for (int i = 0; i < Klanten.Length; i++)
                {
                    nieuweKlantenLijst[i] = Klanten[i]; //Voert alle oude gebruikers als Gebruiker object in de nieuwe lijst
                }
                Klant nieuweKlant = new Klant(naamInput, password, adres, woonplaats, 0); //nieuwe klant word aangemaakt
                nieuweKlantenLijst[nieuweKlantenLijst.Length - 1] = nieuweKlant; //voegt nieuwe klant toe aan lijst
                Klanten = nieuweKlantenLijst; //Klanten array van gebruikers wordt aangepast naar de nieuwe lijst die is gemaakt.
                var toSerializeKlant = JsonSerializer.Serialize(this, jsonOptions);
                File.WriteAllText("gebruikers.json", toSerializeKlant);


                Console.Clear();
                Console.WriteLine(ASCIIART.RegistrerenArt());
                Console.WriteLine($"Welkom nieuwe klant: {naamInput}!\nKlik op een toets om verder te gaan");
                Console.ReadKey();
                return nieuweKlant;
            }

            else
            {
                return gebruiker; //Als een persoon toch geen nieuw account wilt returnt de functie gewoon de oude standaard gebruiker account
            }
        }

        public Gebruiker logOut()
        {
            bool passed = false;
            bool wrongInput = false;
            while (!passed)
            {
                Console.Clear();
                Console.WriteLine(ASCIIART.LogoutArt());
                Console.WriteLine("U bent uitgelogd!\n\n0: Terug");
                if (wrongInput)
                    Console.WriteLine("Verkeerde Input! Probeer 0");
                char userInput = Console.ReadKey().KeyChar;
                if (userInput == '0')
                    return new Gebruiker("", "", "","");
                wrongInput = true;
            }
            return null;
        }
    }

}

    public class Gebruiker
    {
        public string Naam { get; set; }
        public string Wachtwoord { get; set; }
        public string Adres { get; set; }
        public string Woonplaats { get; set; }

        public Gebruiker(string naam, string wachtwoord, string adres, string woonplaats)
        {
            this.Naam = naam;
            this.Wachtwoord = wachtwoord;
            this.Adres = adres;
            this.Woonplaats = woonplaats;
        }
        public Gebruiker() { }

        public virtual TheFatDuckRestaurant.Menu bekijkMenu(TheFatDuckRestaurant.Menu menu)
        {
            menu.BekijkMenuGebruiker();
            //menu.Bekijkmenu -> Gebruiker
            //menu.ReserveerMenu -> Klant
            //menu.PasAanMenu -> Medewerker
            return menu;
        }
        public virtual ReserveerLijst reserveer(TheFatDuckRestaurant.Menu menu, ReserveerLijst reserveerLijst)
        {
        return reserveerLijst;
        }

    public virtual void bekijkReserveringen(ReserveerLijst Reserveerlijst)
    {
        if (Reserveerlijst.Reserveringen == null)
        {
            Reserveerlijst.Reserveringen = new Reservering[0];
        }
        if (Reserveerlijst.Reserveringen.Length == 0)
        {
            Console.Clear();
            Console.WriteLine("Er zijn nog geen reserveringen gemaakt\x0a\x0a" + "Enter: Ga terug naar het startscherm");
            Console.ReadKey();
            return;
        }
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Voor welke datum wilt u de reserveringen bekijken? (21 juni)");
            string datum = Console.ReadLine();
            Console.Clear();
            string datumLower = "";
            foreach (char sym in datum)
            {
                if (Char.IsLetter(sym))
                {
                    Char.ToLower(sym);
                }
                datumLower += sym;
            }
            int Aantal = 0;
            foreach (Reservering reservering in Reserveerlijst.Reserveringen)
            {
                if (reservering.Datum == datumLower)
                {
                    Aantal++;
                }
            }

            if(Aantal > 0)
            {
                int huidigePaginaNR = 0;
                //double TotalRevenue = 0.0;
                Reservering[] RelevanteReserveringen = new Reservering[Aantal];
                int j = 0;
                foreach (Reservering reservering in Reserveerlijst.Reserveringen)
                {
                    if (reservering.Datum == datumLower)
                    {
                        RelevanteReserveringen[j++] = reservering;
                        /*foreach(Gerecht gerecht in reservering)
                         * {
                         *  TotalRevenue += gerecht.Prijs;
                         * }
                         */
                    }
                }
                int hoeveelheidPaginas = (int)Math.Ceiling(RelevanteReserveringen.Length / 7.0);
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine($"{datumLower}\nPagina {huidigePaginaNR + 1}/{hoeveelheidPaginas}\n");
                    for(int i = 0; i < 7 && i + huidigePaginaNR * 7 < RelevanteReserveringen.Length; i++)
                    { 
                        Console.WriteLine($"{i+1}: {RelevanteReserveringen[i + huidigePaginaNR * 7].TijdString()} {RelevanteReserveringen[i + huidigePaginaNR * 7].Bezoeker.Naam} ({RelevanteReserveringen[i + huidigePaginaNR * 7].Personen} personen)");
                    }
                    Console.WriteLine();
                    if (huidigePaginaNR + 1 < hoeveelheidPaginas)
                        Console.WriteLine("8: Volgende pagina");
                    if (huidigePaginaNR + 1 >= hoeveelheidPaginas && (hoeveelheidPaginas > 1))
                        Console.WriteLine("9: Vorige pagina");
                    Console.WriteLine("0: Ga terug naar het startscherm");
                    //Console.WriteLine(TotalRevenue + " euro");
                    int Index = Int32.Parse(Console.ReadKey().KeyChar.ToString());
                    Console.Clear();
                    if (Index == 0)
                    {
                        return;
                    }
                    if (Index > 0 && Index < 8)
                    {
                        RelevanteReserveringen[Index - 1].Info();
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
                        Console.WriteLine("Dit is geen geldige input");
                        Console.WriteLine("\x0a" + "Enter: Ga terug naar het vorige scherm");
                        Console.ReadKey();
                    }
                }
            }
            Console.WriteLine("Er zijn nog geen reserveringen gedaan voor deze datum\x0a");
            Console.WriteLine("Enter: Ga terug naar het startscherm");
            Console.ReadKey();
            /*string ReserveringString = "";
            foreach (Reservering reservering in Reserveerlijst.Reserveringen)
            {
                if (reservering.Datum == datumLower)
                {
                    ReserveringString += $"{reservering.TijdString()} {reservering.Bezoeker.Naam} ({reservering.Personen} personen)\x0a";
                    reservering.Info();
                }
            }
            Console.WriteLine(ReserveringString == "" ? "Er zijn nog geen reserveringen gedaan voor deze datum\x0a" : ReserveringString);
            Console.WriteLine("Enter: Ga terug naar het startscherm");
            Console.ReadKey();
            return;*/
        }
    }

        public virtual string bekijkDailyRevenue()
        {
        return null;
        }

        public virtual string bekijkClickStream()
        {
        return null;
        }

        public virtual void bekijkReserveringen() { }

        public virtual void bekijkAccount() { }

        public virtual char startScherm()
        {
            bool verkeerdeInput = false;
            bool passed = false;
            while (!passed)
            {
                Console.Clear();
                Console.WriteLine(TheFatDuckRestaurant.ASCIIART.GeneralArt());
                Console.WriteLine("STATUS: Uitgelogd\x0a");
                Console.WriteLine("1: Informatie\x0a");
                Console.WriteLine("2: Login\x0a");
                Console.WriteLine("3: Bezichtig het menu\x0a");

                if (verkeerdeInput)
                {
                    Console.WriteLine("Verkeerde input, probeer 1,2,3");
                    verkeerdeInput = false;
                }

                ConsoleKeyInfo userInput = Console.ReadKey();
                char userInputChar = userInput.KeyChar;
                switch (userInputChar)
                {
                    case '1':
                        return '1';
                    case '2':
                        return '2';
                    case '3':
                        return '4';
                    default:
                        verkeerdeInput = true;
                        break;
                }
            }
            return '0'; 
        }
    }

    public class Klant : Gebruiker
    {
        public int AantalReserveringen { get; set; }
        
        public Klant(string naam, string wachtwoord, string adres, string woonplaats, int reserveringen) : base(naam, wachtwoord, adres, woonplaats)
        {
            this.AantalReserveringen = reserveringen;
        }
    public Klant() { }

    public override TheFatDuckRestaurant.Menu bekijkMenu(TheFatDuckRestaurant.Menu menu)
    {
        menu.BekijkMenuKlant();
        //menu.Bekijkmenu -> Gebruiker
        //menu.ReserveerMenu -> Klant
        //menu.PasAanMenu -> Medewerker
        return menu;
    }

    public override void bekijkReserveringen(ReserveerLijst Reserveerlijst)
    {
        if (Reserveerlijst.Reserveringen == null)
        {
            Reserveerlijst.Reserveringen = new Reservering[0];
        }
        if (Reserveerlijst.Reserveringen.Length == 0)
        {
            Console.Clear();
            Console.WriteLine("U heeft nog geen reserveringen gemaakt\x0a\x0a" + "Enter: Ga terug naar het startscherm");
            Console.ReadKey();
            return;
        }
        int huidigePaginaNR = 0;
        Reservering[] KlantReserveringen = new Reservering[this.AantalReserveringen];
        int j = 0;
        for (int i = 0; i < Reserveerlijst.Reserveringen.Length; i++)
        {
            if (Reserveerlijst.Reserveringen[i].Bezoeker.Naam == this.Naam)
            {
                KlantReserveringen[j++] = Reserveerlijst.Reserveringen[i];
            }
        }
        int hoeveelheidPaginas = (int)Math.Ceiling(KlantReserveringen.Length / 7.0);
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"Pagina {huidigePaginaNR + 1}/{hoeveelheidPaginas}\n");
            for (int i = 0; i < 7 && i + huidigePaginaNR * 7 < KlantReserveringen.Length; i++)
            {
                Console.WriteLine($"{i+1}: {KlantReserveringen[i + huidigePaginaNR * 7].Datum} om {KlantReserveringen[i + huidigePaginaNR * 7].TijdString()} ({KlantReserveringen[i + huidigePaginaNR * 7].Personen} personen)");
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
            {
                return;
            }
            if (Index < 7 && Index > 0)
            {
                if (!Reserveerlijst.changeReservering(KlantReserveringen[Index - 1]))
                {
                    this.AantalReserveringen -= 1;
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
    public override char startScherm()
        {
            bool verkeerdeInput = false;
            bool passed = false;
            while (!passed)
            {
                Console.Clear();
                Console.WriteLine(TheFatDuckRestaurant.ASCIIART.GeneralArt());
                Console.WriteLine($"STATUS: Ingelogd als klant, welkom {Naam}\x0a");
                Console.WriteLine("1: Restaurant informatie\x0a");
                Console.WriteLine("2: Account informatie\n");
                Console.WriteLine("3: Logout\n");
                Console.WriteLine("4: Bezichtig het menu\x0a");
                Console.WriteLine("5: Reserveren\x0a");
                Console.WriteLine("6: Bezichtig uw reserveringen");

                if (verkeerdeInput)
                {
                    Console.WriteLine("Verkeerde input, probeer 1,2,3,4 of 5");
                    verkeerdeInput = false;
                }

                ConsoleKeyInfo userInput = Console.ReadKey();
                char userInputChar = userInput.KeyChar;
                switch (userInputChar)
                {
                    case '1':
                        return '1';
                    case '2':
                        return 'A';
                    case '3':
                        return '3';
                    case '4':
                        return '4';
                    case '5':
                        return '5';
                    case '6':
                        return '9';
                    default:
                        verkeerdeInput = true;
                        break;
                }
            }
            return '0';
        }

    public override void bekijkAccount()
    {
        bool verkeerdeInput = false;
        bool passed = false;
        while (!passed)
        {
            Console.Clear();
            Console.WriteLine(TheFatDuckRestaurant.ASCIIART.AccountArt());
            Console.WriteLine($"Account Naam: {Naam}\x0a");
            Console.WriteLine($"Account Type: Klant\x0a");
            Console.WriteLine($"1: Account Reserveringen\x0a");
            Console.WriteLine("0: Return");
            if (verkeerdeInput)
            {
                Console.WriteLine("Verkeerde Input! Probeer 1 of 0");
                verkeerdeInput = false;
            }

            char userInput = Console.ReadKey().KeyChar;
            switch (userInput)
            {
                case '1':
                //call to account reserveringen.
                case '0':
                    return;
                default:
                    verkeerdeInput = true;
                    break;
            }
        }
    }

    public override void bekijkReserveringen()
    {
        Console.Clear();
        Console.WriteLine(TheFatDuckRestaurant.ASCIIART.ReserverenArt());
        if (this.AantalReserveringen == 0)
        {
            Console.WriteLine("U heeft momenteel geen reserveringen! Klik op een toets om terug te gaan");
            Console.ReadLine();
            return;
        }
        return; //functionaliteit voor als er wel reserveringen zijn gemaakt.
    }
}


    public class Medewerker : Gebruiker
    {
        public override TheFatDuckRestaurant.Menu bekijkMenu(TheFatDuckRestaurant.Menu menu)
        {
            menu.BekijkMenuMedewerker();
            //menu.Bekijkmenu -> Gebruiker
            //menu.ReserveerMenu -> Klant
            //menu.PasAanMenu -> Medewerker
            return menu;
        }
        public override char startScherm()
        {
            bool verkeerdeInput = false;
            bool passed = false;
            while (!passed)
            {
                Console.Clear();
                Console.WriteLine(TheFatDuckRestaurant.ASCIIART.GeneralArt());
                Console.WriteLine("STATUS: Ingelogd als medewerker\x0a");
                Console.WriteLine("1: Informatie\x0a");
                Console.WriteLine("2: Logout\n");
                Console.WriteLine("3: Bezichtig het menu\x0a");
                Console.WriteLine("4: Bezichtig reserveringen\x0a");
                Console.WriteLine("5: Bezichtig de Clickstream\x0a");
                Console.WriteLine("6: Bezichtig de dagelijkse opbrengsten\x0a");
                Console.WriteLine("0: Applicatie afsluiten\x0a");

                if (verkeerdeInput)
                {
                    Console.WriteLine("Verkeerde input, probeer 1,2,3,4,5,6 of 0");
                    verkeerdeInput = false;
                }

                ConsoleKeyInfo userInput = Console.ReadKey();
                char userInputChar = userInput.KeyChar;
                switch (userInputChar)
                {
                    case '1':
                        return '1';
                    case '2':
                        return '3';
                    case '3':
                        return '4';
                    case '4':
                        return '9';
                    case '5':
                        return '7';
                    case '6':
                        return '6';
                    case '0':
                        return '0';
                    default:
                        verkeerdeInput = true;
                        break;
                }
            }
            return '0';
        }
    }

    public class Eigenaar : Medewerker
    {
        public override char startScherm()
        {
            bool verkeerdeInput = false;
            bool passed = false;
            while (!passed)
            {
                Console.Clear();
                Console.WriteLine(TheFatDuckRestaurant.ASCIIART.GeneralArt());
                Console.WriteLine("1: Informatie\x0a");
                Console.WriteLine("2: Login\x0a");
                Console.WriteLine("3: Logout\n");
                Console.WriteLine("4: Bezichtig het menu\x0a");
                Console.WriteLine("5: Bezichtig reserveringen\x0a");
                Console.WriteLine("6: Bezichtig de Clickstream\x0a");
                Console.WriteLine("0: Applicatie afsluiten\x0a");

                if (verkeerdeInput)
                    Console.WriteLine("Verkeerde input, probeer 1,2,3,4,5,6 of 0");

                ConsoleKeyInfo userInput = Console.ReadKey();
                char userInputChar = userInput.KeyChar;
                switch (userInputChar)
                {
                    case '1':
                        return '1';
                    case '2':
                        return '2';
                    case '3':
                        return '4';
                    case '4':
                        return '3';
                    case '5':
                        return '5';
                    case '6':
                        return '7';
                    case '0':
                        return '0';
                    default:
                        verkeerdeInput = true;
                        break;
                }
            }
            return '0';
        }
    }