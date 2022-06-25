using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Life
{
    class Image
    {
        int width, height;
        Bitmap clearImage;
        public BitmapImage curentImageSourse;
        public Bitmap[] personVisual;
        public Bitmap[] houseVisual;
        public Bitmap[] foodVisual;
        public Bitmap[] vaccineVisual;
        public Bitmap maskVisual;
        public Bitmap virusVisual;
        public Image(int width, int height)
        {
            this.width = width;
            this.height = height;
            clearImage = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(clearImage))
            {
                SolidBrush blueBrush = new SolidBrush(Color.White);
                Rectangle rect = new Rectangle(0, 0, width, height);
                g.FillRectangle(blueBrush, rect);
            }
            personVisual= new Bitmap[Settings.visualManCount];
            personVisual[0] = new Bitmap(Life.Properties.Resources.man1,Settings.sizeCell, Settings.sizeCell);
            personVisual[1] = new Bitmap(Life.Properties.Resources.man2, Settings.sizeCell, Settings.sizeCell);
            personVisual[2] = new Bitmap(Life.Properties.Resources.man3, Settings.sizeCell, Settings.sizeCell);
            houseVisual = new Bitmap[Settings.visualHouseCount];
            houseVisual[0] = new Bitmap(Life.Properties.Resources.house1, Settings.sizeCell, Settings.sizeCell);
            houseVisual[1] = new Bitmap(Life.Properties.Resources.house2, Settings.sizeCell, Settings.sizeCell);
            foodVisual = new Bitmap[Settings.visualFoodCount];
            foodVisual[0] = new Bitmap(Life.Properties.Resources.food1, Settings.sizeCell, Settings.sizeCell);
            vaccineVisual = new Bitmap[Settings.visualFoodCount];
            vaccineVisual[0] = new Bitmap(Life.Properties.Resources.vaccine1, Settings.sizeCell, Settings.sizeCell);
            maskVisual = new Bitmap(Life.Properties.Resources.mask1, Settings.sizeCell, Settings.sizeCell);
            virusVisual = new Bitmap(Life.Properties.Resources.virus1, Settings.sizeCell, Settings.sizeCell);
        }
        public void generatePreview()
        {
            Bitmap temp = (Bitmap)clearImage.Clone();
            using (Graphics g = Graphics.FromImage(temp))
            {
                for (int x = 0; x < Settings.xCount; x++)
                {
                    for (int y = 0; y < Settings.yCount; y++)
                    {
                        Pen blackPen = new Pen(Color.Black, 3);
                        Rectangle rect = new Rectangle(x* Settings.sizeCell, y* Settings.sizeCell, Settings.sizeCell, Settings.sizeCell);
                        g.DrawRectangle(blackPen, rect);
                    }

                }
            }

            curentImageSourse = BitmapToImageSource(temp);
            //return curentImageSourse;
            //return temp;
        }
        public void generateImage(Colony colony)
        {
            Bitmap temp = (Bitmap)clearImage.Clone();
            using (Graphics g = Graphics.FromImage(temp))
            {
                foreach (People person in colony.people)
                {
                    Pen healthColor = new Pen(Color.Red, Settings.sizeBar);
                    Pen foodhColor = new Pen(Color.Brown, Settings.sizeBar);
                    Pen ageColor = new Pen(Color.Green, Settings.sizeBar);
                    Pen vaccineColor = new Pen(Color.Blue, Settings.sizeBar);
                    Pen virusColor = new Pen(Color.Purple, Settings.sizeBar);
                    double persentHealth = 1;
                    double persentFood = 1;
                    double persentAge = 1;
                    double persentVaccine = 1;
                    double persentVirus = 1;

                    if (!person.AtHouse)
                    {
                        g.DrawImage(personVisual[person.VisualType], person.X * Settings.sizeCell, person.Y * Settings.sizeCell);
                        if (person.HasMask)
                        {
                            g.DrawImage(maskVisual, person.X * Settings.sizeCell + 42, person.Y * Settings.sizeCell + 10, 18, 25);
                        }

                        persentHealth = person.Health * 1.0 / Settings.healthCellDefault;
                        persentFood = person.Feed * 1.0 / Settings.feedCellDefault;
                        persentAge = (Settings.maxAge - person.Old) * 1.0 / Settings.maxAge;
                        persentVaccine = person.VaccineProtection * 1.0 / Settings.persentVaccineProtection;
                        persentVirus = person.VirusStrength * 1.0 / ((100 * 1.0 / Settings.percentVirusGoDown) * 1.5);
                        g.DrawLine(ageColor, new Point(person.X * Settings.sizeCell, person.Y * Settings.sizeCell - Settings.barPadding), new Point((person.X * Settings.sizeCell) + Convert.ToInt32(Settings.sizeCell * persentAge), person.Y * Settings.sizeCell - Settings.barPadding));
                        g.DrawLine(healthColor, new Point(person.X * Settings.sizeCell, person.Y * Settings.sizeCell), new Point((person.X * Settings.sizeCell) + Convert.ToInt32(Settings.sizeCell * persentHealth), person.Y * Settings.sizeCell));
                        g.DrawLine(foodhColor, new Point(person.X * Settings.sizeCell, person.Y * Settings.sizeCell + Settings.barPadding), new Point((person.X * Settings.sizeCell) + Convert.ToInt32(Settings.sizeCell * persentFood), person.Y * Settings.sizeCell + Settings.barPadding));
                        g.DrawLine(vaccineColor, new Point(person.X * Settings.sizeCell, person.Y * Settings.sizeCell + (Settings.barPadding * 2)), new Point((person.X * Settings.sizeCell) + Convert.ToInt32(Settings.sizeCell * persentVaccine), person.Y * Settings.sizeCell + (Settings.barPadding * 2)));
                        g.DrawLine(virusColor, new Point(person.X * Settings.sizeCell, person.Y * Settings.sizeCell + (Settings.barPadding * 3)), new Point((person.X * Settings.sizeCell) + Convert.ToInt32(Settings.sizeCell * persentVirus), person.Y * Settings.sizeCell + (Settings.barPadding * 3)));
                    }


                }
                foreach (Virus virus in colony.viruses)
                {
                    g.DrawImage(virusVisual, virus.X * Settings.sizeCell, virus.Y * Settings.sizeCell);
                }
                foreach (House house in colony.houses)
                {
                    Pen houseFullColor = new Pen(Color.Red, Settings.sizeBar);
                    double persentHouseFull = house.CurentCells * 1.0 / house.MaxCells;
                    g.DrawImage(houseVisual[house.VisualType], house.X * Settings.sizeCell, house.Y * Settings.sizeCell);
                    g.DrawLine(houseFullColor, new Point(house.X * Settings.sizeCell, house.Y * Settings.sizeCell), new Point((house.X * Settings.sizeCell) + Convert.ToInt32(Settings.sizeCell * persentHouseFull), house.Y * Settings.sizeCell));
                }
                foreach (Food food in colony.foods)
                {
                    if (food.Vaccine)
                    {
                        g.DrawImage(vaccineVisual[food.VisualType], food.X * Settings.sizeCell, food.Y * Settings.sizeCell);
                    }
                    else
                    {
                        g.DrawImage(foodVisual[food.VisualType], food.X * Settings.sizeCell, food.Y * Settings.sizeCell);
                    }
                    
                }
            }
            
            curentImageSourse = BitmapToImageSource(temp);
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
