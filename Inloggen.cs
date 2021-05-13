using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using static TheFatDuckRestaurant.MainClass;
using static TheFatDuckRestaurant.Reserveren;
using static TheFatDuckRestaurant.ASCIIART;
using static TheFatDuckRestaurant.Menucode;

namespace TheFatDuckRestaurant
{
    public class Gebruikers
    {
        public Klant[] Klanten { get; set; }
        public Medewerker[] Medewerkers { get; set; }
        public Eigenaar eigenaar { get; set; }

        public Gebruikers() { } //Empty constructor for json deserialisen.
        public Gebruikers(Klant[] klanten, Medewerker[] medewerkers)
        {
            this.Klanten = klanten;
            this.Medewerkers = medewerkers;
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
                Console.WriteLine("3: Registreer een nieuw account\x0a");
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
                        return null;
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
                    return new Gebruiker("", "");
                
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
                    Console.WriteLine(TheFatDuckRestaurant.ASCIIART.LoginArt());
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
                        return new Gebruiker("", "");
                    }
                    }
                }
            return null;
        }
        public Gebruiker logOut()
        {
            bool passed = false;
            bool wrongInput = false;
            while (!passed)
            {
                Console.Clear();
                //ASCIIART
                Console.WriteLine("U bent uitgelogd!\n\n0: Terug");
                if (wrongInput)
                    Console.WriteLine("Verkeerde Input! Probeer 0");
                char userInput = Console.ReadKey().KeyChar;
                if (userInput == '0')
                    return new Gebruiker("", "");
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

        public Gebruiker(string naam, string wachtwoord)
        {
            this.Naam = naam;
            this.Wachtwoord = wachtwoord;
        }
        public Gebruiker() { }

        public virtual TheFatDuckRestaurant.Menu bekijkMenu(TheFatDuckRestaurant.Menu menu)
        {
        //menu.Bekijkmenu -> Gebruiker
        //menu.ReserveerMenu -> Klant
        //menu.PasAanMenu -> Medewerker
        return menu;
        }
        public virtual ReserveerLijst reserveer(TheFatDuckRestaurant.Menu menu, ReserveerLijst reserveerLijst)
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
        

        public virtual char startScherm()
        {
            bool verkeerdeInput = false;
            bool passed = false;
            while (!passed)
            {
                Console.Clear();
                Console.WriteLine(TheFatDuckRestaurant.ASCIIART.GeneralArt());
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
        public Reservering[] Reserveringen { get; set; }

        public Klant(string naam, string wachtwoord, Reservering[] reserveringen) : base(naam, wachtwoord)
        {
            this.Reserveringen = reserveringen;
        }
    public Klant() { }
    public override char startScherm()
        {
            bool verkeerdeInput = false;
            bool passed = false;
            while (!passed)
            {
                Console.Clear();
                Console.WriteLine(TheFatDuckRestaurant.ASCIIART.GeneralArt());
                Console.WriteLine("1: Informatie\x0a");
                Console.WriteLine("2: Logout\n");
                Console.WriteLine("3: Bezichtig het menu\x0a");
                Console.WriteLine("4: Reserveren\x0a");

                if (verkeerdeInput)
                {
                    Console.WriteLine("Verkeerde input, probeer 1,2,3 of 4");
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
                        return '5';
                    default:
                        verkeerdeInput = true;
                        break;
                }
            }
            return '0';
        }
    }

    public class Medewerker : Gebruiker
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
                        return '5';
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

/*
 class Registreren
 {
     public static Dictionary<string, dynamic> Registreerscherm()
     {
         var jsonOptions = new JsonSerializerOptions
         {
             WriteIndented = true,
         };
         var jsonString = File.ReadAllText("gebruikers.json");
         Gebruikers gebruikers = JsonSerializer.Deserialize<Gebruikers>(jsonString);

         Console.Clear();
         Console.WriteLine(ASCIIART.RegistrerenArt());
         Console.WriteLine("Voer uw gebruikers naam in\n0: Terug");
         var naamInput = Console.ReadLine();
         Console.Clear();

         if (naamInput != "0")
         {
             foreach (var gebruiker in gebruikers.Klanten)
             {
                 while (gebruiker.Naam == naamInput)
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

             Klant[] nieuweGebruikerLijst = new Klant[gebruikers.Klanten.Length + 1];

             for(int i = 0; i < gebruikers.Klanten.Length; i++)
             {
                 nieuweGebruikerLijst[i] = gebruikers.Klanten[i]; //Voert alle oude gebruikers als Gebruiker object in de nieuwe lijst
             }
             nieuweGebruikerLijst[nieuweGebruikerLijst.Length - 1] = new Klant(naamInput,  password, null); //voegt nieuwe gebruiker toe aan lijst
             gebruikers.Klanten = nieuweGebruikerLijst; //Klanten array van gebruikers wordt aangepast naar de nieuwe lijst die is gemaakt.
             var toSerializeKlant = JsonSerializer.Serialize(gebruikers, jsonOptions);
             File.WriteAllText("gebruikers.json", toSerializeKlant);


             Console.Clear();
             Console.WriteLine(ASCIIART.RegistrerenArt());
             Console.WriteLine($"Welkom nieuwe gebruiker: {naamInput}!\n0: Enter");
             Console.ReadKey();
             return null;
         }

         else
         {
             return null;
         }
     }
 }
}
*/