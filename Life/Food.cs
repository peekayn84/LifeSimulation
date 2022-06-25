using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Life
{
    class Food
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int FeedAdd { get; set; }
        public bool Vaccine { get; set; }
        public int VisualType { get; set; }
        public Food(int x, int y, int feedAdd, bool vaccine, int visualType)
        {
            X = x;
            Y = y;
            FeedAdd = feedAdd;
            Vaccine = vaccine;
            VisualType = visualType;

        }
    }
}
