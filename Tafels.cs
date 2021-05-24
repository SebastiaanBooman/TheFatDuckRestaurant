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
        public List<Tafel> AutomatischKoppelen(int aantalMensen, List<Tafel> gereserveerdeTafels)
        {
            while (true)
            {
                Console.Clear();
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
                        int totalePlekken = 0;
                        if (gereserveerdeTafels == null)
                            gereserveerdeTafels = new List<Tafel>();
                        foreach (Tafel tafel in Tafels)
                        {
                            if (totalePlekken < aantalMensen)
                            {
                                if (tafel.Gereserveerd != true)
                                {
                                    Console.WriteLine($"Tafel {tafel.ID} toegevoegd aan reservering");
                                    totalePlekken += tafel.Plekken;
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
