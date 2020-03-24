using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Defaults;
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
        public enum meths
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


        private meths CurrentMethod = meths.Scale;

        private Dictionary<int, double> FirstSignal = new Dictionary<int, double>();
        private Dictionary<int, double> SecondSignal = new Dictionary<int, double>();
        private Dictionary<int, double> ResultSignal = new Dictionary<int, double>();

        public ChartValues<ObservablePoint> FirstSignalChartValues { get; set; }
        public ChartValues<ObservablePoint> SecondSignalChartValues { get; set; }
        public ChartValues<ObservablePoint> FinalSignalChartValues { get; set; }

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

            DataContext = this;
        }

        public void Proceed()
        {
            Reset();

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

            foreach (var item in ResultSignal)
            {
                FinalSignalChartValues.Add(new ObservablePoint(item.Key, item.Value));
            }
        }



        private meths StringToEnum(int s)
        {
            meths f = (meths)s;
            return f;
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

        private void RadioButton_OnChecked(object sender, RoutedEventArgs e)
        {
            var btn = sender as RadioButton;

            if (btn == ExpandRadioButton) isSqueeze = false;
            else isSqueeze = true;
        }

        private void FactorUpDown_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Factor = Convert.ToInt32((sender as IntegerUpDown).Value);
        }

        public bool IsAllChecked()
        {
            bool IsProceeded = true;
            if (FirstSignal.Count > 0)
            {
                switch (CurrentMethod)
                {
                    case meths.Scale:
                        if (Factor > 0)
                            ResultSignal = Operations.Scale(FirstSignal, Factor);
                        else IsProceeded = false;
                        break;
                    case meths.AddTwoSignals:
                        if (SecondSignal.Count > 0)
                            ResultSignal = Operations.AddTwoSignals(FirstSignal, SecondSignal);
                        else IsProceeded = false;
                        break;
                    case meths.ExpandInTime:
                        if (Factor > 0)
                            ResultSignal = Operations.ExpandInTime(FirstSignal, Factor, !isSqueeze);
                        else IsProceeded = false;
                        break;
                    case meths.MultiplyTwoSignals:
                        if (SecondSignal.Count > 0)
                            ResultSignal = Operations.Multiply(FirstSignal, SecondSignal);
                        else IsProceeded = false;
                        break;
                    case meths.Reverse:
                        ResultSignal = Operations.Reverse(FirstSignal);
                        break;
                    case meths.ShiftByTime:
                        if (Factor != 0)
                            ResultSignal = Operations.ShiftByTime(FirstSignal, Factor);
                        else IsProceeded = false;
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

        private void SwitchBlocks(meths method)
        {
            switch (CurrentMethod)
            {
                case meths.Scale:
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

                case meths.AddTwoSignals:
                    {
                        SettingsBlock.Visibility = Visibility.Hidden;
                        SecondSignalBlock.Visibility = Visibility.Visible;

                        SwapSize(true);
                        Grid.SetRow(OutputBlock, 3);

                        break;
                    }

                case meths.ExpandInTime:
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

                case meths.MultiplyTwoSignals:
                    {
                        SettingsBlock.Visibility = Visibility.Hidden;
                        SecondSignalBlock.Visibility = Visibility.Visible;

                        SwapSize(true);
                        Grid.SetRow(OutputBlock, 3);

                        break;
                    }
                case meths.Reverse:
                    {
                        SecondSignalBlock.Visibility = Visibility.Hidden;
                        SettingsBlock.Visibility = Visibility.Hidden;
                        SwapSize(false);

                        Grid.SetRow(OutputBlock, 2);
                        break;
                    }
                case meths.ShiftByTime:
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

        private void SwapSize(bool straight)
        {
            if (straight)
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
            string s = Box.SelectedItem.ToString();
            int cnt = 0;

            foreach (var str in MethodsList)
            {
                if (str == s)
                {
                    CurrentMethod = StringToEnum(cnt);
                    SwitchBlocks(CurrentMethod);
                    return;
                }

                cnt++;
            }
        }

    }
}
