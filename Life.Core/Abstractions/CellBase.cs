namespace Life.Core.Abstractions
{
    public abstract class CellBase : ICell
    {
        private int _x;
        private int _prevX;
        private int _y;
        private int _prevY;

        public int X { get { return _x; } set { _prevX = _x; _x = value; } }
        public int Y { get { return _y; } set { _prevY = _y; _y = value; } }
        public int PrevX { get { return _prevX; } set { _prevX = value; } }
        public int PrevY { get { return _prevY; } set { _prevY = value; } }
        //public int X { get; set; }
        //public int Y { get; set; }
        public int VisualType { get; set; }

        public CellBase(int x, int y, int visualType)
        {
            PrevX = -1;
            PrevY = -1;
            //_x = x;
            //_y = y;
            _x = x;
            _y = y;
            VisualType = visualType;
        }
    }
}