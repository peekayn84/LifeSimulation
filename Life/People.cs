using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Life
{
    class People
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Health { get; set; }
        public int Feed { get; set; }
        public int Old { get; set; }
        public int VisualType { get; set; }
        public int VirusStrength { get; set; }
        public bool VirusGoDown { get; set; }
        public bool HasMask { get; set; }
        public int VaccineProtection { get; set; }
        public bool AtHouse { get; set; }
        public People(int x, int y, int health, int feed, int old, int visualType, int virusStrength, bool virusGoDown, bool hasMask, int vaccineProtection, bool atHouse)
        {
            X = x;
            Y = y;
            Health = health;
            Feed = feed;
            Old = old;
            VisualType = visualType;
            VirusStrength = virusStrength;
            VirusGoDown = virusGoDown;
            HasMask = hasMask;
            VaccineProtection = vaccineProtection;
            AtHouse = atHouse;
        }
    }
}
