using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace TheFatDuckRestaurant
{
    public class Gerecht
    {
        public string naam { get; set; }
        public double prijs { get; set; }
        public string beschrijving { get; set; }
        public List<string> ingredienten { get; set; }

        public Gerecht(string type) //Bij Gerecht constructor met 1 parameter wordt het type gerecht doorgegeven en het gerecht aangemaakt.
        {
            this.naam = "Voer een naam in";
            this.prijs = 0.0;
            this.beschrijving = "Voer een beschrijving in";
            this.ingredienten = new List<string>();

            ItemAanMaakScherm(type);
        }

        public Gerecht() { } //Lege constructor voor json serialisen

        /// <summary>
        /// Controller functie, vanuit hier kan ieder attribuut van een gerecht aangepast worden
        /// </summary>
        /// <param name="type">String, voorgerecht, hoofdgerecht of nagerecht</param>
        private void ItemAanMaakScherm(string type) //ItemAanMaakScherm is de plek waar alle attributen worden aangepast van een gerecht, voordat deze vanuit de constructor weer wordt toegevoegd. Als het gerecht toch niet hoeft worden toegevoegd dan returnt de code gewoon een gerecht met de basis attributen.
        {
            bool nogNietAllesIngevuldBijOpslaan = false;
            bool wrongInput = false;
            while (true)
            {
                Console.Clear();
                Console.WriteLine(ASCIIART.MenuArt());
                Console.WriteLine($"Voeg een nieuw {type} item toe aan het menu\n\nToets een van de volgende opties in om het item aan te passen, of toe te voegen/annuleren\n");
                Console.WriteLine($"1: Naam \t\t{naam}");
                Console.WriteLine($"2: Prijs \t\t{prijs}");
                Console.WriteLine($"3: Beschrijving \t{beschrijving}");
                Console.WriteLine("4: Ingredienten:");
                if (ingredienten.Count != 0) //als de ingredienten list meer dan 0 strings bezit, worden ze getoont.
                {
                    for (int i = 0; i < ingredienten.Count; i++)
                        Console.WriteLine($"- {ingredienten[i]}");
                    Console.WriteLine();
                }
                else
                    Console.WriteLine("\t\t\tVoer de ingredienten in\n");
                Console.WriteLine("5: Voeg het item toe aan het menu");
                Console.WriteLine("0: Annuleer en ga terug");
                if (nogNietAllesIngevuldBijOpslaan)
                {
                    Console.WriteLine("\nERROR: Vul eerst alle velden in voordat u het item opslaat!");
                    nogNietAllesIngevuldBijOpslaan = false;
                }
                if (wrongInput)
                    Console.WriteLine("Verkeerde input! Probeer 1, 2, 3, 4, 5 of 6");
                char userInput = Console.ReadKey().KeyChar;
                switch (userInput)
                {
                    case '0':
                        if (ConfirmatieAnnuleren())
                            return;
                        break;
                    case '1':
                        VeranderGerechtNaam();
                        break;
                    case '2':
                        VeranderGerechtPrijs();
                        break;
                    case '3':
                        VeranderGerechtBeschrijving();
                        break;
                    case '4':
                        VeranderGerechtIngredienten();
                        break;
                    case '5':
                        if (CheckOfAlDeAttributenZijnIngevuld())
                            nogNietAllesIngevuldBijOpslaan = false;
                        else
                        {
                            nogNietAllesIngevuldBijOpslaan = true;
                            break;
                        }
                        ConfirmatieToevoegen();
                        return;
                }
            }
        }
        private void VeranderGerechtNaam()
        {
            bool wrongInput = false;
            while (true)
            {
                Console.Clear();
                Console.WriteLine(ASCIIART.MenuArt());
                Console.WriteLine($"Naam aanpassen:\n\nDit is de oude naam: {naam}\n\nToets de nieuwe naam in en klik op enter\n\n0: Terug");
                if (wrongInput)
                {
                    Console.WriteLine("Foutieve input!\nZorg ervoor dat de naam alleen letters bevat.");
                    wrongInput = false;
                }
                string userInputNaam = Console.ReadLine();
                if (userInputNaam == "0") //Als de input "0" is , veranderd de attribuut naam niet.
                    return;
                else if (userInputNaam.Any(char.IsDigit)) //Checkt of er een getal tussen de letters staat.
                    wrongInput = true;
                else
                    naam = userInputNaam;
                return;
            }
        }
        private void VeranderGerechtPrijs()
        {
            bool wrongInput = false;
            while (true)
            {
                Console.Clear();
                Console.WriteLine(ASCIIART.MenuArt());
                Console.WriteLine($"Prijs aanpassen:\n\nDit is de huidige prijs: {prijs}\n\nToets de nieuwe prijs in en klik op enter\n\n0: Terug");
                if (wrongInput)
                {
                    Console.WriteLine("Verkeerde Input! Voer een int (1) of double (1.x) in.");
                    wrongInput = false;
                }
                var userInputPrijs = Console.ReadLine();
                if (userInputPrijs == "0")
                    return;
                else
                {
                    try
                    {
                        var userInputPrijsConverted = double.Parse(userInputPrijs.Replace(',', '.'), CultureInfo.InvariantCulture);
                        prijs = userInputPrijsConverted;
                        return;
                    }
                    catch (FormatException)
                    {
                        wrongInput = true;
                    }
                }
            }
        }
        private void VeranderGerechtBeschrijving()
        {
            Console.Clear();
            Console.WriteLine(ASCIIART.MenuArt());
            Console.WriteLine($"Beschrijving aanpassen:\n\nDit is de huidige beschrijving: {beschrijving}\n\nToets de nieuwe beschrijving in en klik op enter\n\n0: Terug");
            var userInputBeschrijving = Console.ReadLine();
            if (userInputBeschrijving == "0")
                return;
            beschrijving = userInputBeschrijving;
            return;
        }
        private void VeranderGerechtIngredienten()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine(ASCIIART.MenuArt());
                Console.WriteLine($"Ingredienten aanpassen:");
                if (ingredienten.Count != 0)
                {
                    Console.WriteLine("\nAls u een ingredient wil verwijderen toets dan de index in die ernaast staat\nDit zijn de huidige ingredienten:");
                    for (int i = 0; i < ingredienten.Count; i++)
                        Console.WriteLine($"{i + 1} - {ingredienten[i]}");
                }
                else
                    Console.WriteLine("\n<Nog geen ingredienten>");
                Console.WriteLine("\nToets een nieuw ingredient in en klik op enter\n\n0: Terug");
                var userInputIngredienten = Console.ReadLine();

                try
                {
                    var removeIndex = Int32.Parse(userInputIngredienten) - 1;
                    ingredienten.RemoveAt(removeIndex);
                }
                catch
                {
                    int tempInt;
                    if (userInputIngredienten == "0")
                        return;
                    else if (userInputIngredienten != "" && int.TryParse(userInputIngredienten, out tempInt) != true)
                        ingredienten.Add(userInputIngredienten);
                }
            }
        }
        private bool CheckOfAlDeAttributenZijnIngevuld()
        {
            if (naam != "Voer een naam in" && ingredienten.Count != 0 && beschrijving != "Voer een beschrijving in")
                return true;
            return false;
        }
        private bool ConfirmatieAnnuleren()
        {
            bool wrongInput = false;
            while (true)
            {
                Console.Clear();
                Console.WriteLine(ASCIIART.MenuArt());
                Console.WriteLine("Er wordt geen gerecht toegevoegd, weet u dit zeker?\n\n1: Ja, en ga terug\n\n0: Nee en werkt verder aan het gerecht.");
                if (wrongInput)
                    Console.WriteLine("Verkeerde input! Probeer 1 of 0");
                char userInput = Console.ReadKey().KeyChar;
                switch (userInput)
                {
                    case '1':
                        naam = "Voer een naam in"; //als het item niet hoeft worden toegevoegd en de gebruiker terug wilt, wordt de naam op de standard string gezet. Dit wordt gedaan om er zeker van te zijn dat het gerecht niet toch wordt toegevoegd (wat zou gebeuren als alle attributen correct waren aangepast voor het annuleren)
                        return true;
                    case '0':
                        return false;
                    default:
                        wrongInput = true;
                        break;
                }
            }
        }
        private bool ConfirmatieToevoegen()
        {
            bool wrongInput = false;
            while (true)
            {
                Console.Clear();
                Console.WriteLine(ASCIIART.MenuArt());
                Console.WriteLine($"{naam} wordt toegevoegd aan het menu, weet u dit zeker?\n\n1: Ja, en ga terug\n\n0: Nee en werkt verder aan het gerecht.");
                if (wrongInput)
                    Console.WriteLine("Verkeerde input! Probeer 1 of 0");
                char userInput = Console.ReadKey().KeyChar;
                switch (userInput)
                {
                    case '1':
                        return true;
                    case '0':
                        return false;
                    default:
                        wrongInput = true;
                        break;
                }
            }
        }
    }
}
