using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using LiveCharts;
using LiveCharts.Defaults;
using siig.methods;
using siig.Views_ViewModels;

namespace siig
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window,INotifyPropertyChanged
    {
        private static List<UserControl> Controls = new List<UserControl>()
        {
            new OperationsControl()
        };

        private UserControl CurrentControl = Controls.First();

        public static ChartValues<ObservablePoint> CorelationSignalChartValues = new ChartValues<ObservablePoint>();

        public MainWindow()
        {
            InitializeComponent();
            SettingsBlock.Content = CurrentControl;
            DataContext = CurrentControl;
        }

        public event PropertyChangedEventHandler PropertyChanged;


        private void Proceed_OnClick(object sender, RoutedEventArgs e)
        {
            var MethodControl = CurrentControl as IMethod;

            if (MethodControl.IsAllChecked())
            {
               MethodControl.Proceed();
               DataContext = CurrentControl;
               PropertyChanged?.Invoke(null,null);
            }
        }
    }


}
