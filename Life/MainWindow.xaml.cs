using Life.Core.Abstractions;
using Life.Core.Configuration;
using Life.Core.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Life
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IConfigManager<Config> _configManager;
        private readonly Config? _config;

        private Colony? colony;
        private Image? image;
        private Graph? graph;

        private readonly GridLength sizeGridHeight;
        private readonly DispatcherTimer timer;

        private int addGhraph = 0;

        public MainWindow()
        {
            InitializeComponent();

            _configManager = new JSONConfigManager(AssetsSettings.JSONSettingsFilename);
            _config = _configManager.LoadConfig().GetAwaiter().GetResult();

            if (_config == null)
            {
                throw new Exception("Unable to load config.");
            }

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
            graph.AddAlive(colony.CountType(1));
            graph.AddInfected(colony.CountType(7) + colony.CountType(5));
            graph.AddFood(colony.CountType(2));

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

            /*Settings.minNeighborToGenerateLive = TryConvert(minNeighborToGenerateLiveTextBox.Text);
            Settings.maxNeighborToGenerateLive = TryConvert(maxNeighborToGenerateLiveTextBox.Text);
            Settings.minNeighborToContinueLive = TryConvert(minNeighborToContinueLiveTextBox.Text);
            Settings.maxNeighborToContinueLive = TryConvert(maxNeighborToContinueLiveTextBox.Text);
            Settings.persentToGenerateNewLife = TryConvert(persentToGenerateNewLifeTextBox.Text);
            Settings.persentToContinueLife = TryConvert(persentToContinueLifeTextBox.Text);
            Settings.persentToInfectedFromAir = TryConvert(persentToInfectedFromAirTextBox.Text);
            Settings.persentToInfectedNeighbor = TryConvert(persentToInfectedNeighborTextBox.Text);
            Settings.persentToInfectedDie = TryConvert(persentToInfectedDieTextBox.Text);
            Settings.persentToInfectedAlive = TryConvert(persentToInfectedAliveTextBox.Text);

            Settings.move = TryConvert(moveTextBox.Text);
            Settings.wolfChild = (bool)wolfChildCheckBox.IsChecked;
            Settings.wolfHealth = TryConvert(wolfHealthTextBox.Text);
            Settings.wolfTeenager = TryConvert(wolfTeenagerTextBox.Text);
            Settings.wolfTeenagerPersent = TryConvert(wolfTeenagerPersentTextBox.Text);
            Settings.rabbitHealth = TryConvert(rabbitHealthTextBox.Text);
            Settings.rabbitTeenager = TryConvert(rabbitTeenagerTextBox.Text);
            Settings.rabbitTeenagerPersent = TryConvert(rabbitTeenagerPersentTextBox.Text);*/
        }
        public void updateSettings()
        {
            loadAdditionalSettings();
            graph = new Graph();
            colony = new Colony(_config.ColumnsCount, _config.RowsCount, _configManager);
            image = new Image(_config.ColumnsCount * AssetsSettings.CellSizePX, colony.RowsCount * AssetsSettings.CellSizePX, _config);
            image.GeneratePreview();
            mainImage.Source = image.CurrentImageSource;
        }

        private void SizeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Cell size px - константа, нужно чото придумать
            // AssetsSettings.CellSizePX = TryConvert(sizeTextBox.Text);
            // updateSettings();
        }

        private void YTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _config.RowsCount = TryConvert(yTextBox.Text);
            updateSettings();
        }

        public int TryConvert(string text)
        {
            try
            {
                int tempInt = Convert.ToInt32(text);
                if (tempInt < 1)
                {
                    tempInt = 1;
                }
                return tempInt;
            }
            catch
            {
                return 10;
            }
        }

        private void XTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            _config.ColumnsCount = TryConvert(xTextBox.Text);
            updateSettings();
        }

        private void DebugButton_Click(object sender, RoutedEventArgs e)
        {
            colony.UpdateColony();

            image.GenerateImage(colony);
            mainImage.Source = image.CurrentImageSource;
        }

        private void timer_Tick(object? sender, EventArgs e)
        {
            if (addGhraph == 2)
            {
                addGhraph = 0;
                graph.AddAlive(colony.CountType(1));
                graph.AddInfected(colony.CountType(7) + colony.CountType(5));
                graph.AddFood(colony.CountType(2));
                graphImage.Source = graph.GenerateImage();
            }
            else
            {
                addGhraph++;
            }


            colony.UpdateColony();

            image.GenerateImage(colony);
            mainImage.Source = image.CurrentImageSource;
        }

        /*public void iterLive()
        {
            Thread.Sleep(500);
            colony.UpdateColony();
            image.GenerateImage(colony);
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
            int x = Convert.ToInt32(Math.Truncate(clickedPoint.X / (mainImage.RenderSize.Width / _config.ColumnsCount)));
            int y = Convert.ToInt32(Math.Truncate(clickedPoint.Y / (mainImage.RenderSize.Height / _config.RowsCount)));

            int typeCell = colony.GetTypeByCoord(x, y);
            if (typeCell == 0)
            {
                colony.AddPeopleByCoord(x, y);
            }
            else if ((typeCell == 1) || (typeCell == 5))
            {
                colony.RemovePersonByCoord(x, y);
                colony.AddFoodByCoord(x, y, false);

            }
            else if (typeCell == 2)
            {
                colony.RemoveFoodByCoord(x, y);
                colony.AddFoodByCoord(x, y, true);

            }
            else if (typeCell == 3)
            {
                colony.RemoveFoodByCoord(x, y);
                colony.AddHouseByCoord(x, y);

            }
            else if (typeCell == 4)
            {
                colony.RemoveHouseByCoord(x, y);
                colony.AddVirusByCoord(x, y, _config.MovesToVirusDeath);
            }
            else if (typeCell == 8)
            {
                colony.RemoveVirusByCoord(x, y);

            }
            image.GenerateImage(colony);
            mainImage.Source = image.CurrentImageSource;
            //int y = Convert.ToInt32(Math.Truncate(clickedPoint.X / (mainImage.Width/ Settings.xCount)));
            //MessageBox.Show(x.ToString());
        }



        private void mainImage_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            //MessageBox.Show("qq");
        }
    }
}
