using System;
using System.Collections.Generic;
using System.Text;

namespace TheFatDuckRestaurant
{
    public class TafelArray
    {
        public Tafel Tafels { get; set; }
    }


    public class Tafel
    {
        public string ID { get; set; }
        public int Plekken { get; set; }
        public bool Gereserveerd { get; set; }
    }
}
