using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CustomControls;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using siig.Annotations;
using siig.methods;
using siig.models;
using visual;
using Xceed.Wpf.Toolkit.Core.Converters;

namespace siig.Views_ViewModels
{
    /// <summary>
    /// Interaction logic for ConvolutionControl.xaml
    /// </summary>
    public partial class ConvolutionControl : UserControl,IMethod,INotifyPropertyChanged
    {
        public ChartValues<ObservablePoint> FirstSignalChartValues { get; set; }
        public ChartValues<ObservablePoint> SecondSignalChartValues { get; set; }
        public ChartValues<ObservablePoint> FinalSignalChartValues { get; set; }

        private Dictionary<int, double> FirstSignal;
        private Dictionary<int, double> SecondSignal;
        private Dictionary<int, double> FinalSignal;

        private bool isSeriesBinded = false;

        private string OuputString = "";

        public UserControl CurrentControl;

        public string NameOfView { get; set; } = "Convolution";

        private SeriesCollection _collection;

        public SeriesCollection Collection
        {
            get { return _collection;}
            set
            {
                _collection = value; 
                OnPropertyChanged();
            } }

        public ConvolutionControl()
        {
            InitializeComponent();

            FirstSignal = new Dictionary<int, double>();
            SecondSignal = new Dictionary<int, double>();
            FinalSignal = new Dictionary<int, double>();

            FirstSignalChartValues = new ChartValues<ObservablePoint>();
            SecondSignalChartValues = new ChartValues<ObservablePoint>();
            FinalSignalChartValues = new ChartValues<ObservablePoint>();

            CurrentControl = this;

            DataContext = this;
        }

        private void BindSeries()
        {
            if (!isSeriesBinded)
            {
                Collection = new SeriesCollection()
                {
                    new ColumnSeries()
                    {
                        Title = "FinalSignal Signal",
                        Values = FinalSignalChartValues,
                        Fill = Brushes.OrangeRed
                    },
                    new ScatterSeries()
                    {
                        Title = "First Signal",
                        Values = FirstSignalChartValues,
                        Fill = Brushes.LawnGreen
                    },

                    new ScatterSeries()
                    {
                        Title = "Second Signal",
                        Values = SecondSignalChartValues,
                        Fill = Brushes.Aqua
                    }

                };
                isSeriesBinded = true;
            }
        }

        private void BindCharts()
        {
            BindSeries();

            FirstSignalChartValues.Clear();
            SecondSignalChartValues.Clear();
            FinalSignalChartValues.Clear();


            foreach (var item in FirstSignal)
            {
                FirstSignalChartValues.Add(new ObservablePoint(item.Key,item.Value));
            }

            foreach (var item in SecondSignal)
            {
                SecondSignalChartValues.Add(new ObservablePoint(item.Key,item.Value));
            }

            OuputString = "";

            foreach (var item in FinalSignal)
            {
                FinalSignalChartValues.Add(new ObservablePoint(item.Key,item.Value));
                OuputString += $"{item.Key};{item.Value:f1} ";
            }
        }

        public bool IsAllChecked()
        {
            if (FirstSignal.Count != 0 && SecondSignal.Count != 0)
                return true;
            else
                return false;

        }

        public void Proceed()
        {
            if (IsAllChecked())
            { 
                FinalSignal = Convolution.Convolute(FirstSignal,SecondSignal);

                BindCharts();

                OutputSignal.Text = OuputString;
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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
