using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LiveCharts;
using LiveCharts.Defaults;
using siig.Annotations;
using siig.methods;
using siig.Views_ViewModels;

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
                _CurrentControl = value;
                OnPropertyChanged();
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            CurrentControl = Controls.First();

            DataContext = this;
        }

        private void Proceed_OnClick(object sender, RoutedEventArgs e)
        {
            var MethodControl = CurrentControl as IMethod;

            if ( MethodControl!=null && MethodControl.IsAllChecked())
            {
                MethodControl.Proceed();
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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }


}
