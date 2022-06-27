namespace Life.Core.Abstractions
{
    public interface ICell
    {
        int X { get; set; }
        int Y { get; set; }
        int VisualType { get; set; }
    }
}