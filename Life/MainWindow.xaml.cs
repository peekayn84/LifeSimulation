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
using System.Diagnostics;

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

        private readonly GridLength sizeGridHeight;
        private readonly DispatcherTimer timer;

        private int updateLogic = 0;

        double[] Healthy = new double[200];
        double[] Infected = new double[200];
        double[] Food = new double[200];
        double[] AvgAge = new double[200];
        

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
            clearButton.Click += ClearButton_Click;
            oneStepButton.Click += OneStepButton_Click;
            closeButton.Click += CloseButton_Click;
            mainImage.MouseDown += MainImage_MouseDown;
            cbInfected.Click += CbInfected_Checked;
            cbFood.Click += CbFood_Checked;
            cbHealthy.Click += CbHealthy_Checked;
            cbAvgAge.Click += CbAvgAge_Click;
            mainImage.Stretch = Stretch.Fill;
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 70);
            initSettings();

            wpfPlot1.Plot.AddSignal(Healthy, 1, System.Drawing.Color.Green, label: "Healthy").FillAboveAndBelow(System.Drawing.Color.Green, System.Drawing.Color.Green);
            wpfPlot1.Plot.AddSignal(Infected, 1, System.Drawing.Color.Purple, label: "Infected").FillAboveAndBelow(System.Drawing.Color.Purple, System.Drawing.Color.Purple);
            wpfPlot1.Plot.AddSignal(Food, 1, System.Drawing.Color.Blue, label: "Food").FillAboveAndBelow(System.Drawing.Color.Blue, System.Drawing.Color.Blue);
            wpfPlot1.Plot.AddSignal(AvgAge, 1, System.Drawing.Color.Red, label: "Avg Age").FillAboveAndBelow(System.Drawing.Color.Red, System.Drawing.Color.Red);
            wpfPlot1.Plot.SetAxisLimits(yMax: 20);
            wpfPlot1.Plot.XAxis.Label("Time ->");
            wpfPlot1.MouseMove += FormsPlot_MouseMove;
            wpfPlot1.Refresh();
            wpfPlot1.Plot.Legend(location: ScottPlot.Alignment.UpperLeft);
            wpfPlot1.Visibility = Visibility.Hidden;
        }

        private void CbAvgAge_Click(object sender, RoutedEventArgs e)
        {
            updateVisibleGraph();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            this.Close();
        }

        public void updateVisibleGraph()
        {
            bool result = false;
            if (cbFood.IsChecked == true)
                result = true;            
            if (cbHealthy.IsChecked == true)
                result = true;            
            if (cbInfected.IsChecked == true)
                result = true;            
            if (cbAvgAge.IsChecked == true)
                result = true;
            if (result)
            {
                wpfPlot1.Visibility = Visibility.Visible;
            }
            else
            {
                wpfPlot1.Visibility= Visibility.Hidden;
            }

        }
        private void CbHealthy_Checked(object sender, RoutedEventArgs e)
        {
            updateVisibleGraph();
        }

        private void CbFood_Checked(object sender, RoutedEventArgs e)
        {
            updateVisibleGraph();
        }

        private void CbInfected_Checked(object sender, RoutedEventArgs e)
        {
            updateVisibleGraph();
        }

        private void OneStepButton_Click(object sender, RoutedEventArgs e)
        {
            updateGUI();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            initSettings();
        }

        private void FormsPlot_MouseMove(object sender, MouseEventArgs e)
        {
            wpfPlot1.Plot.Remove(_crosshair);
            (double x, double y) = wpfPlot1.GetMouseCoordinates();
            _crosshair = wpfPlot1.Plot.AddCrosshair(x, y);
            wpfPlot1.Render();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Start();
        }

      
        public void initSettings()
        {
            colony = new Colony(_config.ColumnsCount, _config.RowsCount, _configManager);
            image = new Image(_config.ColumnsCount * AssetsSettings.CellSizePX, colony.RowsCount * AssetsSettings.CellSizePX, _config);
            image.GeneratePreview();
            mainImage.Source = image.CurrentImageSource;
        }

        public void updateGUI()
        {
            
            image.GenerateImage(colony);
            mainImage.Source = image.CurrentImageSource;
            
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
            if (cbAvgAge.IsChecked == true)
            {
                Debug.WriteLine(colony.GetAvgAge());
                Array.Copy(AvgAge, 1, AvgAge, 0, AvgAge.Length - 1);
                double nextValue3 = Convert.ToDouble(colony.GetAvgAge());
                AvgAge[AvgAge.Length - 1] = nextValue3;
            }
            else
            {
                Array.Copy(AvgAge, 1, AvgAge, 0, AvgAge.Length - 1);
                AvgAge[AvgAge.Length - 1] = 0;
            }
            wpfPlot1.Refresh();
            colony.UpdateColony();
        }

     

      



        private void timer_Tick(object? sender, EventArgs e)
        {
            if (updateLogic == 5)
            {
                updateLogic = 0;
                updateGUI();
            }
            else
            {
                image.GenerateImage(colony, updateLogic*20);
                mainImage.Source = image.CurrentImageSource;
                updateLogic++;
            }

        }

      
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
        }

    }
}
