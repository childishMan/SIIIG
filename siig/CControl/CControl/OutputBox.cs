using System.Windows;
using System.Windows.Controls;

namespace CustomControls
{
    public class OutputBox : Control
    {
        static OutputBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(OutputBox), new FrameworkPropertyMetadata(typeof(OutputBox)));
        }



        public string content
        {
            get { return (string)GetValue(contentProperty); }
            set { SetValue(contentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for content.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty contentProperty =
            DependencyProperty.Register("content", typeof(string), typeof(OutputBox), new PropertyMetadata(""));


    }
}
