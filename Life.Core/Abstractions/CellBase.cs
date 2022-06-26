namespace Life.Core.Abstractions
{
    public abstract class CellBase : ICell
    {
        int X { get; set; }
        int Y { get; set; }
        int VisualType { get; set; }

        public CellBase(int x, int y, int visualType)
        {
            X = x;
            Y = y;
            VisualType = visualType;
        }
    }
}