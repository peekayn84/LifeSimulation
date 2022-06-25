using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Life
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Colony colony;
        Image image;
        GridLength sizeGridHeight;
        DispatcherTimer timer;
        int addGhraph = 0;
        Graph graph;



        public MainWindow()
        {
            InitializeComponent();
            startButton.Click += StartButton_Click;
            stopButton.Click += StopButton_Click;
            xTextBox.TextChanged += XTextBox_TextChanged;
            yTextBox.TextChanged += YTextBox_TextChanged;
            sizeTextBox.TextChanged += SizeTextBox_TextChanged;
            debugButton.Click += DebugButton_Click;
            mainImage.MouseDown += MainImage_MouseDown;
            mainImage.Stretch = Stretch.Fill;
            graphImage.Stretch = Stretch.Fill;
            typeComboBox.Items.Add("1 - Standart");
            typeComboBox.Items.Add("2 - Infected");
            typeComboBox.Items.Add("3 - Wolf and rabbit");
            typeComboBox.SelectedIndex = 0;
            typeComboBox.SelectionChanged += TypeComboBox_SelectionChanged;
            sizeGridHeight = grindTable.RowDefinitions[1].Height;
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            updateSettings();

            //live = new Thread(new ThreadStart(iterLive));
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {

            graph.addAlive(colony.countType(1));
            graph.addInfected(colony.countType(7) + colony.countType(5));
            graph.addFood(colony.countType(2));

            loadAdditionalSettings();
            timer.Start();
        }

        private void TypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*if (typeComboBox.SelectedIndex==0)
            {
                Settings.type = 0;
                grindTable.RowDefinitions[1].Height = sizeGridHeight;
                grindTable.RowDefinitions[2].Height = sizeGridHeight;
                grindTable.RowDefinitions[3].Height = new GridLength(0);
                grindTable.RowDefinitions[4].Height = new GridLength(0);

            }else if (typeComboBox.SelectedIndex == 1)
            {
                Settings.type = 1;
                grindTable.RowDefinitions[1].Height = sizeGridHeight;
                grindTable.RowDefinitions[2].Height = sizeGridHeight;
                grindTable.RowDefinitions[3].Height = new GridLength(0);
                grindTable.RowDefinitions[4].Height = new GridLength(0);
            }
            else
            {
                Settings.type = 2;
                grindTable.RowDefinitions[3].Height = sizeGridHeight;
                grindTable.RowDefinitions[4].Height = sizeGridHeight;
                grindTable.RowDefinitions[1].Height = new GridLength(0);
                grindTable.RowDefinitions[2].Height = new GridLength(0);
            }*/
        }
        public void loadAdditionalSettings()
        {

            /*Settings.minNeighborToGenerateLive = tryConvet(minNeighborToGenerateLiveTextBox.Text);
            Settings.maxNeighborToGenerateLive = tryConvet(maxNeighborToGenerateLiveTextBox.Text);
            Settings.minNeighborToContinueLive = tryConvet(minNeighborToContinueLiveTextBox.Text);
            Settings.maxNeighborToContinueLive = tryConvet(maxNeighborToContinueLiveTextBox.Text);
            Settings.persentToGenerateNewLife = tryConvet(persentToGenerateNewLifeTextBox.Text);
            Settings.persentToContinueLife = tryConvet(persentToContinueLifeTextBox.Text);
            Settings.persentToInfectedFromAir = tryConvet(persentToInfectedFromAirTextBox.Text);
            Settings.persentToInfectedNeighbor = tryConvet(persentToInfectedNeighborTextBox.Text);
            Settings.persentToInfectedDie = tryConvet(persentToInfectedDieTextBox.Text);
            Settings.persentToInfectedAlive = tryConvet(persentToInfectedAliveTextBox.Text);

            Settings.move = tryConvet(moveTextBox.Text);
            Settings.wolfChild = (bool)wolfChildCheckBox.IsChecked;
            Settings.wolfHealth = tryConvet(wolfHealthTextBox.Text);
            Settings.wolfTeenager = tryConvet(wolfTeenagerTextBox.Text);
            Settings.wolfTeenagerPersent = tryConvet(wolfTeenagerPersentTextBox.Text);
            Settings.rabbitHealth = tryConvet(rabbitHealthTextBox.Text);
            Settings.rabbitTeenager = tryConvet(rabbitTeenagerTextBox.Text);
            Settings.rabbitTeenagerPersent = tryConvet(rabbitTeenagerPersentTextBox.Text);*/
        }
        public void updateSettings()
        {
            image = null;
            colony = null;
            loadAdditionalSettings();
            graph = new Graph();
             colony = new Colony(Settings.xCount, Settings.yCount);
            image = new Image(colony.xCount * Settings.sizeCell, colony.yCount * Settings.sizeCell);
            image.generatePreview();
            mainImage.Source = image.curentImageSourse;

        }
        private void SizeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Settings.sizeCell = tryConvet(sizeTextBox.Text);
            updateSettings();
        }

        private void YTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Settings.yCount = tryConvet(yTextBox.Text);
            updateSettings();
        }

        public int tryConvet(string text)
        {
            try
            {
                int tempInt = Convert.ToInt32(text);
                if (tempInt < 1)
                {
                    tempInt = 1;
                }
                return tempInt;
            }catch(Exception ex)
            {
                return 10;
            }
        }
        private void XTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            Settings.xCount = tryConvet(xTextBox.Text);
            updateSettings();
        }

        private void DebugButton_Click(object sender, RoutedEventArgs e)
        {
            colony.updateColony();

            image.generateImage(colony);
            mainImage.Source = image.curentImageSourse;
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            if (addGhraph == 2)
            {
                addGhraph = 0;
                graph.addAlive(colony.countType(1));
                graph.addInfected(colony.countType(7) + colony.countType(5));
                graph.addFood(colony.countType(2));
                graphImage.Source = graph.generateImage();
            }
            else
            {
                addGhraph++;
            }

            
            colony.updateColony();

            image.generateImage(colony);
            mainImage.Source = image.curentImageSourse;
        }

        /*public void iterLive()
        {
            Thread.Sleep(500);
            colony.updateColony();
            image.generateImage(colony);
            Dispatcher.Invoke(new Action(() =>
            {
                lock (bitmapConveter)
                {
                    mainImage.Source = bitmapConveter;
                }
                
            }));
            mainImage.Dispatcher.Invoke(new Action(() =>
            {
                BitmapImage tmp = image.curentImageSourse;
                mainImage.Source = tmp;

            }));
            BitmapImage tmp = image.curentImageSourse;
            this.Dispatcher.BeginInvoke(new Action(() =>
            {

                
                mainImage.Source = tmp;

            }));

        }*/
        private void MainImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var clickedPoint = e.GetPosition((IInputElement)sender);
            int x = Convert.ToInt32(Math.Truncate(clickedPoint.X / (mainImage.RenderSize.Width / Settings.xCount)));
            int y = Convert.ToInt32(Math.Truncate(clickedPoint.Y / (mainImage.RenderSize.Height / Settings.yCount)));
            int index = -1;
            int typeCell = colony.getTypeByCoord(x, y);
            if (typeCell == 0)
            {
                colony.addPeopleByCoord(x, y);
            }else if ((typeCell == 1)|| (typeCell == 5)){
                colony.removePersonByCoord(x, y);
                colony.addFoodByCoord(x, y, false);

            }else if (typeCell == 2){
                colony.removeFoodByCoord(x, y);
                colony.addFoodByCoord(x, y, true);

            }else if (typeCell == 3){
                colony.removeFoodByCoord(x, y);
                colony.addHouseByCoord(x, y);

            }else if (typeCell == 4){
                colony.removeHouseByCoord(x, y);
                colony.addVirusByCoord(x, y, Settings.virusCellRemoveAfterDie);
            }else if (typeCell == 8){
                colony.removeVirusByCoord(x, y);

            }
            image.generateImage(colony);
            mainImage.Source = image.curentImageSourse;
            //int y = Convert.ToInt32(Math.Truncate(clickedPoint.X / (mainImage.Width/ Settings.xCount)));
            //MessageBox.Show(x.ToString());
        }



        private void mainImage_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            
            //MessageBox.Show("qq");
        }
    }
}
