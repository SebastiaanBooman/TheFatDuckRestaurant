﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TheFatDuckRestaurant
{
    class Reserveren
    {
        public class Reservatie
        {
            public int Tijd { get; set; }
            public int Dag { get; set; }
            public int Maand { get; set; }
            public int Jaar { get; set; }
            public int Personen { get; set; }
        }
        public static void Reserveer()
        {
            Console.Clear();
            Console.WriteLine("Reserveren\x0a");
            Tuple<int, int, int>[] Dagen = Datums(DateTime.Now);
            for (int i = 0; i < Dagen.Length; i++)
            {
                Console.WriteLine($"{i}: Reserveer voor {Dagen[i].Item1}/{Dagen[i].Item2}/{Dagen[i].Item3}\x0a");
            }
            Console.ReadKey();
        }

        public static Tuple<int, int, int>[] Datums(DateTime Date)
        {
            int Day = Date.Day, Month = Date.Month, Year = Date.Year;
            Tuple<int, int, int>[] tarr = new Tuple<int, int, int>[14];
            tarr[0] = Tuple.Create(Day, Month, Year);
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
                tarr[i] = Tuple.Create(Day, Month, Year);
            }
            return tarr;
        }
    }
}