using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Life
{
    class Virus
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Health { get; set; }
        public Virus(int x, int y, int health)
        {
            X = x;
            Y = y;
            Health = health;
        }
    }
}
