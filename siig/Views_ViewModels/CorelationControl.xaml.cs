using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Defaults;
using siig.models;
using siig.methods;
using visual;

namespace siig.Views_ViewModels
{
    /// <summary>
    /// Interaction logic for CorelationControl.xaml
    /// </summary>
    public partial class CorelationControl : UserControl, IMethod
    {
        private bool IsCrossCorelation = true;
        private bool Normalize = false;


        private Dictionary<int, double> FirstSignal = new Dictionary<int, double>();
        private Dictionary<int, double> SecondSignal = new Dictionary<int, double>();
        private List<double> FinalSignal = new List<double>();

        private string OutputString = "";

        public string NameOfView { get; set; } = "Corelation";


        public ChartValues<ObservablePoint> CorelationSignalChartValues { get; set; }



        public CorelationControl()
        {
            InitializeComponent();
            CorelationSignalChartValues = new ChartValues<ObservablePoint>();

            DataContext = this;
        }



        private void MethodButtonFired(object sender, RoutedEventArgs e)
        {
            if (SecondSignalBlock != null)
            {
                if (CrossCorelate.IsChecked == true)
                {
                    IsCrossCorelation = true;

                    SecondSignalBlock.Visibility = Visibility.Visible;
                    Grid.SetRow(OutputBlock, 3);
                }
                else
                {
                    IsCrossCorelation = false;

                    SecondSignalBlock.Visibility = Visibility.Hidden;
                    Grid.SetRow(OutputBlock,2);
                }
            }
        }

        private void NormalizeCheckChanged(object sender, RoutedEventArgs e)
        {
            Normalize = Convert.ToBoolean((sender as CheckBox).IsChecked);
        }

        private void Input_OnMouseEnter(object sender, MouseEventArgs e)
        {
            var Box = sender as TextBox;

            if (Box.Text == "example: 1;2 2;2 5;3")
            {
                Box.Text = "";
            }

            Box.BorderThickness = new Thickness(1);
            Box.Foreground = Brushes.AliceBlue;
        }

        private void Input_OnMouseLeave(object sender, MouseEventArgs e)
        {
            var Box = sender as TextBox;
            var TempString = Box.Text;
            if (String.IsNullOrEmpty(TempString))
            {
                Box.BorderThickness = new Thickness(0);
                Box.Text = "example: 1;2 2;2 5;3";
                Box.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#90a4ae"));
            }
            else
            {
                if (inputParser.isCorrect(TempString))
                {
                    Box.BorderThickness = new Thickness(0);
                    if (Box == InputFirstSignal)
                    {
                        FirstSignal = DictionaryWorker.Sort(inputParser.Parse(TempString));
                        Box.Text = DictionaryWorker.ToString(FirstSignal);
                    }
                    else
                    {
                        SecondSignal = DictionaryWorker.Sort(inputParser.Parse(TempString));
                        Box.Text = DictionaryWorker.ToString(SecondSignal);

                    }
                }
                else
                {
                    Box.BorderThickness = new Thickness(1);
                    Box.BorderBrush = Brushes.Red;
                }
            }
        }

        private void OutputSignal_OnMouseEnter(object sender, MouseEventArgs e)
        {
            if (OutputString != "")
            {
                ToolTip tp = new ToolTip();
                tp.Placement = PlacementMode.Top;
                tp.Content = OutputString;
                OutputSignal.ToolTip = tp;
            }
        }

        private void MapListToChart()
        {
            CorelationSignalChartValues.Clear();

            for (int i = 0; i < FinalSignal.Count; i++)
            {
                CorelationSignalChartValues.Add(new ObservablePoint(i,FinalSignal[i]));
            }
        }



        public bool IsAllChecked()
        {
            if (FirstSignal.Count > 0 && !IsCrossCorelation)
            {
                return true;
            }

            if (IsCrossCorelation && SecondSignal.Count > 0 && FirstSignal.Count>0)
            {
                return true;
            }
        return false;
        }

        public void Proceed()
        {
            if (IsAllChecked())
            {
                if (IsCrossCorelation)
                {
                    FinalSignal = Corelation.MutualCorealtion(FirstSignal, SecondSignal, Normalize);
                }
                else
                {
                    FinalSignal = Corelation.autoCorelation(FirstSignal, Normalize);
                }

                if (FinalSignal.Count > 0)
                {
                    OutputString = "";
                    foreach (var item in FinalSignal)
                    {
                        OutputString += $"{item:F3} ";
                    }

                    OutputSignal.Text = OutputString;
                    MapListToChart();
                }
            }
        }

    }
}
