using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using static TheFatDuckRestaurant.MainClass;
using static TheFatDuckRestaurant.ReserveerLijst;
using static TheFatDuckRestaurant.ASCIIART;
using static TheFatDuckRestaurant.Menu;
using System.Text.RegularExpressions;
using System.Security;

namespace TheFatDuckRestaurant
{
    public class Gebruikers
    {
        public Klant[] Klanten { get; set; }
        public Medewerker[] Medewerkers { get; set; }
        public Eigenaar Eigenaar { get; set; }

        public Gebruikers() { } //Empty constructor for json deserialisen.
        public Gebruikers(Klant[] klanten, Medewerker[] medewerkers, Eigenaar eigenaar)
        {
            this.Klanten = klanten;
            this.Medewerkers = medewerkers;
            this.Eigenaar = eigenaar;
        }

        public Gebruiker accountManager(Gebruiker gebruiker)
        {
            bool verkeerdeInput = false;
            while (true)
            {
                Console.Clear();
                Console.WriteLine(ASCIIART.LoginArt());
                Console.WriteLine("1: Login als klant\x0a");
                Console.WriteLine("2: Login als medewerker\n");
                Console.WriteLine("3: Login als eigenaar\n");
                Console.WriteLine("4: Registreer een nieuw account als klant\x0a");
                Console.WriteLine("0: Terug\x0a");

                if (verkeerdeInput)
                    Console.WriteLine("Verkeerde input, probeer 1,2,3");

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
                        gebruiker = logIn("Eigenaar");
                        if (gebruiker as Eigenaar != null)
                            return gebruiker;
                        break;
                    case '4':
                        gebruiker = registreerKlant(gebruiker);
                        if (gebruiker as Klant != null)
                            return gebruiker;
                        break;
                    case '0':
                        return gebruiker;
                    default:
                        verkeerdeInput = true;
                        break;
                }
            }
        }
        public Gebruiker logIn(string gebruikerType)
        {
            Func<Gebruiker, Tuple<bool, string>> CheckWachtwoord = (gebruikerObject) =>
           {
               string Input = Console.ReadLine();
               return Tuple.Create(Input == gebruikerObject.Wachtwoord || Input == "0", Input);
           };
            while (true)
            {
                //returnt een tuple die aangeeft of de input het juiste wachtwoord of 'terug' is en de input als een string
                bool NaamBestaat = false;
                Klant klantObject = null;
                Medewerker medewerkerObject = null;
                Eigenaar eigenaarObject = null;

                Console.Clear();
                Console.WriteLine(ASCIIART.LoginArt());
                Console.WriteLine("Voer uw gebruikersnaam in\x0A\x0A" + "0: Terug");
                string GegevenNaam = Console.ReadLine();
                if (GegevenNaam == "0")
                    return new Gebruiker("", "", "", "");

                if (gebruikerType == "Klant")
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
                else if (gebruikerType == "Medewerker")
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
                else
                {
                    if (GegevenNaam == Eigenaar.Naam)
                    {
                        NaamBestaat = true;
                        eigenaarObject = Eigenaar;
                    }
                }
                if (NaamBestaat)
                {
                    Tuple<bool, string> Password = Tuple.Create(false, "");
                    Console.Clear();
                    Console.WriteLine(ASCIIART.LoginArt());
                    Console.WriteLine($"Gebruikersnaam: {GegevenNaam}\x0A\x0AVoer uw wachtwoord in:");
                    if (gebruikerType == "Klant")
                        Password = CheckWachtwoord(klantObject);
                    else if (gebruikerType == "Medewerker")
                        Password = CheckWachtwoord(medewerkerObject);
                    else
                        Password = CheckWachtwoord(eigenaarObject);
                    while (!Password.Item1) //blijft om het wachtwoord vragen totdat het juiste wachtwoord voor de gebruikersnaam wordt gegeven of er 'terug' wordt getypt
                    {
                        Console.Clear();
                        Console.WriteLine(ASCIIART.LoginArt());
                        Console.WriteLine("Verkeerd wachtwoord\x0A\x0A\x0AVoer uw wachtwoord in:\x0A\x0A" + "0: Terug");
                        if (klantObject != null) //Als klantObject geen null is, betekent dat de gebruiker in wilt loggen als klant
                            Password = CheckWachtwoord(klantObject);
                        else
                            Password = CheckWachtwoord(medewerkerObject);
                    }
                    if (Password.Item2 != "0") //sluit het inlogscherm af wanneer 'terug' was getypt
                    {
                        Console.Clear();
                        Console.WriteLine(ASCIIART.LoginArt());
                        Console.WriteLine("U bent ingelogd!\x0A\x0A" + "0: Naar het startscherm");
                        Console.ReadKey();
                        if (klantObject != null)
                            return klantObject;
                        else if (medewerkerObject != null)
                            return medewerkerObject;
                        else
                            return eigenaarObject;
                    }
                }
                else //reset het inlogscherm wanneer een nog niet geregistreerde gebruikersnaam wordt gegeven of sluit het inlogscherm af wanneer '0' is ingevoerd
                {
                    Console.Clear();
                    Console.WriteLine(ASCIIART.LoginArt());
                    Console.WriteLine("Verkeerde gebruikersnaam.\x0A\x0A" + "Enter: Probeer opnieuw in te loggen\x0A\x0A" + "0: Terug");
                    if (Console.ReadKey().KeyChar == '0')
                        return new Gebruiker("", "", "", "");
                }
            }
        }

        public Tuple<string, string, string, string> registreer() //Returned een Tuple met naam,wachtwoord,adres en woonplaats voor registreerKlant en registreerMedewerker functies.
        {
            var jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            // Lambda om te checken of het een geldig wachtwoord is. Gebruikt RegEx
            //      1       2               3                   4                5      6
            // @"   ^  (?=.+?[A-Z])    (?=.+?[0-9])    (?=.+?[^a-zA-Z0-9])     .{8,}    $"
            // 1 is de start van de string (input)
            // 2 is een check of er ergens in de string 1 of meer hoofdletters zijn.
            // 3 is een check of er ergens in de string 1 of meer cijfers zijn.
            // 4 is een check of er ergens in de string 1 of meer characters zijn dat geen kleine letter, hoofdletter of getal is
            // 5 is een check of de string minimaal 8 tot meer characters heeft
            // 6 is het einde van de string (input)

            Func<string, bool> ValidatePassword = (input) =>
            {
                Regex regex = new Regex(@"^(?=.+?[A-Z])(?=.+?[0-9])(?=.+?[^a-zA-Z0-9_@.-]).{8,}$");
                Match match = regex.Match(input);
                return match.Success;
            };
            Console.Clear();
            Console.WriteLine(ASCIIART.RegistrerenArt());
            Console.WriteLine("Voer uw gebruikers naam in\n0: Terug");
            var naamInput = Console.ReadLine();
            bool uniekeNaam = false;
            while (!uniekeNaam)
            {
                bool uniekKlant = true;
                bool uniekMedewerker = true;
                bool uniekEigenaar = true; //3 booleans voor iedere array.

                if (naamInput != "0")
                {
                    foreach (var klant in Klanten) //Checkt per klant in de Klanten array of de naam al bestaat
                    {
                        if (klant.Naam == naamInput)
                        {
                            uniekKlant = false;
                            break;
                        }
                    }
                    foreach (var medewerker in Medewerkers) //Checkt per medewerker in de Medewerkers array of de naam al bestaat
                    {
                        if (medewerker.Naam == naamInput)
                        {
                            uniekMedewerker = false;
                            break;
                        }
                    }
                    if (Eigenaar.Naam == naamInput)
                        uniekEigenaar = false;

                    if (uniekEigenaar == false || uniekKlant == false || uniekMedewerker == false)
                    {
                        Console.Clear();
                        Console.WriteLine(ASCIIART.RegistrerenArt());
                        Console.WriteLine("Deze naam bestaat al in het systeem! Probeer een andere\n0: Terug");
                        naamInput = Console.ReadLine();
                    }
                    else
                        uniekeNaam = true;
                }
            }
                Console.Clear();
                Console.WriteLine(ASCIIART.RegistrerenArt());
                Console.WriteLine("Voer uw wachtwoord in van minimaal 8 tekens waarvan minimaal 1 Hoofdletter, 1 cijfer en 1 speciaal karakter:");
                SecureString pass1 = VarComponents.MaskStringInput();
                string password = new System.Net.NetworkCredential(string.Empty, pass1).Password;
                while (!VarComponents.IsPassword(password))
                {
                    Console.Clear();
                    Console.WriteLine(ASCIIART.RegistrerenArt());
                    Console.WriteLine("Verkeerd wachtwoord\x0A\x0A\x0AVoer uw wachtwoord in van minimaal 8 tekens waarvan minimaal 1 Hoofdletter, 1 cijfer en 1 speciaal karakter:");
                    SecureString pass2 = VarComponents.MaskStringInput();
                    password = new System.Net.NetworkCredential(string.Empty, pass2).Password;
                }
                /*if (naamInput == "0")
                {
                    Tuple<string, string, string, string> Tuple = null; //Als een persoon toch geen nieuw account wilt returnt de functie gewoon de oude standaard gebruiker account
                    return Tuple;
                }*/

            /*Console.Clear();
            Console.WriteLine(ASCIIART.RegistrerenArt());
            Console.WriteLine("Voer een wachtwoord in van minimaal 8 tekens waarvan minimaal 1 Hoofdletter, 1 cijfer en 1 speciaal karakter:");
            string password = Console.ReadLine();*/
            while (!ValidatePassword(password))
            {
                Console.Clear();
                Console.WriteLine(ASCIIART.RegistrerenArt());
                Console.WriteLine("Voer uw adres in:");
                string adres = Console.ReadLine();
                while (!VarComponents.IsAdres(adres))
                {
                    Console.Clear();
                    Console.WriteLine(ASCIIART.RegistrerenArt());
                    Console.WriteLine("Verkeerd adres\x0A\x0A\x0AVoer uw adres in (straatnaam en huisnummer):");
                    adres = Console.ReadLine();
                }

                Console.Clear();
                Console.WriteLine(ASCIIART.RegistrerenArt());
                Console.WriteLine("Voer uw woonplaats in:");
                string woonplaats = Console.ReadLine();
                while (!VarComponents.IsWoonplaats(woonplaats))
                {
                    Console.Clear();
                    Console.WriteLine(ASCIIART.RegistrerenArt());
                    Console.WriteLine("Verkeerde woonplaats\x0A\x0A\x0Awoonplaats in:");
                    woonplaats = Console.ReadLine();
                }

                Tuple<string, string, string, string> returnTuple = Tuple.Create(naamInput, password, adres, woonplaats);
                return returnTuple;
            }
            return null;
        }

        public Gebruiker registreerKlant(Gebruiker gebruiker)
        {

            Tuple<string, string, string, string> klantInformatie = registreer();

            if(klantInformatie == null)
                return gebruiker;

            var jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            Klant[] nieuweKlantenLijst = new Klant[Klanten.Length + 1];

            for (int i = 0; i < Klanten.Length; i++)
            {
                nieuweKlantenLijst[i] = Klanten[i]; //Voert alle oude klanten als Klant object in de nieuwe lijst
            }
            Klant nieuweKlant = new Klant(klantInformatie.Item1, klantInformatie.Item2, klantInformatie.Item3, klantInformatie.Item4); //nieuwe klant wordt aangemaakt
            nieuweKlantenLijst[nieuweKlantenLijst.Length - 1] = nieuweKlant; //voegt nieuwe klant toe aan lijst
            Klanten = nieuweKlantenLijst; //Klanten array van gebruikers wordt aangepast naar de nieuwe lijst die is gemaakt.
            var toSerializeKlant = JsonSerializer.Serialize(this, jsonOptions);
            File.WriteAllText("gebruikers.json", toSerializeKlant);


            Console.Clear();
            Console.WriteLine(ASCIIART.RegistrerenArt());
            Console.WriteLine($"Welkom nieuwe klant: {klantInformatie.Item1}!\nKlik op een toets om verder te gaan");
            Console.ReadKey();
            return nieuweKlant;
        }
        public void registreerMedewerker() //void omdat alleen een eigenaar een medewerker account aan kan maken, de medewerker wordt dus alleen maar toegevoegd aan de lijst van medewerkers.
        {
            Tuple<string, string, string, string> medewerkerInformatie = registreer();

            if (medewerkerInformatie == null)
                return;


            var jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            Medewerker[] nieuweMedewerkerLijst = new Medewerker[Medewerkers.Length + 1];

            for(int i = 0; i < Medewerkers.Length; i++)
            {
                nieuweMedewerkerLijst[i] = Medewerkers[i]; //Voert alle oude medewerkers als Medewerker object in de nieuwe lijst
            }
            Medewerker nieuweMedewerker = new Medewerker(medewerkerInformatie.Item1, medewerkerInformatie.Item2, medewerkerInformatie.Item3, medewerkerInformatie.Item4); //nieuwe medewerker wordt aangemaakt.
            nieuweMedewerkerLijst[nieuweMedewerkerLijst.Length - 1] = nieuweMedewerker; //voegt nieuwe medewerker toe aan lijst
            Medewerkers = nieuweMedewerkerLijst;
            var toSerializeMedewerker = JsonSerializer.Serialize(this, jsonOptions);
            File.WriteAllText("gebruikers.json", toSerializeMedewerker);
            Console.Clear();
            Console.WriteLine(ASCIIART.RegistrerenArt());
            Console.WriteLine($"Medewerker {medewerkerInformatie.Item1} toegevoegd aan de lijst van medewerkers!\nKlik op een toets om terug te gaan");
            Console.ReadKey();
            return;
        }

        public Gebruiker logOut()
        {
            bool wrongInput = false;
            while (true)
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
        public virtual TheFatDuckRestaurant.ReserveerLijst reserveer(TheFatDuckRestaurant.Menu menu, TheFatDuckRestaurant.ReserveerLijst reserveerLijst)
        {
        return reserveerLijst;
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
            while (true)
            {
                Console.Clear();
                Console.WriteLine(TheFatDuckRestaurant.ASCIIART.GeneralArt());
                Console.WriteLine("STATUS: Uitgelogd\x0a");
                Console.WriteLine("1: Informatie\x0a");
                Console.WriteLine("2: Login\x0a");
                Console.WriteLine("3: Bezichtig het menu\x0a");

                if (verkeerdeInput)
                    Console.WriteLine("Verkeerde input, probeer 1,2,3");

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
        }
    }

    public class Klant : Gebruiker
    {
        public Klant(string naam, string wachtwoord, string adres, string woonplaats) : base(naam, wachtwoord, adres, woonplaats) { }
    public Klant() { }

    public override TheFatDuckRestaurant.Menu bekijkMenu(TheFatDuckRestaurant.Menu menu)
    {
        menu.BekijkMenuGebruiker();
        //menu.Bekijkmenu -> Gebruiker
        //menu.ReserveerMenu -> Klant
        //menu.PasAanMenu -> Medewerker
        return menu;
    }
    public override char startScherm()
        {
            bool verkeerdeInput = false;
            while (true)
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
                    Console.WriteLine("Verkeerde input, probeer 1,2,3,4 of 5");

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
        }

    public override void bekijkAccount()
    {
        bool verkeerdeInput = false;
        while (true)
        {
            Console.Clear();
            Console.WriteLine(TheFatDuckRestaurant.ASCIIART.AccountArt());
            Console.WriteLine($"Account Naam: {Naam}\x0a");
            Console.WriteLine($"Account Type: Klant\x0a");
            Console.WriteLine($"1: Account Reserveringen\x0a");
            Console.WriteLine("0: Return");
            if (verkeerdeInput)
                Console.WriteLine("Verkeerde Input! Probeer 1 of 0");

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
}


    public class Medewerker : Gebruiker
    {
    public Medewerker(string naam, string wachtwoord, string adres, string woonplaats) : base(naam, wachtwoord, adres, woonplaats) { }

    public Medewerker() { }


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
            while (true)
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
                    Console.WriteLine("Verkeerde input, probeer 1,2,3,4,5,6 of 0");

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
                        return 'C';
                    case '5':
                        return '7';
                    case '6':
                        return '6';
                    case '0':
                        return '8';
                    default:
                        verkeerdeInput = true;
                        break;
                }
            }
    }
    public void DailyRevenue(TheFatDuckRestaurant.Reservering[] reserveerlijst, string Datum)
    {
        /*string Datum = null;
        TheFatDuckRestaurant.Reservering X = new TheFatDuckRestaurant.Reservering();
        while (Datum == null)
        {
            Console.Clear();
            Console.WriteLine("Voor welke dag wilt u de opbrengst bekijken? (21 juni 2021)");
            Datum = X.checkDatum(Console.ReadLine(), false);
        }
        Console.Clear();*/
        double Revenue = 0;
        foreach (TheFatDuckRestaurant.Reservering reservering in reserveerlijst)
        {
            if (Datum == reservering.Datum)
            {
                for (int i = 0; i < reservering.Bestelling.Count; i++)
                {
                    Revenue += reservering.Bestelling[i].Prijs * reservering.Bestelling[i].Aantal;
                }
            }
        }
        Console.WriteLine($"De totale opbrengst van {Datum} is {Revenue} euro\n\nEnter: Ga terug naar het startscherm");
        Console.ReadKey();
    }
}

    public class Eigenaar : Medewerker
    {
        public Eigenaar(string naam, string wachtwoord, string adres, string woonplaats) : base(naam, wachtwoord, adres, woonplaats) { }
        public Eigenaar() { }



        public override char startScherm()
        {
            bool verkeerdeInput = false;
            while (true)
            {
                Console.Clear();
                Console.WriteLine(TheFatDuckRestaurant.ASCIIART.GeneralArt());
                Console.WriteLine("STATUS: Ingelogd als eigenaar\x0a");
                Console.WriteLine("1: Informatie\x0a");
                Console.WriteLine("2: Logout\n");
                Console.WriteLine("3: Voeg nieuwe medewerker toe\n");
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
                        return '4';
                    case '3':
                        return 'B';
                    case '4':
                        return '4';
                    case '5':
                        return '5';
                    case '6':
                        return '7';
                    case '0':
                        return '8';
                    default:
                        verkeerdeInput = true;
                        break;
                }
            }
        }
    }