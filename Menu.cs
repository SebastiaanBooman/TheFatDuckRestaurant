using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
//using static TheFatDuckRestaurant.Startscherm;
using static TheFatDuckRestaurant.ASCIIART;
using System.Linq;

namespace TheFatDuckRestaurant
{
    public class Menu
    {
        public Gerechten[] Voorgerechten { get; set; }
        public Gerechten[] Hoofdgerechten { get; set; }
        public Gerechten[] Nagerechten { get; set; }

        public void kiesMenuOpties(string typeGebruiker) //Print de lijst met typen Gerechten
        {
            Console.Clear();
            Console.WriteLine(ASCIIART.MenuArt());
            if (typeGebruiker == "Klant")
                Console.WriteLine("Kies het type gerecht wat u wilt toevoegen aan uw reservering:\x0A\x0A");
            else
                Console.WriteLine("KIES HET TYPE GERECHT:\x0A\x0A");
            Console.WriteLine("1: Voorgerechten\x0A");
            Console.WriteLine("2: Hoofdgerechten\x0A");
            Console.WriteLine("3: Nagerechten\x0A\x0a");
            Console.WriteLine("0: Terug");
        }

        public Bestelling laadSpecifiekMenu(Gerechten[] typeGerecht, string typeGebruiker) //Laad de gerechten van het type menu (Voorgerechten, Hoofdgerechten, Nagerechten etc).
        {
            int huidigePaginaNR = 0; //Bij de eerste keer laden van het menu zal de eerste 7 gerechten worden getoont.
            while (true)
            {
                Console.Clear();
                Console.WriteLine(ASCIIART.MenuArt());
                if (typeGerecht == Voorgerechten)
                    Console.WriteLine($"\nVoorgerechten\n");
                else if (typeGerecht == Hoofdgerechten)
                    Console.WriteLine($"\nHoofdgerechten\n");
                else
                    Console.WriteLine($"\nNagerechten\n");

                int hoeveelheidPaginas = (int)Math.Ceiling(typeGerecht.Length / 7.0); //Berekent hoeveel pagina's van 7 gerechten er moeten zijn om alle gerechten te kunnen tonen (Bijv. 25 / 7 = 4 paginas)
                Console.WriteLine($"Pagina {huidigePaginaNR + 1}/{hoeveelheidPaginas}\n");
                for (int i = 1; i < 8; i++) //Laat 7 gerechten zien per slide
                {
                    try
                    {
                        Console.WriteLine(i + ": " + typeGerecht[(i - 1) + (7 * huidigePaginaNR)].naam + "\x0A"); //1 : "GerechtNaam" (etc), laat alleen de gerechten zien voor de juiste pagina nr
                    }
                    catch (IndexOutOfRangeException) //Omdat de gerechten per 7 worden laten zien, zal er bij 5 gerechten op een pagina een error komen omdat het programma niet bestaande gerechten 6 en 7 ook probeert te tonen.
                    {

                    }
                }
                if (huidigePaginaNR + 1 < hoeveelheidPaginas)
                    Console.WriteLine("8: Volgende pagina");
                if (huidigePaginaNR + 1 >= hoeveelheidPaginas && (hoeveelheidPaginas > 1))
                    Console.WriteLine("9: Vorige pagina");


                if (typeGebruiker == "Medewerker") //Als de gebruiker een medewerker is, dan moet er een extra print komen met de optie om het menu aan te passen
                    Console.WriteLine("A: Menu aanpassen");
                Console.WriteLine("0: Terug");

                ConsoleKeyInfo toetsUser = Console.ReadKey();
                char toetsUserChar = toetsUser.KeyChar;

                if (typeGebruiker == "Medewerker" && (toetsUserChar == 'A' || toetsUserChar == 'a')) //Indien de gebruiker die het menu opent een Medewerker is, moet hij het menu kunnen aanpassen
                    typeGerecht = MenuAanpassenScherm(typeGerecht);

                else if (huidigePaginaNR + 1 < hoeveelheidPaginas && toetsUserChar == '8')
                    huidigePaginaNR++;

                else if (huidigePaginaNR + 1 >= hoeveelheidPaginas && (hoeveelheidPaginas > 1) && toetsUserChar == '9')
                    huidigePaginaNR--;

                else if (toetsUserChar == '0')
                    return null;
                else if (typeGebruiker == "Klant") //Indien er als klant is ingelogd, moet er bij het klikken op een gerecht de optie worden geprint om het item toe te kunnen voegen aan een reservering
                    return ShowItemReserveringHandler(typeGerecht, toetsUserChar, huidigePaginaNR);
                else
                    ShowItemHandler(typeGerecht, toetsUserChar, huidigePaginaNR);
            }
        }

        public Bestelling BekijkSpecifiekMenu(string typeGebruiker) // Called laadSpecifiekMenu, met de type gebruiker die het menu wil bekijken.
        {
            bool verkeerdeInput = false;

            while (true)
            {
                Console.Clear();
                kiesMenuOpties(typeGebruiker);
                if (verkeerdeInput)
                    Console.WriteLine("Verkeerde input, probeer 1, 2, 3 of 0");

                ConsoleKeyInfo toetsUser = Console.ReadKey();
                char toetsUserChar = toetsUser.KeyChar;
                switch (toetsUserChar)
                {
                    case '1':
                        var specifiekMenuVoorgerechten = laadSpecifiekMenu(Voorgerechten, typeGebruiker);
                        if (specifiekMenuVoorgerechten != null)
                            return specifiekMenuVoorgerechten;
                        else
                            break;

                    case '2':
                        var specifiekMenuHoofdgerechten = laadSpecifiekMenu(Hoofdgerechten, typeGebruiker);
                        if (specifiekMenuHoofdgerechten != null)
                            return specifiekMenuHoofdgerechten;
                        else
                            break;
                    case '3':
                        var specifiekMenuNagerechten = laadSpecifiekMenu(Nagerechten, typeGebruiker);
                        if (specifiekMenuNagerechten != null)
                            return specifiekMenuNagerechten;
                        else
                            break;

                    case '0':
                        return null;
                    default:
                        verkeerdeInput = true;
                        break;
                }
            }
        }

        public void BekijkMenuMedewerker() //Opent het menu met de opties voor een medewerker
        {
            while (true)
            {
                Console.Clear();
                BekijkSpecifiekMenu("Medewerker");
                return;
            }
        }

        public void BekijkMenuGebruiker() //Opent het menu met de opties voor een Gebruiker (niet ingelogd) of klant die niet wil reserveren.
        {
            while (true)
            {
                Console.Clear();
                BekijkSpecifiekMenu("Gebruiker");
                return;

            }
        }

        public List<Bestelling> BekijkMenuKlant(List<Bestelling> bestelling = null) //Opent het menu met de opties voor een klant die een item wil toevoegen aan hun reservering
        {
            while (true)
            {
                Console.Clear();
                var bekijkMenu = BekijkSpecifiekMenu("Klant"); //Opent het menu en returned een lijst met Bestellingen die zijn gemaakt bij het openen van het menu.
                if (bekijkMenu != null)
                {
                    if (bestelling == null)
                        bestelling = new List<Bestelling>();

                    bool addNewOne = true;
                    foreach (var bestellingItem in bestelling)
                    {
                        if (bestellingItem.Naam == bekijkMenu.Naam)
                        {
                            bestellingItem.Aantal += bekijkMenu.Aantal;
                            addNewOne = false;
                        }
                    }

                    if (addNewOne)
                        bestelling.Add(bekijkMenu);

                    Console.Clear();
                    Console.WriteLine(ReserverenArt());
                    Console.WriteLine($"{bekijkMenu.Naam} ({bekijkMenu.Aantal}x) is succesvol toegevoegd aan uw reservering!\nWil u doorgaan met het menu bekijken/Items toevoegen?\n\n1: Doorgaan\n0: Terug naar reservering");
                    ConsoleKeyInfo toetsUser = Console.ReadKey();
                    char toetsUserChar = toetsUser.KeyChar;

                    if (toetsUserChar != '1')
                        return bestelling;
                }
                else
                    return bestelling;
            }
        }

        public void ShowItemHandler(Gerechten[] geladenMenu, char toetsUserChar, int paginaNR) //Toont informatie over een specifiek gerecht uit het menu
        {
            try
            {
                ShowItemStandaard(geladenMenu[(Int32.Parse(toetsUserChar.ToString()) - 1) + (7 * paginaNR)]); //7 * paginaNR zorgt ervoor dat het juiste item wordt laten zien
                Console.WriteLine($"\n\n0 : Terug");
                Console.ReadKey();
            }
            catch { return; }
        }

        public Bestelling ShowItemReserveringHandler(Gerechten[] geladenMenu, char toetsUserChar, int paginaNR) //Toont informatie over specifiek gerecht en geeft optie om het item toe te voegen aan je reservering
        {
            try
            {
                bool specifiekPassed = false;
                while (true)
                {
                    ShowItemStandaard(geladenMenu[(Int32.Parse(toetsUserChar.ToString()) - 1) + (7 * paginaNR)]); //7 * paginaNR zorgt ervoor dat het juiste item wordt laten zien
                    Console.WriteLine("\nR: Toevoegen aan reservering");
                    Console.WriteLine($"0 : Terug");
                    ConsoleKeyInfo toetsUsr = Console.ReadKey();
                    char toetsUsrChar = toetsUsr.KeyChar;

                    if (toetsUsrChar == 'R' || toetsUsrChar == 'r') //Indien er op R wordt geklikt wordt het item toegevoegd aan de lijst met Bestellingen.
                        while (specifiekPassed != true)
                        {
                            Console.Clear();
                            Console.WriteLine(ASCIIART.ReserverenArt()); //Vraagt hoeveel je van het item wil toevoegen aan je reservering, indien je 0 klikt ga je terug en voeg je het niet toe.
                            Console.WriteLine("Toets hoeveel u van " + geladenMenu[(Int32.Parse(toetsUserChar.ToString()) - 1) + (7 * paginaNR)].naam + " wil bestellen en enter om dit toe te voegen aan uw reservering\n\n0: Terug");
                            var amountStr = Console.ReadLine();
                            if (amountStr == "0")
                                break;
                            try
                            {
                                int amount = Int32.Parse(amountStr); //Als er wel gereserveerd is wordt het item aan de lijst met Bestellingen toegevoegd.
                                return new Bestelling(geladenMenu[(Int32.Parse(toetsUserChar.ToString()) - 1) + (7 * paginaNR)].naam, geladenMenu[(Int32.Parse(toetsUserChar.ToString()) - 1) + (7 * paginaNR)].prijs, amount);
                            }
                            catch { };
                        }
                    if (toetsUsrChar == '0')
                        return null;
                }
            }
            catch { return null; }
        }

        public void ShowItemStandaard(Gerechten gerecht) //Print informatie over een specifiek gerecht uit het menu
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            string totaalprijs = "" + gerecht.prijs;
            totaalprijs += (!totaalprijs.Contains(',') ? ",-" : totaalprijs[totaalprijs.Length - 2] == ',' ? "0" : "");
            Console.Clear();
            Console.WriteLine(ASCIIART.MenuArt());
            Console.WriteLine($"Gerecht: " + gerecht.naam + "\x0A");
            Console.Out.WriteLine($"Prijs: €{totaalprijs}\x0a");
            Console.WriteLine($"Beschrijving: " + gerecht.beschrijving + "\x0a");
            Console.WriteLine($"Ingredienten: ");
            for (int i = 0; i < gerecht.ingredienten.Length; i++)
                Console.WriteLine($"- {gerecht.ingredienten[i]}");
        }

        public Gerechten[] MenuAanpassenScherm(Gerechten[] typeGerecht) //Handelt het toevoegen van een item aan een specifiek menu.
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine(ASCIIART.MenuArt());
                if (typeGerecht == Voorgerechten)
                    Console.WriteLine("Voorgerechten menu aanpassen\n\n1: Item toevoegen\n\n2: Item verwijderen\n\n0: Terug");
                else if (typeGerecht == Hoofdgerechten)
                    Console.WriteLine("Hoofdgerechten menu aanpassen\n\n1: Item toevoegen\n\n2: Item verwijderen\n\n0: Terug");
                else
                    Console.WriteLine("Nagerechten menu aanpassen\n\n1: Item toevoegen\n\n2: Item verwijderen\n\n0: Terug");
                ConsoleKeyInfo toetsUser = Console.ReadKey();
                char toetsUserChar = toetsUser.KeyChar;
                //Afhankelijk van het menu wat je wil aanpassen, zal de AddItemHandler worden gecalled.
                switch (toetsUserChar)
                {
                    case '1':
                        if (typeGerecht == Voorgerechten)
                        {
                            AddItemHandler("Voorgerechten");
                            typeGerecht = Voorgerechten;
                        }
                        else if (typeGerecht == Hoofdgerechten)
                        {
                            AddItemHandler("Hoofdgerechten");
                            typeGerecht = Hoofdgerechten;
                        }
                        else
                        {
                            AddItemHandler("Nagerechten");
                            typeGerecht = Nagerechten;
                        }
                        break;
                    case '2': //Indien je een item wil verwijderen, zal het correcte menu worden geladen met removeItemScreen.
                        if (typeGerecht == Voorgerechten)
                            typeGerecht = removeItemScreen(typeGerecht, "Voorgerechten");
                        else if (typeGerecht == Hoofdgerechten)
                            typeGerecht = removeItemScreen(typeGerecht, "Hoofdgerechten");
                        else
                            typeGerecht = removeItemScreen(typeGerecht, "Nagerechten");
                        break;
                    case '0':
                        if (typeGerecht == Voorgerechten)
                            return Voorgerechten;
                        else if (typeGerecht == Hoofdgerechten)
                            return Hoofdgerechten;
                        else
                            return Nagerechten;
                }
            }
        }

        public Gerechten AddItemScherm(string typeGerechtNaam) //Print de opties om een item toe te voegen, en handeld correcte inputs.
        {
            var nietAllesIngevuld = false;
            var naam_ = "<Nog geen naam>";
            var prijs_ = 0.0;
            var beschrijving_ = "<Nog geen beschrijving>";
            List<string> ingredienten_ = new List<string>();

            while (true)
            {
                var passedSpecifiek = false;
                Console.Clear();
                Console.WriteLine(ASCIIART.MenuArt());
                Console.WriteLine("Voeg een nieuw " + typeGerechtNaam + " item toe aan het menu \x0A");
                Console.WriteLine($"1: Naam \t\t{naam_}");
                Console.WriteLine($"2: Prijs \t\t{prijs_}");
                Console.WriteLine($"3: Beschrijving \t{beschrijving_}");
                Console.WriteLine("4: Ingredienten:");
                if (ingredienten_.Count != 0)
                {
                    for (int i = 0; i < ingredienten_.Count; i++)
                        Console.WriteLine($"- {ingredienten_[i]}");
                }
                else
                {
                    Console.WriteLine("<Nog geen ingredienten>");

                    Console.WriteLine($"\n5: Item opslaan\xA0");
                    Console.WriteLine($"0: Terug");
                    if (nietAllesIngevuld)
                    {
                        Console.WriteLine("\nERROR: Vul eerst alle velden in voordat u het item opslaat!");
                        nietAllesIngevuld = false;
                    }
                    ConsoleKeyInfo toetsUser = Console.ReadKey();
                    char toetsUserChar = toetsUser.KeyChar;
                    switch (toetsUserChar)
                    {
                        case '1':
                            naam_ = ChangeItemName(naam_); //Input "1" roept de functie die een gerecht's naam laat veranderen.
                            break;
                        case '2':
                            prijs_ = ChangeItemPrice(prijs_); //Input "2" roept de functie die een gerecht's prijs laat veranderen.
                            break;
                        case '3':
                            beschrijving_ = ChangeItemDescription(beschrijving_); //Input "3" roept de functie die een gerecht's beschrijving laat veranderen.
                            break;
                        case '4': //TODO: Ingredienten aanpassen moet ook in een aparte functie komen. Momenteel moeilijk te implementeren omdat de ingredienten niet een field of property van een class is en je kan het dus moeilijk callen.
                            while (!passedSpecifiek) //Dit gedeelte handelt het toevoegen van ingredienten aan het nieuwe gerecht
                            {
                                Console.Clear();
                                Console.WriteLine(ASCIIART.MenuArt());
                                Console.WriteLine($"Ingredienten aanpassen:");
                                if (ingredienten_.Count != 0)
                                {
                                    Console.WriteLine("\nAls u een ingredient wil verwijderen toets dan de index in die ernaast staat\nDit zijn de huidige ingredienten:");
                                    for (int i = 0; i < ingredienten_.Count; i++)
                                    {
                                        Console.WriteLine($"{i + 1} - {ingredienten_[i]}");
                                    }
                                }
                                else
                                    Console.WriteLine("\n<Nog geen ingredienten>");
                                Console.WriteLine("\nToets een nieuw ingredient in en klik op enter\n\n0: Terug");
                                var userInputIngredienten = Console.ReadLine();

                                try
                                {
                                    var removeIndex = Int32.Parse(userInputIngredienten) - 1;
                                    ingredienten_.RemoveAt(removeIndex);
                                }
                                catch
                                {
                                    int tempInt;
                                    if (userInputIngredienten == "0")
                                        passedSpecifiek = true;
                                    else if (userInputIngredienten != "" && int.TryParse(userInputIngredienten, out tempInt) != true)
                                        ingredienten_.Add(userInputIngredienten);

                                }
                            }
                            break;
                        case '5': //Indien item opgeslagen moet worden, wordt er gecheckt of daadwerkelijk alle velden zijn ingevuld, zodat er geen errors in het JSON bestand ontstaan.
                            if (ingredienten_.Count != 0 || naam_ == "<Nog geen naam>" || beschrijving_ == "<Nog geen beschrijving>")
                            {
                                nietAllesIngevuld = true;
                                break;
                            }

                            Console.Clear();
                            Console.WriteLine(ASCIIART.MenuArt());
                            Console.WriteLine($"Het item wordt toegevoegd aan het {typeGerechtNaam} menu, weet u dit zeker?\n\nA: Item bevestigen en toevoegen aan het menu\n0: Item annuleren");
                            ConsoleKeyInfo userInputConfirmatie = Console.ReadKey();
                            char userInputConfirmatieChar = userInputConfirmatie.KeyChar;
                            if (userInputConfirmatieChar == 'A' || userInputConfirmatieChar == 'a') //Vraagt nog een keer bevestiging of het item daadwerkelijk moet worden toegevoegd aan het menu
                            {
                                Console.Clear();
                                Console.WriteLine(ASCIIART.MenuArt());
                                Console.WriteLine($"{naam_} is succesvol toegevoegd aan de {typeGerechtNaam}!\n\n0: Terug");
                                Console.ReadKey();
                                return new Gerechten(naam_, prijs_, beschrijving_, ingredienten_.ToArray());
                            }
                            break;
                        case '0':
                            Console.Clear();
                            Console.WriteLine(ASCIIART.MenuArt());
                            Console.WriteLine("U gaat terug naar het algemene menu en er worden verder geen gerechten toegevoegd aan het menu, weet u dit zeker?\n\nA: Verder werken aan menu item\n0: Item schrappen en terug gaan");
                            ConsoleKeyInfo userInputFinale = Console.ReadKey();
                            char userInputFinaleChar = userInputFinale.KeyChar;
                            if (userInputFinaleChar == '0')
                                return null;
                            break;
                    }
                }
            }
        }

        public void AddItemHandler(string typeGerechtNaam)
        {
            var JSONoptions = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            var newGerecht = AddItemScherm(typeGerechtNaam);

            if (newGerecht != null)
            {
                //Afhankelijk van het menu wat de mederwerker op heeft staan, wordt het juiste menu aangepast van het object Menu
                if (typeGerechtNaam == "Voorgerechten")
                {
                    var newGerechten = new Gerechten[Voorgerechten.Length + 1];
                    for (int i = 0; i < Voorgerechten.Length; i++)
                        newGerechten[i] = Voorgerechten[i];
                    newGerechten[Voorgerechten.Length] = newGerecht;
                    Voorgerechten = newGerechten;
                }

                if (typeGerechtNaam == "Hoofdgerechten")
                {
                    var newGerechten = new Gerechten[Hoofdgerechten.Length + 1];
                    for (int i = 0; i < Hoofdgerechten.Length; i++)
                        newGerechten[i] = Hoofdgerechten[i];
                    newGerechten[Hoofdgerechten.Length] = newGerecht;
                    Hoofdgerechten = newGerechten;
                }
                if (typeGerechtNaam == "Nagerechten")
                {
                    var newGerechten = new Gerechten[Nagerechten.Length + 1];
                    for (int i = 0; i < Nagerechten.Length; i++)
                        newGerechten[i] = Nagerechten[i];
                    newGerechten[Nagerechten.Length] = newGerecht;
                    Nagerechten = newGerechten;
                }
            }

            MenuOpslaan(); //Called functie die het JSON bestand update
            return;
        }

        private static string ChangeItemName(string OldName) //Functie die gecalled kan worden om een specifiek menu item's naam te veranderen
        {
            bool wrongInput = false;
            while (true)
            {
                Console.Clear();
                Console.WriteLine(ASCIIART.MenuArt());
                Console.WriteLine($"Naam aanpassen:\n\nDit is de oude naam: {OldName}\n\nToets de nieuwe naam in en klik op enter\n\n0: Terug");
                if (wrongInput)
                {
                    Console.WriteLine("Foutieve input!\nZorg ervoor dat de naam alleen letters bevat.");
                    wrongInput = false;
                }
                string userInputNaam = Console.ReadLine();
                if (userInputNaam == "0") //Als de input "0" is , return de oude naam (veranderd niets), anders loopt de code door een paar checks.
                    return OldName;
                else if (userInputNaam.Any(char.IsDigit)) //Checkt of er een getal tussen de letters staat.
                    wrongInput = true;
                else
                    return userInputNaam;
            }
        }

        private static double ChangeItemPrice(double OldPrice) //Functie die gecalled kan worden om een specifiek menu item's prijs te veranderen
        {
            bool wrongInput = false;
            while (true)
            {
                Console.Clear();
                Console.WriteLine(ASCIIART.MenuArt());
                Console.WriteLine($"Prijs aanpassen:\n\nDit is de huidige prijs: {OldPrice}\n\nToets de nieuwe prijs in en klik op enter\n\n0: Terug");
                if (wrongInput)
                {
                    Console.WriteLine("Verkeerde Input! Voer een int (1) of double (1.x) in.");
                    wrongInput = false;
                }
                var userInputPrijs = Console.ReadLine();
                if (userInputPrijs == "0")
                    return OldPrice;
                else
                {
                    try
                    {
                        var userInputPrijsConverted = double.Parse(userInputPrijs.Replace('.', ','));
                        return userInputPrijsConverted;
                    }
                    catch (System.FormatException)
                    {
                        wrongInput = true;
                    }
                }
            }
        }

        private static string ChangeItemDescription(string oldDescription) //Functie die gecalled kan worden om een specifiek menu item's beschrijving te veranderen
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine(ASCIIART.MenuArt());
                Console.WriteLine($"Beschrijving aanpassen:\n\nDit is de huidige beschrijving: {oldDescription}\n\nToets de nieuwe beschrijving in en klik op enter\n\n0: Terug");
                var userInputBeschrijving = Console.ReadLine();
                if (userInputBeschrijving == "0")
                    return oldDescription;
                return userInputBeschrijving;
            }
        }

        public Gerechten[] removeItemScreen(Gerechten[] typeGerecht, string typeGerechtNaam) //Print de gerechten met de optie om die te verwijderen uit de lijst
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine(ASCIIART.MenuArt());
                Console.WriteLine("Item uit " + typeGerechtNaam + " menu verwijderen\x0aToets op het getal naast het menu item wat u weg wil en enter om het te verwijderen\x0a");
                if (typeGerecht.Length > 0)
                {
                    for (int i = 1; i < typeGerecht.Length + 1; i++)
                        Console.WriteLine(i + ": " + typeGerecht[i - 1].naam + "\x0A");
                }
                else
                    Console.WriteLine("<Er bestaan nog geen " + typeGerechtNaam + " in het menu>");

                Console.WriteLine("\n0: Terug");
                ConsoleKeyInfo userInput = Console.ReadKey();
                char userInputChar = userInput.KeyChar;

                if (userInputChar == '0')
                    return typeGerecht;
                try
                {
                    var userInputConverted = Int32.Parse(userInputChar.ToString());
                    Console.Clear();
                    Console.WriteLine(ASCIIART.MenuArt());
                    Console.WriteLine($"Weet u zeker dat u {typeGerecht[userInputConverted - 1].naam} wil verwijderen uit het menu?\n\nA: Item definitief verwijderen\n0: Terug");

                    ConsoleKeyInfo confirmInput = Console.ReadKey();
                    char confirmInputChar = confirmInput.KeyChar;
                    if (confirmInputChar == 'A' || confirmInputChar == 'a')
                    {
                        typeGerecht = removeItemMenu(typeGerecht, typeGerechtNaam, userInputConverted);
                        Console.Clear();
                        Console.WriteLine(ASCIIART.MenuArt());
                        Console.WriteLine("Item is verwijderd uit het menu!\n\n0: Terug naar menu aanpassen");
                        Console.ReadKey();
                        return typeGerecht;
                    }
                }
                catch
                {
                    if (userInputChar == '0')
                        return typeGerecht;
                }
            }
        }

        public Gerechten[] removeItemMenu(Gerechten[] typeGerecht, string typeGerechtNaam, int removeIndex) //Verwijderd een specifiek item bij een specifiek lijst met de correcte ID in de array.
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

            if (typeGerechtNaam == "Voorgerechten")
                Voorgerechten = newGerechten;
            if (typeGerechtNaam == "Hoofdgerechten")
                Hoofdgerechten = newGerechten;
            if (typeGerechtNaam == "Nagerechten")
                Nagerechten = newGerechten;

            MenuOpslaan(); //Called functie die het JSON bestand update
            return newGerechten;
        }

        public void MenuOpslaan() //Veranderd het huidige menu.json met de geupdate versie door het huidige menu object te serializen.
        {
            var JSONoptions = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            var jsonString = JsonSerializer.Serialize(this, JSONoptions);
            File.WriteAllText("menu.json", jsonString);
        }
    }

    public class Gerechten
    {
        public string naam { get; set; }
        public double prijs { get; set; }
        public string beschrijving { get; set; }
        public string[] ingredienten { get; set; }

        public Gerechten(string _naam, double _prijs, string _beschrijving, string[] _ingredienten)
        {
            this.naam = _naam;
            this.prijs = _prijs;
            this.beschrijving = _beschrijving;
            this.ingredienten = _ingredienten;
        }

        public Gerechten(string type)
        {
            this.naam = "<Nog geen naam>";
            this.prijs = 0.0;
            this.beschrijving = "<nog geen beschrijving";
            this.ingredienten = null;

            ItemAanMaakScherm(type);
        }

        public Gerechten() { } //Lege constructor voor json serialisen


        private void ItemAanMaakScherm(string type)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine(ASCIIART.MenuArt());
                Console.WriteLine($"Voeg een nieuw {type} item toe aan het menu\x0A");
                Console.WriteLine($"1: Naam \t\t{naam}");
                Console.WriteLine($"2: Prijs \t\t{prijs}");
                Console.WriteLine($"3: Beschrijving \t{beschrijving}");
                Console.WriteLine("4: Ingredienten:");
                if (ingredienten != null)
                {
                    for (int i = 0; i < ingredienten.Length; i++)
                        Console.WriteLine($"- {ingredienten[i]}");
                }
                else
                    Console.WriteLine("<Nog geen ingredienten>");
                this.naam = "a";
                return;
            }



        }
    }
}