using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

namespace TheFatDuckRestaurant
{
    public class TafelArray
    {
        public Tafel[] Tafels { get; set; }

        public TafelArray() { } //Empty constructor for json deserializen

        public void BekijkVrijeTafels(string tijdEnDatum)
        {
            int checkTijd = tijdEnDatumNaarInt(tijdEnDatum); //De gegeven tijd in int (1200)
            string checkDatum = tijdEnDatumNaarDatum(tijdEnDatum); //De gegeven datum in string (Zaterdag 1 januari 2021)
            bool wrongInput = false;
            while (true)
            {
                Console.Clear();
                Console.WriteLine(ASCIIART.TafelsArt());
                Console.WriteLine($"Vrije Tafels in The Fat Duck voor {checkDatum} om {checkTijd} uur. (+-2 uur extra voor lopende reserveringen)\n");
                string topString = "";
                string tafelString = ""; //tafel string waar alle tafels aan worden toegevoegt
                int counter = 1;
                foreach (Tafel tafel in Tafels)
                {
                    bool alGereserveerd = false;
                    if (tafel.Gereserveerd != null)
                    {
                        foreach (string data in tafel.Gereserveerd)
                        {
                            if (isEenTafelAlGereserveerd(data, checkDatum, checkTijd))
                            {
                                alGereserveerd = true;
                                break;
                            }
                        }
                    }
                    if (alGereserveerd != true)
                        tafelString+=$"{tafel.ID},\t{tafel.Plekken}\t\t";
                    if (counter % 3 == 0)
                        tafelString += "\n"; //zorgt voor in totaal 3 tabellen van tafels
                    counter++;
                }
                if(counter >= 3)
                    topString += "ID:\tPlek:\t\tID:\tPlek:\t\tID:\tPlek:\t";
                else if(counter == 2)
                    topString += "ID:\tPlek:\t\tID:\tPlek:\t";
                else
                    topString += "ID:\tPlek:\t";

                Console.WriteLine(topString);
                Console.WriteLine(tafelString);
                Console.WriteLine("\n\n0: Terug");
                if (wrongInput)
                    Console.WriteLine("Verkeerde Input! Probeer 0");
                char userInput = Console.ReadKey().KeyChar;
                if (userInput == '0')
                    return;
                wrongInput = true;
            }
        }

        public int tijdEnDatumNaarInt(string tijdEnDatum) => int.Parse(tijdEnDatum.Substring(0,4)); //returnt de eerste 4 characters van een tijd (1200)
        public string tijdEnDatumNaarDatum(string tijdEnDatum) => tijdEnDatum.Substring(4); //returnt de characters na de tijd (datum)
        public bool tijdCheck(int tafelTijd, int reserveringTijd, int uren) => ((tafelTijd + uren >= reserveringTijd) || ((reserveringTijd - uren) <= tafelTijd)) ? true : false; //return true Als tafeltijd + uren groter is dan reserveringtijd, of als reserveringstijd - uren kleiner of gelijk is aan tafeltijd.

        public int BerekenTafelsDieNogGekoppeldMoetenWorden(int aantalMensen, List<Tafel> gereserveerdeTafels)
        {
            int totaleGereserveerdePlekken = 0;
            if (gereserveerdeTafels == null)
                gereserveerdeTafels = new List<Tafel>();
            foreach (Tafel tafel in gereserveerdeTafels)
            {
                totaleGereserveerdePlekken += tafel.Plekken;
                if (totaleGereserveerdePlekken >= aantalMensen)
                    return -1; //returns negative int als er genoeg tafels zijn gekoppeld
            }
            return aantalMensen - totaleGereserveerdePlekken; // returns de hoeveelheid plekken die nog gekkopeled moeten worden.
        }

        private bool isEenTafelAlGereserveerd(string tafelData, string checkDatum, int checkTijd)
        {
            if ((tijdEnDatumNaarDatum(tafelData) == checkDatum) && (tijdCheck(checkTijd, tijdEnDatumNaarInt(tafelData), 200))) //Als het op dezelfde dag plaats vind dan checkt of de tafel al gereserveerd is ergens binnen 200 (2 uur) van tevoren of 2 uur daarna, zo ja, Is al gereserveerd.
                return true;
            return false;
        }

        public List<Tafel> AutomatischKoppelen(int aantalMensen, List<Tafel> gereserveerdeTafels, string tijdEnDatum, bool tochKoppelen = false) //TODO: Aantal plekken zou berekend kunnen worden (voor iedere tafel in de lijst die al beschikbaar is - aantalMensen) zodat er niet dubbel gekoppeld kan worden.
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine(ASCIIART.TafelsKoppelenArt());
                if (BerekenTafelsDieNogGekoppeldMoetenWorden(aantalMensen, gereserveerdeTafels) <= 0) //0 megenomen omdat dat aangeeft dat er precies genoeg tafels zijn gekoppeld.
                {
                    Console.WriteLine("Er zijn al genoeg tafels gekoppeld aan deze reservering. Klik op een toets om terug te gaan");
                    ConsoleKeyInfo input = Console.ReadKey();
                    return gereserveerdeTafels;
                }

                bool wrongInput = false;
                Console.WriteLine($"Er worden automatisch tafels toegevoegd die passen bij {aantalMensen} personen, weet u dit zeker?\n");
                Console.WriteLine($"1: Ja\n");
                Console.WriteLine($"0: Nee\n");
                if (wrongInput)
                    Console.WriteLine("Wrong Input! Probeer 1 of 0");
                char userInput = Console.ReadKey().KeyChar;
                switch (userInput)
                {
                    case '1':
                        Console.Clear();
                        Console.WriteLine(ASCIIART.TafelsKoppelenArt());
                        int plekkenDieNogMoetenWordenGekoppeld = BerekenTafelsDieNogGekoppeldMoetenWorden(aantalMensen, gereserveerdeTafels);
                        int checkTijd = tijdEnDatumNaarInt(tijdEnDatum); //De gegeven tijd in int (1200)
                        string checkDatum = tijdEnDatumNaarDatum(tijdEnDatum); //De gegeven datum in string (Zaterdag 1 januari 2021)
                        if (gereserveerdeTafels == null)
                            gereserveerdeTafels = new List<Tafel>();
                        foreach (Tafel tafel in Tafels)
                        {
                            if (plekkenDieNogMoetenWordenGekoppeld > 0)
                            {
                                if (tafel.Gereserveerd == null)
                                    tafel.Gereserveerd = new List<string>();

                                bool alGereserveerd = false;
                                bool geenGoedeTafel = false;
                                foreach (string data in tafel.Gereserveerd)
                                {
                                    if(isEenTafelAlGereserveerd(data, checkDatum, checkTijd))
                                    {
                                        alGereserveerd = true;
                                        break;
                                    }
                                }
                                if (plekkenDieNogMoetenWordenGekoppeld <= 2 && tafel.Plekken != 2 && !alGereserveerd) //Als er nog maar 2 of minder mensen zijn die een tafel nodig heeft van een reservering, kijkt het systeem om de tafel 2 personen heeft om ruimte te besparen
                                    geenGoedeTafel = true;

                                else if (!alGereserveerd && !geenGoedeTafel)
                                {
                                    Console.WriteLine($"Tafel {tafel.ID} toegevoegd aan reservering, {tafel.Plekken} plekken");
                                    plekkenDieNogMoetenWordenGekoppeld -= tafel.Plekken;
                                    gereserveerdeTafels.Add(tafel);
                                    tafel.Gereserveerd.Add($"{tijdEnDatumNaarInt(tijdEnDatum)}{tijdEnDatumNaarDatum(tijdEnDatum)}"); //1200Zaterdag 1 januari 2021
                                }
                            }
                        }
                        if (plekkenDieNogMoetenWordenGekoppeld > 0 && tochKoppelen == false)
                        { //Als er geen tafels van 2 zijn voor de laatste 2 of 1 persoon van een reservering dan laat het systeem dat weten.
                            Console.WriteLine($"{plekkenDieNogMoetenWordenGekoppeld} Plekken zijn niet succesvol gekoppeld omdat er geen tafels van 2 personen meer vrij zijn, wilt u dat het systeem checkt of er nog tafels van 4 personen zijn en deze koppelen?\n\n1: Ja\n0: Nee");
                            char inputUser = Console.ReadKey().KeyChar;
                            switch (inputUser)
                            {
                                case '1':
                                    AutomatischKoppelen(aantalMensen, gereserveerdeTafels, tijdEnDatum, true);
                                    return gereserveerdeTafels;
                                case '0':
                                    SaveTafels(this);
                                    return gereserveerdeTafels;
                            }
                        }
                        SaveTafels(this);
                        Console.WriteLine("Toets op een knop om verder te gaan");
                        Console.ReadLine();
                        return gereserveerdeTafels;
                    case '0':
                        return gereserveerdeTafels;
                    default:
                        wrongInput = true;
                        break;
                }
            }
        }


        public List<Tafel> KoppelenDoorMedewerker(int aantalMensen, List<Tafel> gereserveerdeTafels, string tijdEnDatum) //Koppelen door medewerker heeft als input een int aantal mensen, en list van tafels die al gekoppeld zijn aan de reservering.
        {
            int checkTijd = tijdEnDatumNaarInt(tijdEnDatum); //De gegeven tijd in int (1200)
            string checkDatum = tijdEnDatumNaarDatum(tijdEnDatum); //De gegeven datum in string (Zaterdag 1 januari 2021)
            while (true)
            {
                Console.Clear();
                Console.WriteLine(ASCIIART.TafelsKoppelenArt());
                if (BerekenTafelsDieNogGekoppeldMoetenWorden(aantalMensen, gereserveerdeTafels) <= 0) //0 megenomen omdat dat aangeeft dat er precies genoeg tafels zijn gekoppeld.
                {
                    Console.WriteLine("Er zijn genoeg tafels gekoppeld aan deze reservering. Klik op een toets om terug te gaan");
                    SaveTafels(this);
                    ConsoleKeyInfo input = Console.ReadKey();
                    return gereserveerdeTafels;
                }
                Console.WriteLine("Voer het ID van een tafel in om hem toe te voegen aan de reservering (bijv: 1A)\n\n0: Terug");
                string userInput = Console.ReadLine();
                if (userInput == "0")
                {
                    SaveTafels(this);
                    return gereserveerdeTafels;
                }
                bool succesvolGekoppeld = false;
                foreach (Tafel tafel in Tafels)
                {
                    bool alGereserveerd = false;
                    if (tafel.Gereserveerd == null)
                        tafel.Gereserveerd = new List<string>();
                    if (userInput == tafel.ID)
                    {
                        foreach (string data in tafel.Gereserveerd)
                        {
                            if (tijdEnDatumNaarDatum(data) == checkDatum) //Als het op dezelfde dag plaats vind
                            {
                                if (tijdCheck(checkTijd, tijdEnDatumNaarInt(data), 200)) //checkt of de tafel al gereserveerd is ergens binnen 200 (2 uur) van tevoren of 2 uur daarna, zo ja, Is al gereserveerd.
                                    alGereserveerd = true;
                            }
                        }
                        if (!alGereserveerd)
                        {
                            if (gereserveerdeTafels == null)
                                gereserveerdeTafels = new List<Tafel>();
                            Console.Clear();
                            Console.WriteLine(ASCIIART.TafelsArt());
                            gereserveerdeTafels.Add(tafel);
                            tafel.Gereserveerd.Add($"{tijdEnDatumNaarInt(tijdEnDatum)}{tijdEnDatumNaarDatum(tijdEnDatum)}"); //1200Zaterdag 1 januari 2021
                            succesvolGekoppeld = true;
                            Console.WriteLine($"{tafel.ID} toegevoegd aan de reservering!\n\nKlik op een toets en klik enter om verder te gaan");
                            Console.ReadLine();
                            break;
                        }
                    }
                }
                if (!succesvolGekoppeld)
                {
                    Console.Clear();
                    Console.WriteLine(ASCIIART.TafelsArt());
                    Console.WriteLine($"{userInput} is al gekoppeld voor deze tijd of bestaat niet als tafel in het systeem! Klik op een toets om door te gaan");
                    Console.ReadLine();
                }
            }
        }
        public List<Tafel> allesAutomatischOntkoppelen(List<Tafel> gereserveerdeTafels, string tijdEnDatum, bool klantCall = false) //klantCall is een variabele die op true wordt gezet als er vanuit een klant een reservering wordt verwijderd.
        {
            while (true)
            {
                bool wrongInput = false;
                char userInput = '6';
                Console.Clear();
                if (klantCall)
                    userInput = '1';
                else
                {
                    Console.WriteLine(ASCIIART.TafelsOntkoppelenArt());
                    Console.WriteLine($"alle tafels van de reservering worden automatisch ontkoppeld, weet u dit zeker?\n");
                    Console.WriteLine($"1: Ja\n");
                    Console.WriteLine($"0: Nee\n");
                    if (wrongInput)
                        Console.WriteLine("Wrong Input! Probeer 1 of 0");
                    userInput = Console.ReadKey().KeyChar;
                }
                switch (userInput)
                {
                    case '1':
                        Console.Clear();
                        if (gereserveerdeTafels == null)
                            gereserveerdeTafels = new List<Tafel>();
                        foreach (Tafel tafel in Tafels)
                        {
                            if (tafel.Gereserveerd == null)
                                tafel.Gereserveerd = new List<string>();

                            for(int i = 0; i < tafel.Gereserveerd.Count; i++)
                            {
                                if(tafel.Gereserveerd[i] == tijdEnDatum)
                                {
                                    tafel.Gereserveerd.Remove(tafel.Gereserveerd[i]);
                                    if (!klantCall) //Als een klant zijn reservering verwijderd hoeft deze print niet getoont te worden
                                        Console.WriteLine($"Tafel {tafel.ID} is ontkoppeld van de reservering");
                                }
                            }
                        }
                        gereserveerdeTafels.Clear();
                        SaveTafels(this);
                        if (klantCall)
                            return gereserveerdeTafels;
                        Console.WriteLine("Toets op een knop om verder te gaan");
                        Console.ReadLine();
                        return gereserveerdeTafels;
                    case '0':
                        return gereserveerdeTafels;
                    default:
                        wrongInput = true;
                        break;
                }
            }
        }

        public List<Tafel> ontKoppelenMetID(int aantalMensen, List<Tafel> gereserveerdeTafels, string tijdEnDatum)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine(ASCIIART.TafelsOntkoppelenArt());
                Console.WriteLine("Voer het ID van de tafel in die u wilt ontkoppelen\n\nKlik op 0 en klik enter om terug te gaan");
                string userInput = Console.ReadLine();
                if (userInput == "0")
                    return gereserveerdeTafels;
                bool tafelSuccesvolOntkoppeld = false;
                for(int j = 0; j < gereserveerdeTafels.Count; j++)
                {
                    if(gereserveerdeTafels[j].ID == userInput)
                    {
                        for (int i = 0; i < gereserveerdeTafels[j].Gereserveerd.Count; i++)
                        {
                            if (gereserveerdeTafels[j].Gereserveerd[i] == tijdEnDatum)
                            {
                                Console.Clear();
                                Console.WriteLine(ASCIIART.TafelsOntkoppelenArt());
                                Console.WriteLine($"{gereserveerdeTafels[j].ID} succesvol ontkoppeld van de reservering\n\nKlik op een toets om door te gaan");
                                gereserveerdeTafels[j].Gereserveerd.Remove(tijdEnDatum);
                                gereserveerdeTafels.Remove(gereserveerdeTafels[j]);
                                tafelSuccesvolOntkoppeld = true;
                                Console.ReadKey();
                                break;
                            }
                        }
                    }
                }
                if (!tafelSuccesvolOntkoppeld)
                {
                    Console.Clear();
                    Console.WriteLine(ASCIIART.TafelsOntkoppelenArt());
                    Console.WriteLine($"Tafel {userInput} is niet gevonden voor deze reservering.\n\nKlik op een toets om terug te gaan");
                    Console.ReadKey();
                }
            }
        }

        public void SaveTafels(TafelArray tafels)
        {
            var JSONoptions = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            File.WriteAllText("Tafels.json", JsonSerializer.Serialize(tafels, JSONoptions));
        }
    }

    public class Tafel
    {
        public string ID { get; set; }
        public int Plekken { get; set; }
        public List<string> Gereserveerd { get; set; }

        public Tafel() { } //Empty constructor for json desrializen
    }
}