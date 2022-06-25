using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Life
{
    class Graph
    {
        const int widthBitmap = 1100;
        const int heightBitmap = 200;
        const int sizeLine = 3;
        List<int> countAlive;
        List<int> countInfected;
        List<int> countFood;
        int maxCount;
        Bitmap clearImage;
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
        public void addAlive(int count)
        {
            if (maxCount < count)
                maxCount = count;
            countAlive.Add(count);
        }        
        public void addInfected(int count)
        {
            if (maxCount < count)
                maxCount = count;
            countInfected.Add(count);
        }        
        public void addFood(int count)
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
        public void drawLine(Graphics g, int firstCount, int secondCount, Pen pen, int sizePart, int n)
        {
            Pen gridColor = new Pen(Color.Black, 1);
            int persentAlive1 = heightBitmap- Convert.ToInt32(firstCount * 1.0 / maxCount * heightBitmap);
            int persentAlive2 = heightBitmap - Convert.ToInt32(secondCount * 1.0 / maxCount * heightBitmap);
            g.DrawLine(pen, new Point(n*sizePart, persentAlive1), new Point((n+1) * sizePart, persentAlive2));
            g.DrawLine(gridColor, new Point(n * sizePart, 0), new Point(n  * sizePart, heightBitmap));
        }
        public BitmapImage generateImage()
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
                Debug.WriteLine(countPoint);
                for (int i = 0; i < countPoint-1; i++)
                {
                    drawLine(g, countAlive[i], countAlive[i + 1], aliveColor, sizePart, i);
                    drawLine(g, countInfected[i], countInfected[i + 1], infectedColor, sizePart, i);
                    drawLine(g, countFood[i], countFood[i + 1], foodColor, sizePart, i);


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
