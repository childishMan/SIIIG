using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using siig.methods;
using visual;
using Xceed.Wpf.Toolkit;

namespace siig.Views_ViewModels
{
    /// <summary>
    /// Interaction logic for OperationsControl.xaml
    /// </summary>
    public partial class OperationsControl : UserControl, IMethod
    {
        public enum EMeths
        {
            Scale,
            Reverse,
            ShiftByTime,
            ExpandInTime,
            AddTwoSignals,
            MultiplyTwoSignals
        }

        private bool isSqueeze = false;

        private int Factor = 1;


        private string OuputString = "";
        private EMeths CurrentMethod = EMeths.Scale;

        private Dictionary<int, double> FirstSignal = new Dictionary<int, double>();
        private Dictionary<int, double> SecondSignal = new Dictionary<int, double>();
        private Dictionary<int, double> ResultSignal = new Dictionary<int, double>();

        private ChartValues<ObservablePoint> FirstSignalChartValues { get; set; }
        private ChartValues<ObservablePoint> SecondSignalChartValues { get; set; }
        private ChartValues<ObservablePoint> FinalSignalChartValues { get; set; }

        public SeriesCollection Collection { get; set; }

        public string NameOfView { get; set; } = "Signal Operations";

        public string[] MethodsList => new string[]
        {
            "Scale",
            "Reverse",
            "Shift by time",
            "Expand in time",
            "Add two signals",
            "Multiply two signals"
        };


        public OperationsControl()
        {
            InitializeComponent();

            FirstSignalChartValues = new ChartValues<ObservablePoint>();
            SecondSignalChartValues = new ChartValues<ObservablePoint>();
            FinalSignalChartValues = new ChartValues<ObservablePoint>();

            CreateSeries();

            DataContext = this;
        }

        public void Proceed()
        {
            Reset();

            if (FirstSignal.Count > 0 && IsAllChecked())
            {
                switch (CurrentMethod)
                {
                    case EMeths.Scale:
                        ResultSignal = Operations.Scale(FirstSignal, Factor);
                        break;
                    case EMeths.AddTwoSignals:
                        ResultSignal = Operations.AddTwoSignals(FirstSignal, SecondSignal);
                        break;
                    case EMeths.ExpandInTime:
                        ResultSignal = Operations.ExpandInTime(FirstSignal, Factor, !isSqueeze);
                        break;
                    case EMeths.MultiplyTwoSignals:
                        ResultSignal = Operations.Multiply(FirstSignal, SecondSignal);
                        break;
                    case EMeths.Reverse:
                        ResultSignal = Operations.Reverse(FirstSignal);
                        break;
                    case EMeths.ShiftByTime:
                        ResultSignal = Operations.ShiftByTime(FirstSignal, Factor);
                        break;
                    default:
                        break;
                }
            }

            foreach (var item in FirstSignal)
            {
                FirstSignalChartValues.Add(new ObservablePoint(item.Key, item.Value));
            }

            if (SecondSignalBlock.Visibility == Visibility.Visible)
            {

                foreach (var item in SecondSignal)
                {
                    SecondSignalChartValues.Add(new ObservablePoint(item.Key, item.Value));
                }

            }

            OuputString = "";
            foreach (var item in ResultSignal)
            {
                FinalSignalChartValues.Add(new ObservablePoint(item.Key, item.Value));
                OuputString += $"{item.Key};{item.Value:f1} ";
            }

            OutputSignal.Text = OuputString;
        }


        private void CreateSeries()
        {
            Collection = new SeriesCollection()
            {
                new ScatterSeries()
                {
                    Title = "First Signal",
                    Values = FirstSignalChartValues,
                    Fill=Brushes.LawnGreen
                },
                new ScatterSeries()
                {
                    Title="Result Signal",
                    Values = FinalSignalChartValues,
                    Fill = Brushes.OrangeRed
                },
                new ScatterSeries()
                {
                    Title="Second Signal",
                    Values = SecondSignalChartValues,
                    Fill=Brushes.Aqua
                }
            };

        }

        private EMeths StringToEnum(int s)
        {
            EMeths f = (EMeths)s;
            return f;
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
            var TemporaryString = Box.Text;
            if (String.IsNullOrEmpty(TemporaryString))
            {
                Box.BorderThickness = new Thickness(0);
                Box.Text = "example: 1;2 2;2 5;3";
                Box.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#90a4ae"));
            }
            else
            {
                if (inputParser.isCorrect(TemporaryString))
                {
                    Box.BorderThickness = new Thickness(0);
                    if (Box == InputFirstSignal)
                    {
                        FirstSignal = DictionaryWorker.Sort(inputParser.Parse(TemporaryString));
                        Box.Text = DictionaryWorker.ToString(FirstSignal);
                    }
                    else
                    {
                        SecondSignal = DictionaryWorker.Sort(inputParser.Parse(TemporaryString));
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

        private void RadioButton_OnChecked(object sender, RoutedEventArgs e)
        {
            var Btn = sender as RadioButton;

            if (Btn == ExpandRadioButton) isSqueeze = false;
            else isSqueeze = true;
        }

        private void FactorUpDown_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Factor = Convert.ToInt32((sender as IntegerUpDown).Value);
        }

        public bool IsAllChecked()
        {
            bool IsProceeded = true;
            if (FirstSignal.Count >= 0)
            {
                switch (CurrentMethod)
                {
                    case EMeths.Scale:
                        if (Factor < 0) IsProceeded = false;
                        break;
                    case EMeths.AddTwoSignals:
                        if (SecondSignal.Count == 0) IsProceeded = false;
                        break;
                    case EMeths.ExpandInTime:
                        if (Factor <= 0) IsProceeded = false;
                        break;
                    case EMeths.MultiplyTwoSignals:
                        if (SecondSignal.Count == 0) IsProceeded = false;
                        break;
                    case EMeths.ShiftByTime:
                        if (Factor == 0) IsProceeded = false;
                        break;
                    case EMeths.Reverse:
                        IsProceeded = true;
                        break;
                    default:
                        return false;
                }

                return IsProceeded;
            }
            else return false;
        }

        private void Reset()
        {
            FirstSignalChartValues.Clear();
            SecondSignalChartValues.Clear();
            FinalSignalChartValues.Clear();
        }

        private void SwitchBlocks(EMeths method)
        {
            switch (CurrentMethod)
            {
                case EMeths.Scale:
                    {

                        SettingsBlock.Visibility = Visibility.Visible;
                        SecondSignalBlock.Visibility = Visibility.Hidden;
                        ExpandStackPanel.Visibility = Visibility.Hidden;

                        FactorUpDown.Minimum = 1;
                        FactorUpDown.Value = 1;


                        SwapSize(true);

                        Grid.SetColumnSpan(FactorStackPanel, 2);
                        Grid.SetRow(OutputBlock, 3);

                        break;
                    }

                case EMeths.AddTwoSignals:
                    {

                        SettingsBlock.Visibility = Visibility.Hidden;
                        SecondSignalBlock.Visibility = Visibility.Visible;

                        SwapSize(true);
                        Grid.SetRow(OutputBlock, 3);

                        break;
                    }

                case EMeths.ExpandInTime:
                    {

                        SettingsBlock.Visibility = Visibility.Visible;
                        SecondSignalBlock.Visibility = Visibility.Hidden;
                        ExpandStackPanel.Visibility = Visibility.Visible;

                        FactorUpDown.Minimum = 1;
                        FactorUpDown.Value = 1;


                        SwapSize(true);

                        Grid.SetRow(OutputBlock, 3);
                        Grid.SetColumnSpan(FactorStackPanel, 1);
                        break;
                    }

                case EMeths.MultiplyTwoSignals:
                    {

                        SettingsBlock.Visibility = Visibility.Hidden;
                        SecondSignalBlock.Visibility = Visibility.Visible;

                        SwapSize(true);
                        Grid.SetRow(OutputBlock, 3);

                        break;
                    }
                case EMeths.Reverse:
                    {

                        SecondSignalBlock.Visibility = Visibility.Hidden;
                        SettingsBlock.Visibility = Visibility.Hidden;
                        SwapSize(false);

                        Grid.SetRow(OutputBlock, 2);
                        break;
                    }
                case EMeths.ShiftByTime:
                    {

                        SecondSignalBlock.Visibility = Visibility.Hidden;
                        SettingsBlock.Visibility = Visibility.Visible;
                        ExpandStackPanel.Visibility = Visibility.Hidden;

                        FactorUpDown.Minimum = -20;
                        FactorUpDown.Value = 1;

                        SwapSize(true);
                        Grid.SetRow(OutputBlock, 3);
                        break;
                    }

                default:
                    break;
            }

        }

        private void SwapSize(bool Straight)
        {
            if (Straight)
            {
                ThirdRow.Height = new GridLength(3, GridUnitType.Star);
                FourthRow.Height = new GridLength(2.5, GridUnitType.Star);
            }
            else
            {
                ThirdRow.Height = new GridLength(2.5, GridUnitType.Star);
                FourthRow.Height = new GridLength(3, GridUnitType.Star);
            }
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var Box = sender as ComboBox;
            string TempStr = Box.SelectedItem.ToString();
            int Cnt = 0;

            foreach (var str in MethodsList)
            {
                if (str == TempStr)
                {
                    CurrentMethod = StringToEnum(Cnt);
                    SwitchBlocks(CurrentMethod);
                    return;
                }

                Cnt++;
            }
        }

        private void OutputSignal_OnMouseEnter(object sender, MouseEventArgs e)
        {
            if (OuputString != "")
            {
                ToolTip tp = new ToolTip();
                tp.Placement = PlacementMode.Top;
                tp.Content = OuputString;
                OutputSignal.ToolTip = tp;
            }
        }
    }
}
