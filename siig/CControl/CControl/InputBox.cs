using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CustomControls
{

    [TemplatePart(Name = "PART_TextBox", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_Placeholder", Type = typeof(Label))]
    public class InputBox : Control
    {
        private TextBox part_TextBox;
        private TextBox part_Placeholder;



        public int Radius
        {
            get { return (int)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Radius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(int), typeof(InputBox), new PropertyMetadata(0));



        public SolidColorBrush ErrorBrush
        {
            get { return (SolidColorBrush)GetValue(ErrorBrushProperty); }
            set { SetValue(ErrorBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ErrorBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ErrorBrushProperty =
            DependencyProperty.Register("ErrorBrush", typeof(SolidColorBrush), typeof(InputBox), new PropertyMetadata(Brushes.OrangeRed));



        private static SolidColorBrush DefaultPlaceholderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#90a4ae"));

        static InputBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(InputBox), new FrameworkPropertyMetadata(typeof(InputBox)));
        }


        //prop
        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }


        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public SolidColorBrush PlaceholderBrush
        {
            get { return (SolidColorBrush)GetValue(PlaceholderBrushProperty); }
            set { SetValue(PlaceholderBrushProperty, value); }
        }


        //dp
        public static readonly DependencyProperty PlaceholderBrushProperty =
            DependencyProperty.Register("PlaceholderBrush", typeof(SolidColorBrush), typeof(InputBox), new PropertyMetadata(DefaultPlaceholderBrush));


        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(InputBox), new FrameworkPropertyMetadata(",",FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register("Placeholder", typeof(string), typeof(InputBox), new PropertyMetadata(""));


        //events
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            if (part_Placeholder.Visibility == Visibility.Visible)
            {
                part_Placeholder.Visibility = Visibility.Collapsed;
                part_TextBox.Visibility = Visibility.Visible;
            }
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);

            if (part_TextBox.Text == "")
            {
                part_Placeholder.Visibility = Visibility.Visible;
                part_TextBox.Visibility = Visibility.Collapsed;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            part_TextBox = GetTemplateChild("PART_TextBox") as TextBox;
            part_Placeholder = GetTemplateChild("PART_Placeholder") as TextBox;
            
        }


        //empty ctor
        public InputBox()
        {
           
        }
    }
}
