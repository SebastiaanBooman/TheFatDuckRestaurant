using System;
using System.Collections.Generic;
using System.Text;

namespace TheFatDuckRestaurant
{
    interface IGebruiker
    {
        string Naam { get; set; }
        string Wachtwoord { get; set; }
        string Adres { get; set; }
        string Woonplaats { get; set; }
        TheFatDuckRestaurant.Menu bekijkMenu(TheFatDuckRestaurant.Menu menu);
        char startScherm();
    }
    public class Gebruiker : IGebruiker
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
            return menu;
        }
        public virtual TheFatDuckRestaurant.ReserveerLijst reserveer(TheFatDuckRestaurant.Menu menu, TheFatDuckRestaurant.ReserveerLijst reserveerLijst) => reserveerLijst;

        /*public virtual string bekijkDailyRevenue() => null;

        public virtual string bekijkClickStream() => null;

        public virtual void bekijkReserveringen() { }*/

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
    }

    public class Eigenaar : Medewerker
    {
        //public Eigenaar(string naam, string wachtwoord, string adres, string woonplaats) : base(naam, wachtwoord, adres, woonplaats) { }
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
                Console.WriteLine("7: Bezichtig de dagelijkse opbrensten\n");
                Console.WriteLine("0: Applicatie afsluiten\x0a");

                if (verkeerdeInput)
                    Console.WriteLine("Verkeerde input, probeer 1, 2, 3, 4, 5, 6, 7, of 0");

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
                        return 'C';
                    case '6':
                        return '7';
                    case '7':
                        return '6';
                    case '0':
                        return '8';
                    default:
                        verkeerdeInput = true;
                        break;
                }
            }
        }
    }
}
