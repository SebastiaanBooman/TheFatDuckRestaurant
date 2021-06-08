using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using static TheFatDuckRestaurant.ASCIIART;

namespace TheFatDuckRestaurant
{
    public class Menu
    {
        public Gerecht[] Voorgerechten { get; set; }
        public Gerecht[] Hoofdgerechten { get; set; }
        public Gerecht[] Nagerechten { get; set; }

        /// <summary>
        /// Print de lijst met verschillende typen gerechten
        /// </summary>
        /// <param name="typeGebruiker">Type gebruiker die de lijst probeert te bekijken. Bijv: Medewerker</param>
        private void KiesMenuOpties(string typeGebruiker) //Print de lijst met typen Gerechten
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

        /// <summary>
        /// Laad alle gerechten die mee zijn gestuurd in de typeGerecht parameter, en print ook de optie om de gerechten aan te passen indien de gebruiker een eigenaar of medewerker is.
        /// </summary>
        /// <param name="typeGerecht">Array met de typen gerechten die worden geladen. Bijv: Voorgerechten</param>
        /// <param name="typeGebruiker">String die de type gebruiker meestuurd. Bijv: Medewerker</param>
        /// <returns></returns>
        private Bestelling LaadSpecifiekMenu(Gerecht[] typeGerecht, string typeGebruiker) //Laad de gerechten van het type menu (Voorgerechten, Hoofdgerechten, Nagerechten etc).
        {
            int huidigePaginaNR = 0; //Bij de eerste keer laden van het menu zal de eerste 7 gerechten worden getoont.
            bool verkeerdeInput = false;
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
                    catch (IndexOutOfRangeException) { } //Omdat de gerechten per 7 worden laten zien, zal er bij 5 gerechten op een pagina een error komen omdat het programma niet bestaande gerechten 6 en 7 ook probeert te tonen.
                }
                if (huidigePaginaNR + 1 < hoeveelheidPaginas)
                    Console.WriteLine("8: Volgende pagina");
                if (huidigePaginaNR + 1 >= hoeveelheidPaginas && (hoeveelheidPaginas > 1))
                    Console.WriteLine("9: Vorige pagina");


                if (typeGebruiker == "Medewerker") //Als de gebruiker een medewerker is, dan moet er een extra print komen met de optie om het menu aan te passen
                    Console.WriteLine("A: Menu aanpassen");
                Console.WriteLine("0: Terug");

                if (verkeerdeInput)
                {
                    Console.WriteLine("ERROR: Toets een geldig index in");
                    verkeerdeInput = false;
                }
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
                {
                    if (ShowItemHandler(typeGerecht, toetsUserChar, huidigePaginaNR) == false)
                        verkeerdeInput = true;
                }
            }
        }

        public Bestelling BekijkSpecifiekMenu(string typeGebruiker) // Called laadSpecifiekMenu, met de type gebruiker die het menu wil bekijken.
        {
            bool verkeerdeInput = false;

            while (true)
            {
                Console.Clear();
                KiesMenuOpties(typeGebruiker);
                if (verkeerdeInput)
                {
                    Console.WriteLine("Verkeerde input, probeer 1, 2, 3 of 0");
                    verkeerdeInput = false;
                }

                ConsoleKeyInfo toetsUser = Console.ReadKey();
                char toetsUserChar = toetsUser.KeyChar;
                switch (toetsUserChar)
                {
                    case '1':
                        var specifiekMenuVoorgerechten = LaadSpecifiekMenu(Voorgerechten, typeGebruiker);
                        if (specifiekMenuVoorgerechten != null)
                            return specifiekMenuVoorgerechten;
                        else
                            break;

                    case '2':
                        var specifiekMenuHoofdgerechten = LaadSpecifiekMenu(Hoofdgerechten, typeGebruiker);
                        if (specifiekMenuHoofdgerechten != null)
                            return specifiekMenuHoofdgerechten;
                        else
                            break;
                    case '3':
                        var specifiekMenuNagerechten = LaadSpecifiekMenu(Nagerechten, typeGebruiker);
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
        /// <summary>
        /// Bekijkt menu als medewerker
        /// </summary>
        public void BekijkMenuMedewerker() //Opent het menu met de opties voor een medewerker
        {
            while (true)
            {
                Console.Clear();
                BekijkSpecifiekMenu("Medewerker");
                return;
            }
        }
        /// <summary>
        /// Bekijkt menu als uitgelogde gebruiker of klant die het menu opent
        /// </summary>
        public void BekijkMenuGebruiker() //Opent het menu met de opties voor een Gebruiker (niet ingelogd) of klant die niet wil reserveren.
        {
            while (true)
            {
                Console.Clear();
                BekijkSpecifiekMenu("Gebruiker");
                return;
            }
        }
        /// <summary>
        /// Bekijkt menu als klant die een gerecht wil toevoegen aan een reservering
        /// </summary>
        /// <param name="bestelling">Lijst met bestellingen die worden meegestuurd</param>
        /// <returns></returns>
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
        /// <summary>
        /// Functie die ShowItemStandaard called om vervolgens informatie over een specifiek gerecht te bekijken
        /// </summary>
        /// <param name="geladenMenu">Array met het type gerecht</param>
        /// <param name="toetsUserChar">Index van het type gerecht</param>
        /// <param name="paginaNR">Index van op welke pagina het gerecht zich bevindt</param>
        /// <returns></returns>
        private bool ShowItemHandler(Gerecht[] geladenMenu, char toetsUserChar, int paginaNR) //Toont informatie over een specifiek gerecht uit het menu
        {
            try
            {
                ShowItemStandaard(geladenMenu[(Int32.Parse(toetsUserChar.ToString()) - 1) + (7 * paginaNR)]); //7 * paginaNR zorgt ervoor dat het juiste item wordt laten zien
                Console.WriteLine($"\n\n0 : Terug");
                Console.ReadKey();
                return true;
            }
            catch { return false; }
        }
        /// <summary>
        /// Doet he tzelfde als ShowItemHandler maar geeft ook de optie om het gerecht toe te voegen aan een reservering
        /// </summary>
        /// <param name="geladenMenu">Array met het type gerecht</param>
        /// <param name="toetsUserChar">Index van het type gerecht</param>
        /// <param name="paginaNR">Index van op welke pagina het gerecht zich bevindt</param>
        /// <returns></returns>
        private Bestelling ShowItemReserveringHandler(Gerecht[] geladenMenu, char toetsUserChar, int paginaNR) //Toont informatie over specifiek gerecht en geeft optie om het item toe te voegen aan je reservering
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
        /// <summary>
        /// Functie die naam, beschrijving prijs en ingredienten van een gerecht print.
        /// </summary>
        /// <param name="gerecht">Gerecht object waarvan de informatie geladen moet worden</param>
        private void ShowItemStandaard(Gerecht gerecht) //Print informatie over een specifiek gerecht uit het menu
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
            for (int i = 0; i < gerecht.ingredienten.Count; i++)
                Console.WriteLine($"- {gerecht.ingredienten[i]}");
        }
        /// <summary>
        /// Scherm waar de medewerker/eigenaar het menu kan aanpassen
        /// </summary>
        /// <param name="typeGerecht">Array met het type gerecht array wat moet worden aangepast. Bijv: Voorgerechten</param>
        /// <returns></returns>
        private Gerecht[] MenuAanpassenScherm(Gerecht[] typeGerecht) //Handelt het toevoegen van een item aan een specifiek menu.
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
        /// <summary>
        /// Scherm vanaf waar er gekozen kan worden om een nieuw item toe te voegen
        /// </summary>
        /// <param name="typeGerechtNaam">Type gerecht: Bijv: Voorgerechten</param>
        /// <returns></returns>
        private Gerecht AddItemScherm(string typeGerechtNaam) //Print de opties om een item toe te voegen, en handeld correcte inputs.
        {
            Gerecht nieuweGerecht = new Gerecht(typeGerechtNaam);
            if (nieuweGerecht.naam != "Voer een naam in" && nieuweGerecht.beschrijving != "Voer een beschrijving in" && nieuweGerecht.ingredienten.Count != 0)
                return nieuweGerecht;
            return null;
        }

        private void AddItemHandler(string typeGerechtNaam)
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
                    var newGerechten = new Gerecht[Voorgerechten.Length + 1];
                    for (int i = 0; i < Voorgerechten.Length; i++)
                        newGerechten[i] = Voorgerechten[i];
                    newGerechten[Voorgerechten.Length] = newGerecht;
                    Voorgerechten = newGerechten;
                }

                if (typeGerechtNaam == "Hoofdgerechten")
                {
                    var newGerechten = new Gerecht[Hoofdgerechten.Length + 1];
                    for (int i = 0; i < Hoofdgerechten.Length; i++)
                        newGerechten[i] = Hoofdgerechten[i];
                    newGerechten[Hoofdgerechten.Length] = newGerecht;
                    Hoofdgerechten = newGerechten;
                }
                if (typeGerechtNaam == "Nagerechten")
                {
                    var newGerechten = new Gerecht[Nagerechten.Length + 1];
                    for (int i = 0; i < Nagerechten.Length; i++)
                        newGerechten[i] = Nagerechten[i];
                    newGerechten[Nagerechten.Length] = newGerecht;
                    Nagerechten = newGerechten;
                }
            }
            MenuOpslaan(); //Called functie die het JSON bestand update
            return;
        }
        /// <summary>
        /// Scherm vanaf waar de medewerker/eigenaar items kan verwijderen uit het menu
        /// </summary>
        /// <param name="typeGerecht">Array met de type gerechten van waar een item weg moet.</param>
        /// <param name="typeGerechtNaam">Naam van de array. Bijv: Voorgerechten</param>
        /// <returns></returns>
        private Gerecht[] removeItemScreen(Gerecht[] typeGerecht, string typeGerechtNaam) //Print de gerechten met de optie om die te verwijderen uit de lijst
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

        private Gerecht[] removeItemMenu(Gerecht[] typeGerecht, string typeGerechtNaam, int removeIndex) //Verwijderd een specifiek item bij een specifiek lijst met de correcte ID in de array.
        {

            var JSONoptions = new JsonSerializerOptions
            {
                WriteIndented = true,
            };

            Gerecht[] newGerechten = new Gerecht[typeGerecht.Length - 1];
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
        /// <summary>
        /// Slaat het menu op in het JSON bestand
        /// </summary>
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
}