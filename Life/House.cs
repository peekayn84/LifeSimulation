using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Life
{
    class House
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int VisualType { get; set; }
        public int MaxCells { get; set; }
        public int CurentCells { get; set; }
        public House(int x, int y, int visualType, int maxCells, int curentCells)
        {
            X = x;
            Y = y;
            VisualType = visualType;
            MaxCells = maxCells;
            CurentCells = curentCells;
        }
    }
}
