using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;

namespace Life
{
    class Graph
    {
        private const int widthBitmap = 1100;
        private const int heightBitmap = 200;
        private const int sizeLine = 3;

        private readonly List<int> countAlive;
        private readonly List<int> countInfected;
        private readonly List<int> countFood;

        private int maxCount;

        private readonly Bitmap clearImage;

        public Graph()
        {
            countAlive = new List<int>();
            countInfected = new List<int>();
            countFood = new List<int>();
            maxCount = 0;
            clearImage = new Bitmap(widthBitmap, heightBitmap);

            using (Graphics g = Graphics.FromImage(clearImage))
            {
                SolidBrush blueBrush = new SolidBrush(Color.White);
                Rectangle rect = new Rectangle(0, 0, widthBitmap, heightBitmap);
                g.FillRectangle(blueBrush, rect);
            }
        }

        public void AddAlive(int count)
        {
            if (maxCount < count)
                maxCount = count;

            countAlive.Add(count);
        }

        public void AddInfected(int count)
        {
            if (maxCount < count)
                maxCount = count;

            countInfected.Add(count);
        }

        public void AddFood(int count)
        {
            if (maxCount < count)
                maxCount = count;

            countFood.Add(count);
        }

        public int max(int a, int b)
        {
            if (a > b)
            {
                return a;
            }
            else
            {
                return b;
            }
        }

        public void DrawLine(Graphics g, int firstCount, int secondCount, Pen pen, int sizePart, int n)
        {
            Pen gridColor = new Pen(Color.Black, 1);
            int persentAlive1 = heightBitmap- Convert.ToInt32(firstCount * 1.0 / maxCount * heightBitmap);
            int persentAlive2 = heightBitmap - Convert.ToInt32(secondCount * 1.0 / maxCount * heightBitmap);
            g.DrawLine(pen, new Point(n*sizePart, persentAlive1), new Point((n+1) * sizePart, persentAlive2));
            g.DrawLine(gridColor, new Point(n * sizePart, 0), new Point(n  * sizePart, heightBitmap));
        }

        public BitmapImage GenerateImage()
        {
            Bitmap temp = (Bitmap)clearImage.Clone();
            using (Graphics g = Graphics.FromImage(temp))
            {
                Pen aliveColor = new Pen(Color.Red, sizeLine);
                Pen foodColor = new Pen(Color.Brown, sizeLine);
                Pen infectedColor = new Pen(Color.Purple, sizeLine);
                int countPoint = max(countAlive.Count(), countInfected.Count());
                countPoint = max(countPoint, countFood.Count());
                int sizePart = Convert.ToInt32( widthBitmap*1.0 / (countPoint - 1));
                //Debug.WriteLine("q:"+countPoint.ToString());

                for (int i = 0; i < countPoint-1; i++)
                {
                    DrawLine(g, countAlive[i], countAlive[i + 1], aliveColor, sizePart, i);
                    DrawLine(g, countInfected[i], countInfected[i + 1], infectedColor, sizePart, i);
                    DrawLine(g, countFood[i], countFood[i + 1], foodColor, sizePart, i);


                }

            }

            return BitmapToImageSource(temp);
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
