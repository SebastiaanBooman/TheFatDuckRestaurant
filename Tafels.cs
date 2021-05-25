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

        public void BekijkVrijeTafels()
        {
            Console.Clear();
            Console.WriteLine("Vrije Tafels in The Fat Duck\n");
            Console.WriteLine("ID:\tPlekken:\n");
            foreach (Tafel tafel in Tafels)
            {
                if (tafel.Gereserveerd != true)
                    Console.WriteLine($"{tafel.ID}, {tafel.Plekken}\n");
            }
        }
        public int BerekenTafelsDieNogGekoppeldMoetenWorden(int aantalMensen, List<Tafel> gereserveerdeTafels)
        {
            int totaleGereserveerdePlekken = 0;
            foreach(Tafel tafel in gereserveerdeTafels)
            {
                totaleGereserveerdePlekken += tafel.Plekken;
                if (totaleGereserveerdePlekken >= aantalMensen)
                    return -1; //returns negative int als er genoeg tafels zijn gekoppeld
            }
            return aantalMensen - totaleGereserveerdePlekken; // returns de hoeveelheid plekken die nog gekkopeled moeten worden.
        }

        public List<Tafel> AutomatischKoppelen(int aantalMensen, List<Tafel> gereserveerdeTafels) //TODO: Aantal plekken zou berekend kunnen worden (voor iedere tafel in de lijst die al beschikbaar is - aantalMensen) zodat er niet dubbel gekoppeld kan worden.
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine(ASCIIART.TafelsArt());
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
                        int totalePlekken = BerekenTafelsDieNogGekoppeldMoetenWorden(aantalMensen, gereserveerdeTafels); //-> de hoeveelheid plekken die nog gereserveerd moeten worden
                        if (gereserveerdeTafels == null)
                            gereserveerdeTafels = new List<Tafel>();
                        foreach (Tafel tafel in Tafels)
                        {
                            if (totalePlekken > 0)
                            {
                                if (tafel.Gereserveerd != true)
                                {
                                    Console.WriteLine($"Tafel {tafel.ID} toegevoegd aan reservering");
                                    totalePlekken -= tafel.Plekken;
                                    gereserveerdeTafels.Add(tafel);
                                    tafel.Gereserveerd = true;
                                }
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

        public List<Tafel> KoppelenDoorMedewerker(int aantalMensen, List<Tafel> gereserveerdeTafels) //Koppelen door medewerker heeft als input een int aantal mensen, en list van tafels die al gekoppeld zijn aan de reservering.
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine(ASCIIART.TafelsArt());
                if (BerekenTafelsDieNogGekoppeldMoetenWorden(aantalMensen, gereserveerdeTafels) <= 0) //0 megenomen omdat dat aangeeft dat er precies genoeg tafels zijn gekoppeld.
                {
                    Console.WriteLine("Er zijn genoeg tafels gekoppeld aan deze reservering. Klik op een toets om terug te gaan");
                    SaveTafels(this);
                    ConsoleKeyInfo input = Console.ReadKey();
                    return gereserveerdeTafels;
                }
                Console.WriteLine("Voer het ID van een tafel in om hem toe te voegen aan de reservering (bijv: 1A)\n\n0: Terug");
                string userInput = Console.ReadLine();
                if(userInput == "0")
                {
                    SaveTafels(this);
                    return gereserveerdeTafels;
                }

                foreach(Tafel tafel in Tafels)
                {
                    if (tafel.Gereserveerd == false && tafel.ID == userInput)
                    {
                        Console.Clear();
                        Console.WriteLine(ASCIIART.TafelsArt());
                        gereserveerdeTafels.Add(tafel);
                        Console.WriteLine($"{tafel.ID} toegevoegd aan de reservering!\n\nKlik op een toets en klik enter om verder te gaan");
                        Console.ReadLine();
                    }
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
        public bool Gereserveerd { get; set; }

        public Tafel() { } //Empty constructor for json desrializen
    }
}
