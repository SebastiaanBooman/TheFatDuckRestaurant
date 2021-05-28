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

        public void kiesMenuOpties(string typeGebruiker)
        {
            Console.Clear();
            Console.WriteLine(ASCIIART.MenuArt());
            if(typeGebruiker == "Klant")
                Console.WriteLine("Kies het type gerecht wat u wilt toevoegen aan uw reservering:\x0A\x0A");
            else
                Console.WriteLine("KIES HET TYPE GERECHT:\x0A\x0A");
            Console.WriteLine("1: Voorgerechten\x0A");
            Console.WriteLine("2: Hoofdgerechten\x0A");
            Console.WriteLine("3: Nagerechten\x0A\x0a");
            Console.WriteLine("0: Terug");
        }

        public Bestelling laadSpecifiekMenu(Gerechten[] typeGerecht, string typeGebruiker)
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
                Console.WriteLine($"Pagina {huidigePaginaNR +1}/{hoeveelheidPaginas}\n");
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
                if (huidigePaginaNR+1 < hoeveelheidPaginas)
                    Console.WriteLine("8: Volgende pagina");
                if (huidigePaginaNR+1 >= hoeveelheidPaginas && (hoeveelheidPaginas > 1))
                    Console.WriteLine("9: Vorige pagina");


                if (typeGebruiker == "Medewerker")
                    Console.WriteLine("A: Menu aanpassen");
                Console.WriteLine("0: Terug");

                ConsoleKeyInfo toetsUser = Console.ReadKey();
                char toetsUserChar = toetsUser.KeyChar;

                if (typeGebruiker == "Medewerker" && (toetsUserChar == 'A' || toetsUserChar == 'a'))
                    typeGerecht = MenuAanpassenScherm(typeGerecht);

                else if (huidigePaginaNR + 1 < hoeveelheidPaginas && toetsUserChar == '8')
                    huidigePaginaNR++;

                else if (huidigePaginaNR + 1 >= hoeveelheidPaginas && (hoeveelheidPaginas > 1) && toetsUserChar == '9')
                    huidigePaginaNR--;

                else if (toetsUserChar == '0')
                    return null;
                else if (typeGebruiker == "Klant")
                    return ShowItemReserveringHandler(typeGerecht, toetsUserChar, huidigePaginaNR);
                else
                    ShowItemHandler(typeGerecht, toetsUserChar, huidigePaginaNR);
            }
        }

        public Bestelling BekijkSpecifiekMenu(string typeGebruiker)
        {
            bool verkeerdeInput = false;

            while(true)
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

        public void BekijkMenuMedewerker()
        {
            while (true)
            {
                Console.Clear();
                BekijkSpecifiekMenu("Medewerker");
                return;
            }
        }

        public void BekijkMenuGebruiker()
        {
            while (true)
            {
                Console.Clear();
                BekijkSpecifiekMenu("Gebruiker");
                return;
        
            }
        }

        public List<Bestelling> BekijkMenuKlant(List<Bestelling> bestelling = null)
        {
            while (true)
            {
                Console.Clear();
                var bekijkMenu = BekijkSpecifiekMenu("Klant");
                if (bekijkMenu != null)
                {
                    if (bestelling == null)
                        bestelling = new List<Bestelling>();

                    bool addNewOne = true;
                    foreach(var bestellingItem in bestelling)
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
                    Console.WriteLine($"{bekijkMenu.Naam} is succesvol toegevoegd aan uw reservering!\nWil u doorgaan met het menu bekijken/Items toevoegen?\n\n1: Doorgaan\n0: Terug naar reservering");
                    ConsoleKeyInfo toetsUser = Console.ReadKey();
                    char toetsUserChar = toetsUser.KeyChar;

                    if (toetsUserChar != '1')
                        return bestelling;
                }
                else
                    return bestelling;
            }
        }

        public void ShowItemHandler(Gerechten[] geladenMenu, char toetsUserChar, int paginaNR)
        {
            try
            {
                ShowItemStandaard(geladenMenu[(Int32.Parse(toetsUserChar.ToString())-1) + (7 * paginaNR)]); //7 * paginaNR zorgt ervoor dat het juiste item wordt laten zien
                Console.WriteLine($"\n\n0 : Terug");
                Console.ReadKey();
            }
            catch{ return; }
        }

        public Bestelling ShowItemReserveringHandler(Gerechten[] geladenMenu, char toetsUserChar, int paginaNR)
        {
            try
            {  
                bool specifiekPassed = false;
                while(true)
                {
                    ShowItemStandaard(geladenMenu[(Int32.Parse(toetsUserChar.ToString()) - 1) + (7 * paginaNR)]); //7 * paginaNR zorgt ervoor dat het juiste item wordt laten zien
                    Console.WriteLine("\nR: Toevoegen aan reservering");
                    Console.WriteLine($"0 : Terug");
                    ConsoleKeyInfo toetsUsr = Console.ReadKey();
                    char toetsUsrChar = toetsUsr.KeyChar;

                    if (toetsUsrChar == 'R' || toetsUsrChar == 'r')
                        while(specifiekPassed != true)
                        {
                            Console.Clear();
                            Console.WriteLine(ASCIIART.ReserverenArt());
                            Console.WriteLine("Toets hoeveel u van " + geladenMenu[(Int32.Parse(toetsUserChar.ToString()) - 1) + (7 * paginaNR)].naam + " wil bestellen en enter om dit toe te voegen aan uw reservering\n\n0: Terug");
                            var amountStr = Console.ReadLine();
                            if (amountStr == "0")
                                break;
                            try
                            {
                                int amount = Int32.Parse(amountStr);
                                return new Bestelling(geladenMenu[(Int32.Parse(toetsUserChar.ToString()) - 1) + (7 * paginaNR)].naam, geladenMenu[(Int32.Parse(toetsUserChar.ToString()) - 1) + (7 * paginaNR)].prijs, amount);
                            }
                            catch { };
                        }
                    if(toetsUsrChar == '0')
                        return null;
                }
            }
            catch { return null; }
        }

        public void ShowItemStandaard(Gerechten gerecht)
        {

            Console.Clear();
            Console.WriteLine(ASCIIART.MenuArt());
            Console.WriteLine($"Gerecht: " + gerecht.naam + "\x0A");
            Console.WriteLine($"Prijs: {gerecht.prijs} Euro\x0a");
            Console.WriteLine($"Beschrijving: " + gerecht.beschrijving + "\x0a");
            Console.WriteLine($"Ingredienten: ");
            for (int i = 0; i < gerecht.ingredienten.Length; i++)
            {
                Console.WriteLine($"- {gerecht.ingredienten[i]}");
            }
        }

        public Gerechten[] MenuAanpassenScherm(Gerechten[] typeGerecht)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine(ASCIIART.MenuArt());
                if(typeGerecht == Voorgerechten)
                    Console.WriteLine("Voorgerechten menu aanpassen\n\n1: Item toevoegen\n\n2: Item verwijderen\n\n0: Terug");
                else if (typeGerecht == Hoofdgerechten)
                    Console.WriteLine("Hoofdgerechten menu aanpassen\n\n1: Item toevoegen\n\n2: Item verwijderen\n\n0: Terug");
                else
                    Console.WriteLine("Nagerechten menu aanpassen\n\n1: Item toevoegen\n\n2: Item verwijderen\n\n0: Terug");
                ConsoleKeyInfo toetsUser = Console.ReadKey();
                char toetsUserChar = toetsUser.KeyChar;

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
                    case '2':
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
                        //break;
                }
            }
        }

        public Gerechten AddItemScherm(string typeGerechtNaam)
        {
            var nietAllesIngevuld = false;
            var naam_ = "<Nog geen naam>";
            var prijs_ = 0.0;
            var beschrijving_ = "<Nog geen beschrijving>";
            string[] ingredienten_ = null; ;

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
                if (ingredienten_ != null)
                {
                    for (int i = 0; i < ingredienten_.Length; i++)
                    {
                        Console.WriteLine($"- {ingredienten_[i]}");
                    }
                }
                else
                {
                    Console.WriteLine("<Nog geen ingredienten>");
                }
                Console.WriteLine($"\n5: Item opslaan\xA0");
                Console.WriteLine($"0: Terug");
                if(nietAllesIngevuld)
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
                        Console.Clear();
                        while (!passedSpecifiek)
                        {
                            Console.Clear();
                            Console.WriteLine(ASCIIART.MenuArt());
                            Console.WriteLine($"Ingredienten aanpassen:\n\nDit zijn de huidige ingredienten:");
                            if (ingredienten_ != null)
                            {
                                for (int i = 0; i < ingredienten_.Length; i++)
                                {
                                    Console.WriteLine($"- {ingredienten_[i]}");
                                }
                            }
                            else
                                Console.WriteLine("<Nog geen ingredienten.>");
                            Console.WriteLine("\nToets een nieuw ingredient in en klik op enter\n\n0: Terug");
                            var userInputIngredienten = Console.ReadLine();
                            if (userInputIngredienten == "0")
                                passedSpecifiek = true;
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
                    case '5':
                        if(ingredienten_ == null || naam_ == "<Nog geen naam>" || beschrijving_ == "<Nog geen beschrijving>")
                        {
                            nietAllesIngevuld = true;
                            break;
                        }

                        Console.Clear();
                        Console.WriteLine(ASCIIART.MenuArt());
                        Console.WriteLine($"Het item wordt toegevoegd aan het {typeGerechtNaam} menu, weet u dit zeker?\n\nA: Item bevestigen en toevoegen aan het menu\n0: Item annuleren");
                        ConsoleKeyInfo userInputConfirmatie = Console.ReadKey();
                        char userInputConfirmatieChar = userInputConfirmatie.KeyChar;
                        if (userInputConfirmatieChar == 'A' || userInputConfirmatieChar == 'a')
                        {
                            Console.Clear();
                            Console.WriteLine(ASCIIART.MenuArt());
                            Console.WriteLine($"{naam_} is succesvol toegevoegd aan de {typeGerechtNaam}!\n\n0: Terug");
                            Console.ReadKey();
                            return new Gerechten(naam_, prijs_, beschrijving_, ingredienten_);
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

        public void AddItemHandler(string typeGerechtNaam)
        {
            var JSONoptions = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            var newGerecht = AddItemScherm(typeGerechtNaam);

            if(newGerecht != null)
            {

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

            MenuOpslaan();
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
                {
                    return userInputNaam;
                }
            }
        }

        private static double ChangeItemPrice(double OldPrice)
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
                        var userInputPrijsConverted = double.Parse(userInputPrijs);
                        return userInputPrijsConverted;
                    }
                    catch (System.FormatException)
                    {
                        wrongInput = true;
                    }
                }
            }
        }

        private static string ChangeItemDescription(string oldDescription)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine(ASCIIART.MenuArt());
                Console.WriteLine($"Beschrijving aanpassen:\n\nDit is de huidige beschrijving: {oldDescription}\n\nToets de nieuwe beschrijving in en klik op enter\n\n0: Terug");
                var userInputBeschrijving = Console.ReadLine();
                if (userInputBeschrijving == "0")
                {
                    return oldDescription;
                }
                else
                {
                    return userInputBeschrijving;
                }
            }
        }

        public Gerechten[] removeItemScreen(Gerechten[] typeGerecht, string typeGerechtNaam)
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
                    Console.WriteLine($"Weet u zeker dat u {typeGerecht[userInputConverted-1].naam} wil verwijderen uit het menu?\n\nA: Item definitief verwijderen\n0: Terug");

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
                    else
                        passed = false;
                }
                catch
                {
                    if (userInputChar == '0')
                        return typeGerecht;
                }

            }
            return null;
        }

        public Gerechten[] removeItemMenu(Gerechten[] typeGerecht, string typeGerechtNaam, int removeIndex)
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
            
            MenuOpslaan();
            return newGerechten;
        }

        public void MenuOpslaan()
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

        public Gerechten() { }
    }
}