using Life.Core.Models;
using Life.Core.Configuration;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

namespace Life
{
    public class Image
    {
        private readonly int _width;
        private readonly int _height;

        private readonly Config _config;

        private readonly Bitmap[] _personVisual;
        private readonly Bitmap[] _houseVisual;
        private readonly Bitmap[] _foodVisual;
        private readonly Bitmap[] _vaccineVisual;
        private readonly Bitmap _maskVisual;
        private readonly Bitmap _virusVisual;
        private readonly Bitmap _clearImage;

        private BitmapImage? _currentImageSource;

        public BitmapImage? CurrentImageSource => _currentImageSource;

        public Image(int width, int height, Config config)
        {
            _config = config;

            _width = width;
            _height = height;
            _clearImage = new Bitmap(width, height);

            using (Graphics g = Graphics.FromImage(_clearImage))
            {
                SolidBrush blueBrush = new SolidBrush(Color.White);
                Rectangle rect = new Rectangle(0, 0, width, height);
                g.FillRectangle(blueBrush, rect);
            }

            _personVisual= new Bitmap[AssetsSettings.PersonIconsCount];
            _personVisual[0] = new Bitmap(Life.Properties.Resources.man1, AssetsSettings.CellSizePX, AssetsSettings.CellSizePX);
            _personVisual[1] = new Bitmap(Life.Properties.Resources.man2, AssetsSettings.CellSizePX, AssetsSettings.CellSizePX);
            _personVisual[2] = new Bitmap(Life.Properties.Resources.man3, AssetsSettings.CellSizePX, AssetsSettings.CellSizePX);

            _houseVisual = new Bitmap[AssetsSettings.HouseIconsCount];
            _houseVisual[0] = new Bitmap(Life.Properties.Resources.house1, AssetsSettings.CellSizePX, AssetsSettings.CellSizePX);
            _houseVisual[1] = new Bitmap(Life.Properties.Resources.house2, AssetsSettings.CellSizePX, AssetsSettings.CellSizePX);

            _foodVisual = new Bitmap[AssetsSettings.FoodIconsCount];
            _foodVisual[0] = new Bitmap(Life.Properties.Resources.food1, AssetsSettings.CellSizePX, AssetsSettings.CellSizePX);

            _vaccineVisual = new Bitmap[AssetsSettings.FoodIconsCount];
            _vaccineVisual[0] = new Bitmap(Life.Properties.Resources.vaccine1, AssetsSettings.CellSizePX, AssetsSettings.CellSizePX);

            _maskVisual = new Bitmap(Life.Properties.Resources.mask1, AssetsSettings.CellSizePX, AssetsSettings.CellSizePX);

            _virusVisual = new Bitmap(Life.Properties.Resources.virus1, AssetsSettings.CellSizePX, AssetsSettings.CellSizePX);
        }

        public void GeneratePreview()
        {
            Bitmap temp = (Bitmap)_clearImage.Clone();
            using (Graphics g = Graphics.FromImage(temp))
            {
                for (int x = 0; x < _config.ColumnsCount; x++)
                {
                    for (int y = 0; y < _config.RowsCount; y++)
                    {
                        Pen blackPen = new Pen(Color.Black, 3);
                        Rectangle rect = new Rectangle(x* AssetsSettings.CellSizePX, y* AssetsSettings.CellSizePX, AssetsSettings.CellSizePX, AssetsSettings.CellSizePX);
                        g.DrawRectangle(blackPen, rect);
                    }

                }
            }

            _currentImageSource = BitmapToImageSource(temp);
            //return curentImageSourse;
            //return temp;
        }

        public void GenerateImage(Colony colony)
        {
            Bitmap temp = (Bitmap)_clearImage.Clone();

            using (Graphics g = Graphics.FromImage(temp))
            {
                foreach (Person person in colony.People)
                {
                    Pen healthColor = new Pen(Color.Red, AssetsSettings.BarSizePX);
                    Pen foodhColor = new Pen(Color.Brown, AssetsSettings.BarSizePX);
                    Pen ageColor = new Pen(Color.Green, AssetsSettings.BarSizePX);
                    Pen vaccineColor = new Pen(Color.Blue, AssetsSettings.BarSizePX);
                    Pen virusColor = new Pen(Color.Purple, AssetsSettings.BarSizePX);

                    double persentHealth = 1;
                    double persentFood = 1;
                    double persentAge = 1;
                    double persentVaccine = 1;
                    double persentVirus = 1;

                    if (!person.AtHouse)
                    {
                        g.DrawImage(_personVisual[person.VisualType], person.X * AssetsSettings.CellSizePX, person.Y * AssetsSettings.CellSizePX);

                        if (person.HasMask)
                        {
                            g.DrawImage(_maskVisual, person.X * AssetsSettings.CellSizePX + 42, person.Y * AssetsSettings.CellSizePX + 10, 18, 25);
                        }

                        persentHealth = person.Health * 1.0 / _config.PersonDefaultHealth;
                        persentFood = person.Saturation * 1.0 / _config.PersonDefaultSaturation;
                        persentAge = (_config.MaxAge - person.Age) * 1.0 / _config.MaxAge;
                        persentVaccine = person.VaccineProtection * 1.0 / _config.InitialVaccinePower;
                        persentVirus = person.VirusStrength * 1.0 / ((100 * 1.0 / _config.InitialChanceVirusGoDown) * 1.5);
                        g.DrawLine(ageColor, new Point(person.X * AssetsSettings.CellSizePX, person.Y * AssetsSettings.CellSizePX - AssetsSettings.BarSizePX), new Point((person.X * AssetsSettings.CellSizePX) + Convert.ToInt32(AssetsSettings.CellSizePX * persentAge), person.Y * AssetsSettings.CellSizePX - AssetsSettings.BarSizePX));
                        g.DrawLine(healthColor, new Point(person.X * AssetsSettings.CellSizePX, person.Y * AssetsSettings.CellSizePX), new Point((person.X * AssetsSettings.CellSizePX) + Convert.ToInt32(AssetsSettings.CellSizePX * persentHealth), person.Y * AssetsSettings.CellSizePX));
                        g.DrawLine(foodhColor, new Point(person.X * AssetsSettings.CellSizePX, person.Y * AssetsSettings.CellSizePX + AssetsSettings.BarSizePX), new Point((person.X * AssetsSettings.CellSizePX) + Convert.ToInt32(AssetsSettings.CellSizePX * persentFood), person.Y * AssetsSettings.CellSizePX + AssetsSettings.BarSizePX));
                        g.DrawLine(vaccineColor, new Point(person.X * AssetsSettings.CellSizePX, person.Y * AssetsSettings.CellSizePX + (AssetsSettings.BarSizePX * 2)), new Point((person.X * AssetsSettings.CellSizePX) + Convert.ToInt32(AssetsSettings.CellSizePX * persentVaccine), person.Y * AssetsSettings.CellSizePX + (AssetsSettings.BarSizePX * 2)));
                        g.DrawLine(virusColor, new Point(person.X * AssetsSettings.CellSizePX, person.Y * AssetsSettings.CellSizePX + (AssetsSettings.BarSizePX * 3)), new Point((person.X * AssetsSettings.CellSizePX) + Convert.ToInt32(AssetsSettings.CellSizePX * persentVirus), person.Y * AssetsSettings.CellSizePX + (AssetsSettings.BarSizePX * 3)));
                    }
                }

                foreach (Virus virus in colony.Viruses)
                {
                    g.DrawImage(_virusVisual, virus.X * AssetsSettings.CellSizePX, virus.Y * AssetsSettings.CellSizePX);
                }

                foreach (House house in colony.Houses)
                {
                    Pen houseFullColor = new Pen(Color.Red, AssetsSettings.BarSizePX);
                    double persentHouseFull = house.CurrentCapacity * 1.0 / house.MaxCapacity;
                    g.DrawImage(_houseVisual[house.VisualType], house.X * AssetsSettings.CellSizePX, house.Y * AssetsSettings.CellSizePX);
                    g.DrawLine(houseFullColor, new Point(house.X * AssetsSettings.CellSizePX, house.Y * AssetsSettings.CellSizePX), new Point((house.X * AssetsSettings.CellSizePX) + Convert.ToInt32(AssetsSettings.CellSizePX * persentHouseFull), house.Y * AssetsSettings.CellSizePX));
                }

                foreach (Food food in colony.FoodItems)
                {
                    if (food.IsVaccine)
                    {
                        g.DrawImage(_vaccineVisual[food.VisualType], food.X * AssetsSettings.CellSizePX, food.Y * AssetsSettings.CellSizePX);
                    }
                    else
                    {
                        g.DrawImage(_foodVisual[food.VisualType], food.X * AssetsSettings.CellSizePX, food.Y * AssetsSettings.CellSizePX);
                    }
                }
            }

            _currentImageSource = BitmapToImageSource(temp);
            //return curentImageSourse;
            //return temp;
        }

        public BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }
    }
}
