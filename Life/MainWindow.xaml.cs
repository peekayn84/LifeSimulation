using Life.Core.Abstractions;
using Life.Core.Configuration;
using Life.Core.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using ScottPlot.Plottable;

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
        private int updateLogic = 0;

        double[] Healthy = new double[200];
        double[] Infected = new double[200];
        double[] Food = new double[200];

        private Crosshair _crosshair;

        public MainWindow()
        {
            InitializeComponent();

            _configManager = new JSONConfigManager("config.json");
            _config = _configManager.LoadConfig();

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
            //graphImage.Stretch = Stretch.Fill;
            typeComboBox.Items.Add("1 - Standart");
            typeComboBox.Items.Add("2 - Infected");
            typeComboBox.Items.Add("3 - Wolf and rabbit");
            typeComboBox.SelectedIndex = 0;
            typeComboBox.SelectionChanged += TypeComboBox_SelectionChanged;
            sizeGridHeight = grindTable.RowDefinitions[1].Height;
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            updateSettings();

            //live = new Thread(new ThreadStart(iterLive));

            timer.Tick += UpdateGraph;

            timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            wpfPlot1.Plot.AddSignal(Healthy, 1, System.Drawing.Color.Green, label: "Healthy").FillAboveAndBelow(System.Drawing.Color.Green, System.Drawing.Color.Green);
            wpfPlot1.Plot.AddSignal(Infected, 1, System.Drawing.Color.Purple, label: "Infected").FillAboveAndBelow(System.Drawing.Color.Purple, System.Drawing.Color.Purple);
            wpfPlot1.Plot.AddSignal(Food, 1, System.Drawing.Color.Blue, label: "Food").FillAboveAndBelow(System.Drawing.Color.Blue, System.Drawing.Color.Blue);

            wpfPlot1.Plot.SetAxisLimits(yMax: 20);
            wpfPlot1.Plot.XAxis.Label("Time ->");
            wpfPlot1.MouseMove += FormsPlot_MouseMove;

            wpfPlot1.Refresh();

            wpfPlot1.Plot.Legend(location: ScottPlot.Alignment.UpperLeft);
        }

        private void FormsPlot_MouseMove(object sender, MouseEventArgs e)
        {
            wpfPlot1.Plot.Remove(_crosshair);
            (double x, double y) = wpfPlot1.GetMouseCoordinates();
            _crosshair = wpfPlot1.Plot.AddCrosshair(x, y);
            wpfPlot1.Render();
        }

        void UpdateGraph(object sender, EventArgs e)
        {
            if (cbHealthy.IsChecked == true)
            {
                Array.Copy(Healthy, 1, Healthy, 0, Healthy.Length - 1);
                double nextValue1 = Convert.ToDouble(colony.CountType(1));
                Healthy[Healthy.Length - 1] = nextValue1;
            }
            else
            {
                Array.Copy(Healthy, 1, Healthy, 0, Healthy.Length - 1);
                Healthy[Healthy.Length - 1] = 0;
            }

            if (cbInfected.IsChecked == true)
            {
                Array.Copy(Infected, 1, Infected, 0, Infected.Length - 1);
                double nextValue2 = Convert.ToDouble(colony.CountType(7) + colony.CountType(5));
                Infected[Infected.Length - 1] = nextValue2;
            }
            else
            {
                Array.Copy(Infected, 1, Infected, 0, Infected.Length - 1);
                Infected[Healthy.Length - 1] = 0;
            }

            if (cbFood.IsChecked == true)
            {
                Array.Copy(Food, 1, Food, 0, Food.Length - 1);
                double nextValue3 = Convert.ToDouble(colony.CountType(2));
                Food[Food.Length - 1] = nextValue3;
            }
            else
            {
                Array.Copy(Food, 1, Food, 0, Food.Length - 1);
                Food[Healthy.Length - 1] = 0;
            }



            wpfPlot1.Refresh();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            timer.Stop();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            graph.AddAlive(colony.CountType(1));
            graph.AddInfected(colony.CountType(7) + colony.CountType(5));
            graph.AddFood(colony.CountType(2));

            timer.Start();
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

        public void updateSettings()
        {
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
            if (updateLogic == 5)
            {
                updateLogic = 0;
                if (addGhraph == 2)
                {
                    addGhraph = 0;
                    graph.AddAlive(colony.CountType(1));
                    graph.AddInfected(colony.CountType(7) + colony.CountType(5));
                    graph.AddFood(colony.CountType(2));
                    //graphImage.Source = graph.GenerateImage();
                }
                else
                {
                    addGhraph++;
                }

                image.GenerateImage(colony, 100);
                mainImage.Source = image.CurrentImageSource;
                colony.UpdateColony();


            }
            else
            {
                image.GenerateImage(colony, updateLogic*20);
                mainImage.Source = image.CurrentImageSource;
                updateLogic++;
            }

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

        private void wpfPlot1_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
