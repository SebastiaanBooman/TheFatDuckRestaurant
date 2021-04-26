using System;
using System.Collections.Generic;
using System.Text;

namespace TheFatDuckRestaurant
{
    class Reserveren
    {
        public class Reservatie
        {
            public string Tijd { get; set; }
            public string Datum { get; set; }
            public int Personen { get; set; }
        }
        public static void Reserveer()
        {
            Console.Clear();
            Console.WriteLine("Reserveren\x0a");
            string[] Dagen = Datums(DateTime.Now);
            for (int i = 0; i < Dagen.Length; i++)
            {
                Console.WriteLine($"{i}: Reserveer voor {Dagen[i]}\x0a");
            }
            if (int.TryParse(Console.ReadLine(), out int Keuze)){
                Console.Clear();
                Console.WriteLine($"{Dagen[Keuze]}\x0a");
                string[] Beschikbaar = Tijden(Dagen[Keuze]);
                for(int i = 0; i < Beschikbaar.Length; i++)
                {
                    Console.WriteLine($"{i}: Reserveer voor {Beschikbaar[i]} ({Plaatsen(Dagen[Keuze],Beschikbaar[i])} vrije plekken)\x0a");
                }
                if (int.TryParse(Console.ReadLine(), out int Keuze2))
                {
                    Console.Clear();
                    Console.WriteLine($"{Dagen[Keuze]} om {Beschikbaar[Keuze2]}\x0a\x0aMet hoeveel personen bent u?");
                    if (int.TryParse(Console.ReadLine(), out int Personen) && Personen > 0)
                    {
                        Console.Clear();
                        Console.WriteLine($"U heeft gereserveerd voor {Dagen[Keuze]} om {Beschikbaar[Keuze2]} voor {Personen}" + (Personen == 1 ? " persoon" : " personen") + "\x0a\x0a" + "Enter: Ga terug naar het beginscherm");
                    }
                }
            }
            else
            {
                Console.WriteLine("Verkeerde input\x0a"+"Enter: Ga terug naar het beginscherm");
            }
            Console.ReadKey();
        }

        public static int Plaatsen(string Datum, string Tijd)
        {
            return 100;
        }
        public static string[] Tijden(string Datum)
        {
            string[] Tijdarr = new string[11];
            for(int i = 0, j = 11;i < 11; i++,j++)
            {
                Tijdarr[i] = $"{j}:00";
            }
            return Tijdarr;
        }
        public static string[] Datums(DateTime Date)
        {
            int Day = Date.Day, Month = Date.Month, Year = Date.Year;
            string[] Dagen = new string[14];
            Dagen[0] = $"{Day}/{Month}/{Year}";
            for (int i = 1; i < 14; i++)
            {
                Day++;
                if (Month == 1 || Month == 3 || Month == 5 || Month == 7 || Month == 8 || Month == 10 || Month == 12)
                {
                    if (Day > 31)
                    {
                        Day = 1;
                        if (Month == 12)
                        {
                            Month = 1;
                            Year++;
                        }
                        else
                        {
                            Month++;
                        }
                    }
                }
                else if (Month == 2)
                {
                    if (Year % 4 == 0 && (Year % 100 != 0 || Year % 400 == 0))
                    {
                        if (Day > 29)
                        {
                            Day = 1;
                            Month++;
                        }
                    }
                    else if (Day > 28)
                    {
                        Day = 1;
                        Month++;
                    }
                }
                else
                {
                    if (Day > 30)
                    {
                        Day = 1;
                        Month++;
                    }
                }
                Dagen[i] = $"{Day}/{Month}/{Year}";
            }
            return Dagen;
        }
    }
}
