namespace Life.Core.Abstractions
{
    public abstract class CellBase : ICell
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int VisualType { get; set; }

        public CellBase(int x, int y, int visualType)
        {
            X = x;
            Y = y;
            VisualType = visualType;
        }
    }
}