﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using CustomControls;
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
        private Dictionary<int, double> FinalSignal = new Dictionary<int, double>();

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
                        FinalSignal = Operations.Scale(FirstSignal, Factor);
                        break;
                    case EMeths.AddTwoSignals:
                        FinalSignal = Operations.AddTwoSignals(FirstSignal, SecondSignal);
                        break;
                    case EMeths.ExpandInTime:
                        FinalSignal = Operations.ExpandInTime(FirstSignal, Factor, !isSqueeze);
                        break;
                    case EMeths.MultiplyTwoSignals:
                        FinalSignal = Operations.Multiply(FirstSignal, SecondSignal);
                        break;
                    case EMeths.Reverse:
                        FinalSignal = Operations.Reverse(FirstSignal);
                        break;
                    case EMeths.ShiftByTime:
                        FinalSignal = Operations.ShiftByTime(FirstSignal, Factor*-1);
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
            foreach (var item in FinalSignal)
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
                new ColumnSeries()
                {
                    Title="Result Signal",
                    Values = FinalSignalChartValues,
                    Fill = Brushes.OrangeRed
                },
                new ScatterSeries()
                {
                    Title = "First Signal",
                    Values = FirstSignalChartValues,
                    Fill=Brushes.LawnGreen
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
                      //  ExpandStackPanel.Visibility = Visibility.Hidden;

                        FactorUpDown.Minimum = 1;
                        FactorUpDown.Value = 1;


                        SwapSize(true);

                       // Grid.SetColumnSpan(FactorStackPanel, 2);
                        Grid.SetRow(OutputBlock, 3);

                        break;
                    }

                case EMeths.AddTwoSignals:
                    {

                        SettingsBlock.Visibility = Visibility.Hidden;
                        SecondSignalBlock.Visibility = Visibility.Visible;

                        SwapSize(false);
                        Grid.SetRow(OutputBlock, 3);

                        break;
                    }

                case EMeths.ExpandInTime:
                    {

                        SettingsBlock.Visibility = Visibility.Visible;
                        SecondSignalBlock.Visibility = Visibility.Hidden;
                      //  ExpandStackPanel.Visibility = Visibility.Visible;

                        FactorUpDown.Minimum = 1;
                        FactorUpDown.Value = 1;


                        SwapSize(true);

                        Grid.SetRow(OutputBlock, 3);
                       // Grid.SetColumnSpan(FactorStackPanel, 1);
                        break;
                    }

                case EMeths.MultiplyTwoSignals:
                    {

                        SettingsBlock.Visibility = Visibility.Hidden;
                        SecondSignalBlock.Visibility = Visibility.Visible;

                        SwapSize(false);
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
                        //ExpandStackPanel.Visibility = Visibility.Hidden;

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
            if (!Straight)
            {
                SettingsBlock.Visibility = Visibility.Hidden;
                SecondSignalBlock.Visibility = Visibility.Visible;
            }
            else
            {
                SettingsBlock.Visibility = Visibility.Visible;
                SecondSignalBlock.Visibility = Visibility.Hidden;
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

        private void InputSecondSignal_OnLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            try
            {
                SecondSignal = MyConverter.ListToDictionary(inputParser.ParseToList((sender as InputBox).Text));
            }
            catch (Exception ex)
            {

            }
        }

        private void InputFirstSignal_OnLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            try
            {
                FirstSignal = MyConverter.ListToDictionary(inputParser.ParseToList((sender as InputBox).Text));
            }
            catch (Exception ex)
            {

            }
        }
    }
}
