using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using LiveCharts.Wpf.Charts.Base;
using siig.methods;
using siig.Views_ViewModels;
using Xceed.Wpf.Toolkit.Core.Converters;
using Color = System.Drawing.Color;
using ColorConverter = System.Drawing.ColorConverter;

namespace siig
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private static List<UserControl> Controls = new List<UserControl>()
        {
            new ConvolutionControl(),
            new OperationsControl(),
            new CorelationControl(),
            new FFTControl()

        };

        private UserControl _CurrentControl;

        public UserControl CurrentControl
        {
            get { return _CurrentControl; }
            set
            {
                PropertyChanged += delegate { };
                _CurrentControl = value;
            }
        }

        public static ChartValues<ObservablePoint> CorelationSignalChartValues = new ChartValues<ObservablePoint>();

        public MainWindow()
        {
            InitializeComponent();

            CurrentControl = Controls.First();

            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Proceed_OnClick(object sender, RoutedEventArgs e)
        {
            var MethodControl = CurrentControl as IMethod;

            if (MethodControl.IsAllChecked())
            {
                MethodControl.Proceed();
                PropertyChanged?.Invoke(null, null);
            }

            else
            {
                MessageBox.Show("Enter correct data to proceed!!!", "Input data Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void NextMethod(object sender, MouseButtonEventArgs e)
        {
            int IndexOfMethod = Controls.FindIndex(control => control == CurrentControl);

            if (IndexOfMethod != Controls.Count - 1)
            {
                CurrentControl = Controls[IndexOfMethod + 1];
            }
        }

        private void PreviousMethod(object sender, MouseButtonEventArgs e)
        {
            int IndexOfMethod = Controls.FindIndex(control => control == CurrentControl);
            if (IndexOfMethod != 0)
            {
                CurrentControl = Controls[IndexOfMethod - 1];
            }
        }


        private void PartaAme_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Project by Marian Hupalo", "About", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }


}
