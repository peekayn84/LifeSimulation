namespace Life.Core.Models
{
    public sealed class Virus : CellBase
    {
        public int Health { get; set; }

        public Virus(int x, int y, int health)
            : base(x, y, 0)
        {
            Health = health;
        }
    }
}
