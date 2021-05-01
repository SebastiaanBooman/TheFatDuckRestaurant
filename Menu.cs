using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using static TheFatDuckRestaurant.Startscherm;

namespace TheFatDuckRestaurant
{
    public class Menu
    {
        public Gerechten[] Voorgerechten { get; set; }
        public Gerechten[] Hoofdgerechten { get; set; }
        public Gerechten[] Nagerechten { get; set; }
    }

    public class Gerechten
    {
        public string naam { get; set; }
        public double prijs { get; set; }
        public string beschrijving { get; set; }
        public string[] ingredienten { get; set; }
    }



    public class Menucode
    {
        /* static void Main(string[] args)
        {
            Menu menu = instantiateMenu();
            addItemMenu(menu.Voorgerechten);
            //KiesMenu(menu);
        } */

        public static void KiesMenu()
        {
            bool verkeerdeInput = false;
            bool passed = false;
            var menu = instantiateMenu();
            while (!passed)
            {
                Console.Clear();
                Console.WriteLine("Menu bekijken\x0a");
                Console.WriteLine("Voorgerechten\x0AKlik op 1 om de voorgerechten in te zien\x0A\x0A\x0A");
                Console.WriteLine("Hoofdgerechten\x0AKlik op 2 om de hoofdgerechten in te zien\x0A\x0A\x0A");
                Console.WriteLine("Nagerechten\x0AKlik op 3 om de nagerechten in te zien\x0A\x0A");
                Console.WriteLine("Klik op Q om terug naar het startscherm te gaan");
                if (verkeerdeInput)
                {
                    Console.WriteLine("Verkeerde input, probeer 1, 2, 3 of Q");
                }
                ConsoleKeyInfo toetsUser = Console.ReadKey();
                char toetsUserChar = toetsUser.KeyChar;

                switch (toetsUserChar)
                {
                    case '1':
                        ShowMenuGerechten(menu.Voorgerechten, "Voorgerechten", menu);
                        break;
                    case '2':
                        ShowMenuGerechten(menu.Hoofdgerechten, "Hoofdgerechten", menu);
                        break;
                    case '3':
                        ShowMenuGerechten(menu.Nagerechten, "Nagerechten", menu);
                        break;
                    case 'Q':
                        return;
                        //break;
                    default:
                        verkeerdeInput = true;
                        break;
                }
            }
        }
        public static void ShowMenuGerechten(Gerechten[] typeGerecht, string typeGerechtNaam, Menu menu)
        {
            string userInput = null;
            int userInputConverted = 0;
            bool passed = false;
            bool verkeerdeInput = false;
            bool verkeerdeInputRange = false;
            while (!passed) // checkt of de user input wel op het menu staat of Q is, anders vraagt het om een nieuwe input.
            {
                Console.Clear();
                Console.WriteLine($"Dit zijn de {typeGerechtNaam} van The Fat Duck.\x0A\x0A");
                for (int i = 1; i < typeGerecht.Length + 1; i++)
                {

                    Console.WriteLine(typeGerecht[i - 1].naam);
                    Console.WriteLine($"Toets {i} voor meer informatie over dit gerecht \x0A\x0A");
                }
                Console.WriteLine($"Toets Q om terug te gaan \x0A\x0A");
                if (verkeerdeInput)
                {
                    Console.WriteLine("Verkeerde input, probeer Q");
                    verkeerdeInput = false;
                }
                else if (verkeerdeInputRange)
                {
                    Console.WriteLine("Dit gerecht bestaat niet! Probeer een ander gerecht.");
                    verkeerdeInputRange = false;
                }
                try
                {
                    userInput = Console.ReadLine();
                    userInputConverted = Int32.Parse(userInput);
                }
                catch (System.FormatException)
                {
                    if (userInput != "Q")
                    {
                        verkeerdeInput = true;
                    }
                    else
                    {
                        passed = true;
                        KiesMenu();
                    }
                }

                if (userInputConverted > typeGerecht.Length || userInputConverted <= 0)
                {
                    verkeerdeInputRange = true;
                }
                else
                {
                    passed = true;
                    showItem(userInputConverted, typeGerecht, typeGerechtNaam, menu);
                }
            }
        }
        public static void showItem(int x, Gerechten[] typeGerecht, string typeGerechtNaam, Menu menu)
        {
            bool passed = false;
            bool verkeerdeInput = false;
            while (!passed)
            {
                Console.Clear();
                Console.WriteLine($"Gerecht: " + typeGerecht[x - 1].naam + "\x0A\x0A");
                Console.WriteLine($"Prijs: " + typeGerecht[x - 1].prijs + "\x0a");
                Console.WriteLine($"Beschrijving: " + typeGerecht[x - 1].beschrijving + "\x0a");
                Console.WriteLine($"Ingredienten: ");
                for (int i = 0; i < typeGerecht[x - 1].ingredienten.Length; i++)
                {
                    Console.WriteLine(typeGerecht[x - 1].ingredienten[i]);
                }
                Console.WriteLine($"\x0a\x0aToets Q om terug te gaan");
                if (verkeerdeInput)
                {
                    Console.WriteLine("Vekeerde input, probeer Q");
                    verkeerdeInput = false;
                }
                string userInput = Console.ReadLine();
                if (userInput == "Q")
                {
                    passed = true;
                    ShowMenuGerechten(typeGerecht, typeGerechtNaam, menu);
                }
                else
                {
                    verkeerdeInput = true;
                }
            }
        }

        public static Menu addItemMenu(Gerechten[] typeGerecht)
        {
            var JSONoptions = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            var newGerecht = createItemMenu();
            Menu menu = instantiateMenu();

            var newGerechten = new Gerechten[typeGerecht.Length +1];
            for(int i = 0; i < typeGerecht.Length; i++)
            {
                newGerechten[i] = typeGerecht[i];
            }
            newGerechten[typeGerecht.Length] = newGerecht;

            menu.Voorgerechten = newGerechten;
            var jsonString = JsonSerializer.Serialize(menu, JSONoptions);
            File.WriteAllText("menu.json", jsonString);
            menu = instantiateMenu();
            return menu;

        }

        public static void removeItemMenu(Gerechten[] typeGerecht, int removeIndex)
        {

            var JSONoptions = new JsonSerializerOptions
            {
                WriteIndented = true,
            };

            Gerechten[] newGerechten = new Gerechten[typeGerecht.Length - 1];
            int j = 0;
            for(int i = 0; i < typeGerecht.Length; i++)
            {
                if(i != removeIndex-1)
                {
                    newGerechten[j] = typeGerecht[i];
                    j++;
                }
            }
            var menu = instantiateMenu();
            menu.Voorgerechten = newGerechten;
            var jsonString = JsonSerializer.Serialize(menu, JSONoptions);
            File.WriteAllText("menu.json", jsonString);
            menu = instantiateMenu();
        }
        public static Gerechten createItemMenu()
        {
            var passed = false;

            var naam_ = "<Nog geen naam>";
            var prijs_ = 0.0;
            var beschrijving_ = "<Nog geen beschrijving>";
            string[] ingredienten_ = null; ;

            while (!passed)
            {
                var passedSpecifiek = false;
                Console.Clear();
                Console.WriteLine("Voeg een nieuw item toe aan het menu \x0A");
                Console.WriteLine($"Dit is de naam van het nieuwe item: \x0A{naam_} \x0AOm de naam aan te passen toets 1 en klik op enter\xA0");
                Console.WriteLine($"\nDit is de prijs van het nieuwe item: \x0A{prijs_} \x0AOm de prijs aan te passen toets 2 en klik op enter");
                Console.WriteLine($"\nDit is de beschrijving van het nieuwe item: \x0A{beschrijving_} \x0AOm de bescrhijving aan te passen toets 3 en klik op enter");
                Console.WriteLine($"\nDit zijn de ingredienten van het nieuwe item: ");
                if(ingredienten_ != null)
                {
                    for (int i = 0; i < ingredienten_.Length; i++)
                    {
                        Console.WriteLine(ingredienten_[i]);
                    }
                }
                else
                {
                    Console.WriteLine("<Nog geen ingredienten>");
                }
                Console.WriteLine("Om de ingredienten aan te passen toets 4 en klik op enter");
                Console.WriteLine($"\nOm het item te confirmeren en toe te voegen klik op 5\xA0");
                Console.WriteLine($"Om te stoppen met het item te maken klik op 6");
                var userInput = Console.ReadLine();
                switch (userInput)
                {
                    case "1":
                        Console.Clear();
                        Console.WriteLine($"Naam aanpassen\nDit is de oude naam: {naam_}\x0A\x0AToets de nieuwe naam in en klik op enter\x0AOm terug te gaan toets Q in en klik op enter");
                        var userInputNaam = Console.ReadLine();
                        if(userInputNaam == "Q" || userInputNaam == "q") {}
                        else
                        {
                            naam_ = userInputNaam;
                        }
                        break;
                    case "2":
                        Console.Clear();
                        while(!passedSpecifiek){
                            Console.WriteLine($"Prijs aanpassen\x0A Dit is de huidige prijs: {prijs_}\x0AToets de nieuwe prijs in en klik op enter\x0A Om terug te gaan toets Q in en klik op enter");
                            var userInputPrijs = Console.ReadLine();
                            if (userInputPrijs == "Q" || userInputPrijs == "q") 
                            {
                                passedSpecifiek = true;
                            }
                            else
                            {

                                try
                                {
                                    var userInputPrijsConverted = Int32.Parse(userInputPrijs);

                                    prijs_ = userInputPrijsConverted;
                                    passedSpecifiek = true;
                                }
                                catch (System.FormatException)
                                {
                                    try
                                    {
                                        var userInputPrijsConverted = float.Parse(userInputPrijs);
                                        prijs_ = userInputPrijsConverted;
                                        passedSpecifiek = true;
                                    }
                                    catch (System.FormatException)
                                    {
                                        Console.WriteLine("Verkeerde input, voer een int in.");
                                    }
                                    
                                }
                            }
                        }
                        passedSpecifiek = true;
                        break;
                    case "3":
                        Console.Clear();
                        Console.WriteLine($"Beschrijving aanpassen\x0A Dit is de huidige beschrijving: {beschrijving_}\x0AToets de nieuwe beschrijving in en klik op enter\x0A Om terug te gaan toets Q in en klik op enter.");
                        var userInputBeschrijving = Console.ReadLine();
                        if(userInputBeschrijving == "Q" || userInputBeschrijving == "q") {}
                        else
                        {
                            beschrijving_ = userInputBeschrijving;
                        }
                        break;
                    case "4":
                        Console.Clear();
                        while (!passedSpecifiek)
                        {
                            Console.Clear();
                            Console.WriteLine($"Ingredienten aanpassen\x0A Dit zijn de huidige ingredienten:");
                            if(ingredienten_ != null)
                            {
                                for(int i = 0; i < ingredienten_.Length; i++)
                                {
                                    Console.WriteLine(ingredienten_[i]);
                                }
                            }
                            else
                            {
                                Console.WriteLine("<Nog geen ingredienten.>");
                            }
                            Console.WriteLine("\x0AToets een nieuw ingredient in en klik op enter\x0A Om terug te gaan toets Q en klik op enter.");
                            var userInputIngredienten = Console.ReadLine();
                            if (userInputIngredienten == "Q" || userInputIngredienten == "q") 
                            {
                                passedSpecifiek = true;
                            }
                            else
                            {
                                if(ingredienten_ != null)
                                {
                                    var tempIngredienten = new string[ingredienten_.Length +1];
                                    for(int i = 0; i < ingredienten_.Length; i++)
                                    {
                                        tempIngredienten[i] = ingredienten_[i];
                                    }
                                    tempIngredienten[tempIngredienten.Length - 1] = userInputIngredienten;
                                    ingredienten_ = tempIngredienten;
                                }
                                else
                                {
                                    ingredienten_ = new string[1];
                                    ingredienten_[0] = userInputIngredienten;
                                }
                            }
                        }
                        break;
                    case "5":
                        Console.Clear();
                        Console.WriteLine("Het item wordt toegevoegd aan het voorgerechten menu, weet u dit zeker? \x0AToets A in en klik op enter om het menu toe te voegen. \x0AToets Q in en klik op enter om het te annuleren.");
                        var userInputConfirmatie = Console.ReadLine();
                        if(userInputConfirmatie == "A" || userInputConfirmatie == "a")
                        {
                            passed = true;
                        }
                        break;
                    case "6":
                        Console.Clear();
                        Console.WriteLine("U gaat terug naar het algemene menu en er worden verder geen gerechten toegevoegd aan het menu, weet u dit zeker? \x0AToets A in en klik op enter om terug te gaan\x0A Toets Q in en klik op enter om verder te werken aan een menu item toevoegen. ");
                        var userInputFinale = Console.ReadLine();
                        if(userInputFinale == "A" || userInputFinale == "a")
                        {
                            //Call naar menu bekijken voor Werknemer scherm.
                        }
                        break;
                }
            }

            var newGerecht = new Gerechten
            {
                naam = naam_,
                prijs = prijs_,
                beschrijving = beschrijving_,
                ingredienten = ingredienten_
            };

            return newGerecht;
        }

        public static Menu instantiateMenu()
        {
            var jsonString = File.ReadAllText("menu.json");
            Menu menu = JsonSerializer.Deserialize<Menu>(jsonString);
            return menu;
        }
    }
}
