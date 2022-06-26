namespace Life.Core.Models
{
    public sealed class House : CellBase
    {
        public int MaxCapacity { get; set; }
        public int CurrentCapacity { get; set; }

        public House(int x, int y, int visualType, int maxCapacity, int currentCapacity)
            : base(x, y, visualType)
        {
            MaxCapacity = maxCapacity;
            CurrentCapacity = currentCapacity;
        }
    }
}
