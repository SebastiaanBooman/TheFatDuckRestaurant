using System;
using System.Security;
using System.Text.RegularExpressions;

namespace TheFatDuckRestaurant
{
    public static class VarComponents
    {
        public static bool IsPassword(string password)
        {
            //      1       2               3                   4                5      6
            // @"   ^  (?=.*?[A-Z])    (?=.*?[0-9])    (?=.*?[^a-zA-Z0-9])     .{8,}    $"
            // 1 is de start van de string (input)
            // 2 is een check of er ergens in de string 1 of meer hoofdletters zijn.
            // 3 is een check of er ergens in de string 1 of meer cijfers zijn.
            // 4 is een check of er ergens in de string 1 of meer characters zijn dat geen kleine letter, hoofdletter of getal is
            // 5 is een check of de string minimaal 8 tot meer characters heeft
            // 6 is het einde van de string (input)

            // een nieuw Regex object aangemaakt met de regex die hierboven in uitgelegd.
            Regex regex = new Regex(@"^(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9]).{8,}$");

            // kijkt of de input (password) klopt met de opgegeven requirements van de regex.
            Match match = regex.Match(password);

            // returnt een bool van het resultaat van de regex.Match.
            // true betekent dat het wachtwoord overeen komt met de requirements van de regex
            // false betekent dat het wachtwoord niet overeen komt met de requirements van de regex
            return match.Success;
        }

        public static bool IsWoonplaats(string woonplaats)
        {
            //      1       2               3                   4
            // @"   ^     [A-Z']       ([A-Za-z\s']*)           $"
            // 1 is de start van de string (input)
            // 2 is een check of het eerste karakter een hoofdletter of een ' is.
            // 3 is een check om te kijken of de opgegeven karakters hoofdletter, kleine letter, spatie of een ' is.
            // 4 is het einde van de string (input)

            // Geldige Woonplaatsen:
            // Woonplaats
            // Woon plaats
            // 't Woonplaats
            // Woon 't Plaats

            // een nieuw Regex object aangemaakt met de regex die hierboven in uitgelegd.
            Regex regex = new Regex(@"^([A-Z'])([A-Za-z\s'.]+)$");

            // kijkt of de input (woonplaats) klopt met de opgegeven requirements van de regex.
            Match match = regex.Match(woonplaats);

            // returnt een bool van het resultaat van de regex.Match.
            // true betekent dat de woonplaats overeen komt met de requirements van de regex
            // false betekent dat de woonplaats niet overeen komt met de requirements van de regex
            return match.Success;
        }

        public static bool IsAdres(string adres)
        {
            //      1       2               3                    4                 5                6                   7                    8              9          10           11         12              13           14
            // @"   ^     [A-Z']      ([A-Za-z\s'.]+)      ([1-9]\d{0,4})    (([\s-]{0,1})    ([A-Za-z]{1,2})     ([\s-]{0,1}))?      ([1-9]\d{0,4})?   (([\s]{1})  (\bbis\b)?  (\bBis\b)?  ([\s]{1}    [A-Za-z]{1,2}))?    $"
            // 1 is de start van de string (input)
            // 2 is een check om te kijken of het eerste karakter een Hoofdletter of een ' is.
            // 3 is een check om te kijken of 1 of meer karakters in de string staan wat een hoofdletter, kleineletter, spatie, ' of een . kan zijn.
            // 4 is een check om te kijken of er in de string een getal staat van min length 1 en max length 5. (van 1 tot 99999, kan geen 0 zijn)
            // 5 is een check om te kijken of er een spatie of een - in de string staat. (optioleen)
            // 6 is een check om te kijken of er 1/2 letters in de string staan. (optioneel)
            // 7 is een check om te kijken of er een spatie of een - in de string staat. (optioneel)
            // 8 is een check om te kijken of er een getal in de string staat van min length 0 en max length 5. (optioneel en van 1 tot 99999, kan geen 0 zijn)
            // 9 is een check om te kijken of er een spatie in de string staat. (optioneel)
            // 10 is een check om te kijken of het woord "bis" in de string staat. (optioneel)
            // 11 is een check om te kijken of het woord "Bis" in de string staat. (optioneel)
            // 12 is een check om te kijken of er een spatie in de string staat. (optioneel)
            // 13 is een check om te kijken of er 1/2 letters in de string staan. (optioneel)
            // 14 is het einde van de string (input)

            // Geldige straatnamen:
            // 't Straatnaam
            // Straat 't Naam
            // Straatnaam
            // Straat van der naam
            // G. Straatnaam

            // Geldige huisnummers:
            // 24
            // 33e
            // 19999
            // 146A02
            // A106-A
            // 5-F 4
            // 113B Bis A

            // een nieuw Regex object aangemaakt met de regex die hierboven in uitgelegd.
            Regex regex = new Regex(@"^[A-Z']([A-Za-z\s'.]+)([1-9]\d{0,4})(([\s-]{0,1})([A-Za-z]{1,2})([\s-]{0,1}))?([1-9]\d{0,4})?(([\s]{1})(\bbis\b)?(\bBis\b)?([\s]{1}[A-Za-z]{1,2}))?$");

            // kijkt of de input (adres) klopt met de opgegeven requirements van de regex.
            Match match = regex.Match(adres);

            // returnt een bool van het resultaat van de regex.Match.
            // true betekent dat het adres overeen komt met de requirements van de regex
            // false betekent dat het adres niet overeen komt met de requirements van de regex
            return match.Success;
        }

        public static bool IsUsername(string username)
        {
            //      1       2               3                   4
            // @"   ^     [A-Z']       ([A-Za-z\s']*)           $"
            // 1 is de start van de string (input)
            // 2 is een check of het eerste karakter een hoofdletter of een ' is.
            // 3 is een check om te kijken of de opgegeven karakters hoofdletter, kleine letter, spatie of een ' is.
            // 4 is het einde van de string (input)

            // een nieuw Regex object aangemaakt met de regex die hierboven in uitgelegd.
            Regex regex = new Regex(@"^([A-Za-z0-9]{1,})$");

            // kijkt of de input (username) klopt met de opgegeven requirements van de regex.
            Match match = regex.Match(username);

            // returnt een bool van het resultaat van de regex.Match.
            // true betekent dat de username overeen komt met de requirements van de regex
            // false betekent dat de username niet overeen komt met de requirements van de regex
            return match.Success;
        }

        public static SecureString MaskStringInput()
        {
            SecureString password = new SecureString();
            ConsoleKeyInfo keyInfo;

            // voert de "do" uit zolang de while statement true is
            do
            {
                keyInfo = Console.ReadKey(true);

                // kijkt naar het karakter dat in ingeklikt en kijkt of dat karakter geen spatie of control karakter(ctrl, esc, alt, etc..) is.
                if (!char.IsControl(keyInfo.KeyChar) && keyInfo.Key != ConsoleKey.Spacebar)
                {
                    // voegt het ingetoetste karakter achteraan de secure string password.
                    password.AppendChar(keyInfo.KeyChar);
                    // voegt een "*" toe aan het eind van de line in de console.
                    Console.Write("*");
                }
                // kijkt of het ingevoerde karakter een backspace is en of de lengte van de string hoger is dan 0
                else if (keyInfo.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    // verwijderd het karakter dat aan het eind van de string staat.
                    password.RemoveAt(password.Length - 1);
                    // verwijderd de "*" dat aan het eind staat in de console
                    Console.Write("\b \b");
                }
            }
            // voert de "while" uit als de while statement false is
            while (keyInfo.Key != ConsoleKey.Enter);
            {
                Console.Write("\n");
                return password;
            }
        }
    }
}
