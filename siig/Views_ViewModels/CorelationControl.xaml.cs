using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Defaults;
using siig.Annotations;
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
            var box = sender as TextBox;

            if (box.Text == "index;garmonic")
            {
                box.Text = "";
            }

            box.BorderThickness = new Thickness(1);
            box.Foreground = Brushes.AliceBlue;
        }

        private void Input_OnMouseLeave(object sender, MouseEventArgs e)
        {
            var box = sender as TextBox;
            var str = box.Text;
            if (String.IsNullOrEmpty(str))
            {
                box.BorderThickness = new Thickness(0);
                box.Text = "index;garmonic";
                box.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#90a4ae"));
            }
            else
            {
                if (inputParser.isCorrect(str))
                {
                    box.BorderThickness = new Thickness(0);
                    if (box == InputFirstSignal)
                    {
                        FirstSignal = DictionaryWorker.Sort(inputParser.Parse(str));
                        box.Text = DictionaryWorker.ToString(FirstSignal);
                    }
                    else
                    {
                        SecondSignal = DictionaryWorker.Sort(inputParser.Parse(str));
                        box.Text = DictionaryWorker.ToString(SecondSignal);

                    }
                }
                else
                {
                    box.BorderThickness = new Thickness(1);
                    box.BorderBrush = Brushes.Red;
                }
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
                    var Output = "";

                    foreach (var item in FinalSignal)
                    {
                        Output += $"{item:F3} ";
                    }

                    OutputSignal.Text = Output;
                    MapListToChart();
                }
            }
        }

    }
}
