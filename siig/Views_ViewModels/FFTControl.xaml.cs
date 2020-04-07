using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using siig.methods;
using siig.models;
using visual;

namespace siig.Views_ViewModels
{
    /// <summary>
    /// Interaction logic for FFT.xaml
    /// </summary>
    public partial class FFTControl : UserControl, IMethod
    {

        public string NameOfView { get; set; } = "f f t";

        public SeriesCollection Collection { get; set; } = new SeriesCollection();

        private ChartValues<ObservableValue> ChartValues = new ChartValues<ObservableValue>();

        private List<double> Signal = new List<double>();
        private List<double> Magnitude = new List<double>();
        private List<double> Phase = new List<double>();
        private List<complex> Result = new List<complex>();

        public string OutputString { get; set; } = "FFT Output";


        private bool IsForward = true;


        private enum Charts
        {
            Signal = 0,
            Magnitude,
            Phase
        }

        private Charts CurrentChartType = (Charts)0;

        public FFTControl()
        {
            InitializeComponent();

            Output.Text = OutputString;


            DataContext = this;
        }

        private void BindSeries()
        {
            Collection.Clear();

            switch (CurrentChartType)
            {
                case Charts.Signal:
                    {
                        var ChartSeries = new LineSeries();
                        ChartSeries.Title = "Signal";
                        ChartSeries.Stroke = Brushes.GreenYellow;

                        BindSignal();


                        ChartSeries.Values = ChartValues;

                        Collection.Add(ChartSeries);
                        break;
                    }

                case Charts.Magnitude:
                    {
                        var ChartSeries = new LineSeries();
                        ChartSeries.Title = "Magnitude";
                        ChartSeries.Stroke = Brushes.Magenta;

                        BindMagnitude();

                        ChartSeries.Values = ChartValues;

                        Collection.Add(ChartSeries);
                        break;
                    }
                case Charts.Phase:
                    {
                        var ChartSeries = new ColumnSeries();

                        ChartSeries.Title = "Phase";
                        ChartSeries.Stroke = Brushes.OrangeRed;

                        BindPhase();

                        ChartSeries.Values = ChartValues;

                        Collection.Add(ChartSeries);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

     
        }

        private void BindSignal()
        {
            ChartValues.Clear();

            Signal.Add(0);
            Signal.Add(0);
            Signal.Add(0);

            foreach (var item in Signal)
            {
                ChartValues.Add(new ObservableValue(item));
            }
        }

        private void BindMagnitude()
        {
            ChartValues.Clear();

            GetMagnitude();

            foreach (var item in Magnitude)
            {
                ChartValues.Add(new ObservableValue(item));
            }
        }

        private void GetMagnitude()
        {
            Magnitude.Clear();
            if (Result.Count != 0)
            {
                foreach (var item in Result)
                {
                    Magnitude.Add(item.Magnitude);
                }
                Magnitude.Add(0);
                Magnitude.Add(0);
                Magnitude.Add(0);
            }
        }

        private void BindPhase()
        {
            ChartValues.Clear();

            GetPhase();

            foreach (var item in Phase)
            {
                ChartValues.Add(new ObservableValue(item));
            }
        }

        private void GetPhase()
        {
            Phase.Clear();
            if (Result.Count != 0)
            {
                foreach (var item in Result)
                {
                    Phase.Add(item.Phase);
                }

                Phase.Add(0);
                Phase.Add(0);
                Phase.Add(0);
            }
        }


        public bool IsAllChecked()
        {
            if (Signal.Count != 0)
                return true;
            else
                return false;
        }

        public void Proceed()
        {

            if (IsAllChecked())
            {
                OutputString = "";

                var InputSignal = ComplexConverter.FromList(Signal);

                if (IsForward)
                    Result = FFT.ForwardFourierTransform(InputSignal);
                else
                    Result = FFT.InverseFourierTransform(InputSignal);

                foreach (var item in Result)
                {
                    OutputString += item.ToString() + "\n";
                }

                BindSeries();

                Output.Text = OutputString;
            }


        }

        private void Method_OnChecked(object sender, RoutedEventArgs e)
        {
            var Method = sender as RadioButton;

            if (Method == ForwardFFT)
            {
                IsForward = true;
            }
            else
            {
                IsForward = false;
            }
        }

        private void ChartType_OnChecked(object sender, RoutedEventArgs e)
        {
            var Type = sender as RadioButton;

            if (Type == ShowSignal)
            {
                CurrentChartType = Charts.Signal;
                return;
            }

            if (Type == ShowMagnitude)
            {
                CurrentChartType = Charts.Magnitude;
                return;
            }

            if (Type == ShowPhase)
            {
                CurrentChartType = Charts.Phase;
                return;
            }

            return;
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
            if (String.IsNullOrEmpty(TempString) || TempString== "example: 1;2 2;2 5;3")
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
                    var res = DictionaryWorker.Sort(inputParser.Parse(TempString));
                    Signal = MyConverter.DictionaryToList(res);
                    Box.Text = DictionaryWorker.ToString(res);
                }
                else
                {
                    Box.BorderThickness = new Thickness(1);
                    Box.BorderBrush = Brushes.Red;
                }
            }
        }
    }
}
