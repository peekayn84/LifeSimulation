namespace Life.Core.Models
{
    public sealed class Food : CellBase
    {
        public int Saturation { get; set; }
        public bool IsVaccine { get; set; }

        public Food(int x, int y, int saturation, bool isVaccine, int visualType)
            : base(x, y, visualType)
        {
            Saturation = saturation;
            IsVaccine = isVaccine;
        }
    }
}
