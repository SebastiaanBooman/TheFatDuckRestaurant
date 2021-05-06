using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using static TheFatDuckRestaurant.Startscherm;
using static TheFatDuckRestaurant.ASCIIART;

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
                Console.WriteLine(ASCIIART.MenuArt());
                Console.WriteLine("KIES HET TYPE GERECHT:\x0A\x0A");
                Console.WriteLine("1: Voorgerechten\x0A");
                Console.WriteLine("2: Hoofdgerechten\x0A");
                Console.WriteLine("3: Nagerechten\x0A\x0a");
                Console.WriteLine("0: Terug");
                if (verkeerdeInput)
                {
                    Console.WriteLine("Verkeerde input, probeer 1, 2, 3 of 0");
                }
                ConsoleKeyInfo toetsUser = Console.ReadKey();
                char toetsUserChar = toetsUser.KeyChar;

                switch (toetsUserChar)
                {
                    case '1':
                        MenuGerechten(menu.Voorgerechten, "Voorgerechten", menu);
                        break;
                    case '2':
                        MenuGerechten(menu.Hoofdgerechten, "Hoofdgerechten", menu);
                        break;
                    case '3':
                        MenuGerechten(menu.Nagerechten, "Nagerechten", menu);
                        break;
                    case '0':
                        return;
                    //break;
                    default:
                        verkeerdeInput = true;
                        break;
                }
            }
        }
        public static void MenuGerechten(Gerechten[] typeGerecht, string typeGerechtNaam, Menu menu)
        {
            string userInput = null;
            int userInputConverted = 0;
            bool passed = false;
            bool verkeerdeInput = false;
            bool verkeerdeInputRange = false;
            while (!passed) // checkt of de user input wel op het menu staat of Q is, anders vraagt het om een nieuwe input.
            {
                Console.Clear();
                Console.WriteLine(ASCIIART.MenuArt());
                Console.WriteLine($"\x0A Dit zijn de {typeGerechtNaam} van The Fat Duck.\x0A Toets op het getal naast het menu item om er meer informatie over te lezen\x0A\x0A");
                for (int i = 1; i < typeGerecht.Length + 1; i++)
                {

                    Console.WriteLine(i + ": " + typeGerecht[i - 1].naam + "\x0A");
                    // Console.WriteLine($"Toets {i} voor meer informatie over dit gerecht \x0A\x0A");
                }
                Console.WriteLine($"\x0AToets A om het menu aan te passen\x0AToets Q om terug te gaan");
                if (verkeerdeInput)
                {
                    Console.WriteLine("Verkeerde input, probeer Q of A");
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
                    if (userInput == "Q")
                    {
                        passed = true;
                        return;
                    }
                    else if(userInput == "A")
                    {
                        typeGerecht = changeMenu(typeGerechtNaam, typeGerecht);
                     
                    }
                    else
                    {
                        verkeerdeInput = true;
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
                Console.WriteLine(ASCIIART.MenuArt());
                Console.WriteLine($"Gerecht: " + typeGerecht[x - 1].naam + "\x0A");
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
                    return;
                }
                else
                {
                    verkeerdeInput = true;
                }
            }
        }

        public static Gerechten[] changeMenu(string typeGerechtNaam, Gerechten[] typeGerecht)
        {
            bool passed = false;
            while(passed != true)
            {
                Console.Clear();
                Console.WriteLine(ASCIIART.MenuArt());
                Console.WriteLine(typeGerechtNaam + " menu aanpassen\x0a\x0a 1: Item toevoegen\x0a\x0a 2: Item verwijderen\x0a\x0aToets Q om terug te gaan");
                ConsoleKeyInfo toetsUser = Console.ReadKey();
                char toetsUserChar = toetsUser.KeyChar;

                switch(toetsUserChar)
                {
                    case '1':
                        typeGerecht = addItemMenu(typeGerecht, typeGerechtNaam);
                        break;
                    case '2':
                        typeGerecht = removeItemScreen(typeGerecht, typeGerechtNaam);
                        return typeGerecht;
                    case 'Q':
                        return typeGerecht;
                }
            }
            return null;

        }

        public static Gerechten[] addItemMenu(Gerechten[] typeGerecht, string typeGerechtNaam)
        {
            var JSONoptions = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            var newGerecht = createItemMenu(typeGerechtNaam);
            Menu menu = instantiateMenu();


            if(newGerecht != null)
            {
                var newGerechten = new Gerechten[typeGerecht.Length + 1];
                for (int i = 0; i < typeGerecht.Length; i++)
                {
                    newGerechten[i] = typeGerecht[i];
                }
                newGerechten[typeGerecht.Length] = newGerecht;

                if (typeGerechtNaam == "Voorgerechten")
                    menu.Voorgerechten = newGerechten;
                if (typeGerechtNaam == "Hoofdgerechten")
                    menu.Hoofdgerechten = newGerechten;
                if (typeGerechtNaam == "Nagerechten")
                    menu.Nagerechten = newGerechten;
                typeGerecht = newGerechten;
            }

            var jsonString = JsonSerializer.Serialize(menu, JSONoptions);
            File.WriteAllText("menu.json", jsonString);
            return typeGerecht;

        }

        public static Gerechten[] removeItemScreen(Gerechten[] typeGerecht, string typeGerechtNaam)
        {
            bool passed = false;
            while (passed != true)
            {
                Console.Clear();
                Console.WriteLine(ASCIIART.MenuArt());
                Console.WriteLine("Item uit " + typeGerechtNaam + " menu verwijderen\x0aToets op het getal naast het menu item wat u weg wil en enter om het te verwijderen\x0a");
                if (typeGerecht.Length > 0)
                {
                    for (int i = 1; i < typeGerecht.Length + 1; i++)
                    {
                        Console.WriteLine(i + ": " + typeGerecht[i - 1].naam + "\x0A");
                    }

                }
                else
                    Console.WriteLine("<Er bestaan nog geen " + typeGerechtNaam + " in het menu>");

                Console.WriteLine("\x0AToets Q en enter om terug te gaan");
                var userInput = Console.ReadLine();
                try
                {
                    var userInputConverted = Int32.Parse(userInput);
                    typeGerecht = removeItemMenu(typeGerecht,typeGerechtNaam, userInputConverted);
                    return typeGerecht;
                }
                catch
                {
                    if(userInput == "Q")
                        return typeGerecht;
                }

            }
            return null;
        }
        public static Gerechten[] removeItemMenu(Gerechten[] typeGerecht,string typeGerechtNaam, int removeIndex)
        {

            var JSONoptions = new JsonSerializerOptions
            {
                WriteIndented = true,
            };

            Gerechten[] newGerechten = new Gerechten[typeGerecht.Length - 1];
            int j = 0;
            for (int i = 0; i < typeGerecht.Length; i++)
            {
                if (i != removeIndex - 1)
                {
                    newGerechten[j] = typeGerecht[i];
                    j++;
                }
            }
            var menu = instantiateMenu();
            if (typeGerechtNaam == "Voorgerechten")
                menu.Voorgerechten = newGerechten;
            if (typeGerechtNaam == "Hoofdgerechten")
                menu.Hoofdgerechten = newGerechten;
            if (typeGerechtNaam == "Nagerechten")
                menu.Nagerechten = newGerechten;

            var jsonString = JsonSerializer.Serialize(menu, JSONoptions);
            File.WriteAllText("menu.json", jsonString);
            menu = instantiateMenu();
            return newGerechten;
        }

        public static Gerechten createItemMenu(string typeGerechtNaam)
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
                Console.WriteLine(ASCIIART.MenuArt());
                Console.WriteLine("Voeg een nieuw " + typeGerechtNaam + " item toe aan het menu \x0A");
                Console.WriteLine($"1: Naam \t\t{naam_}");
                Console.WriteLine($"2: Prijs \t\t{prijs_}");
                Console.WriteLine($"3: Beschrijving \t{beschrijving_}");
                Console.WriteLine("4: Ingredienten");
                if (ingredienten_ != null)
                {
                    Console.WriteLine($"\nDit zijn de ingredienten van het nieuwe item: ");
                    for (int i = 0; i < ingredienten_.Length; i++)
                    {
                        Console.WriteLine(ingredienten_[i]);
                    }
                }
                else
                {
                    Console.WriteLine("\t<Nog geen ingredienten>");
                }
                Console.WriteLine($"\n5: Item opslaan\xA0");
                Console.WriteLine($"Toets Q in om terug te gaan");
                var userInput = Console.ReadLine();
                switch (userInput)
                {
                    case "1":
                        Console.Clear();
                        Console.WriteLine($"Naam aanpassen\nDit is de oude naam: {naam_}\x0A\x0AToets de nieuwe naam in en klik op enter\x0AOm terug te gaan toets Q in en klik op enter");
                        var userInputNaam = Console.ReadLine();
                        if (userInputNaam == "Q" || userInputNaam == "q") { }
                        else
                        {
                            naam_ = userInputNaam;
                        }
                        break;
                    case "2":
                        Console.Clear();
                        while (!passedSpecifiek)
                        {
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
                        if (userInputBeschrijving == "Q" || userInputBeschrijving == "q") { }
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
                            if (ingredienten_ != null)
                            {
                                for (int i = 0; i < ingredienten_.Length; i++)
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
                                if (ingredienten_ != null)
                                {
                                    var tempIngredienten = new string[ingredienten_.Length + 1];
                                    for (int i = 0; i < ingredienten_.Length; i++)
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
                        if (userInputConfirmatie == "A" || userInputConfirmatie == "a")
                        {
                            passed = true;
                        }
                        break;
                    case "Q":
                        Console.Clear();
                        Console.WriteLine("U gaat terug naar het algemene menu en er worden verder geen gerechten toegevoegd aan het menu, weet u dit zeker? \x0AToets Q in en klik op enter om terug te gaan\x0A Toets A in en klik op enter om verder te werken aan een menu item toevoegen. ");
                        var userInputFinale = Console.ReadLine();
                        if (userInputFinale == "Q" || userInputFinale == "q")
                        {
                            return null;
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