using System;
using System.CodeDom;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace CustomControls
{
    
    [TemplatePart(Name = "PART_Bord", Type = typeof(Border))]
    [TemplatePart(Name = "PART_Label", Type = typeof(Border))]
    public class tButton : Button
    {

        private Border border;
        private Label label;

        private int AnimationDuration = 500;
        private double FontSizefactor = 0.5;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            border = GetTemplateChild("PART_Bord") as Border;

            label = GetTemplateChild("PART_Label") as Label;

        }


        static tButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(tButton), new FrameworkPropertyMetadata(typeof(tButton)));
        }



        public SolidColorBrush HoverBrush
        {
            get { return (SolidColorBrush)GetValue(HoverBrushProperty); }
            set { SetValue(HoverBrushProperty, value); }
        }

        public static readonly DependencyProperty HoverBrushProperty =
            DependencyProperty.Register("HoverBrush", typeof(SolidColorBrush), typeof(tButton), new PropertyMetadata(Brushes.Transparent));



        public int Radius
        {
            get { return (int)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(int), typeof(tButton), new PropertyMetadata(0));



        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(tButton), new PropertyMetadata(""));


        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);

            Color clr = HoverBrush.Color;
            double fontSize = label.FontSize;

            DoubleAnimation FontSizeAnimation = new DoubleAnimation(fontSize+FontSizefactor,new Duration(TimeSpan.FromMilliseconds(AnimationDuration/2.0)),FillBehavior.HoldEnd);

            ColorAnimation BackgroundAnimation = new ColorAnimation(clr,new Duration(TimeSpan.FromMilliseconds(AnimationDuration)),FillBehavior.HoldEnd);
            
            PropertyPath targetBackground = new PropertyPath("(Border.Background).(SolidColorBrush.Color)");
            PropertyPath targetFontSize = new PropertyPath("(Label.FontSize)");

            Storyboard str = new Storyboard();

            Storyboard.SetTarget(BackgroundAnimation,border);
            Storyboard.SetTargetProperty(BackgroundAnimation,targetBackground);

            Storyboard.SetTarget(FontSizeAnimation, label);
            Storyboard.SetTargetProperty(FontSizeAnimation, targetFontSize);

            str.Children.Add(BackgroundAnimation);
            str.Children.Add(FontSizeAnimation);

            str.Begin();

        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);

            Color BackgroundColor = ((SolidColorBrush) Background).Color;
            double fontSize = label.FontSize;

            if (BackgroundColor != null && border!=null)
            {
                ColorAnimation BackgroundAnimation = new ColorAnimation(BackgroundColor, new Duration(TimeSpan.FromMilliseconds(AnimationDuration)), FillBehavior.HoldEnd);

                DoubleAnimation FontSizeAnimation = new DoubleAnimation(fontSize - FontSizefactor, new Duration(TimeSpan.FromMilliseconds(AnimationDuration / 2.0)), FillBehavior.HoldEnd);

                PropertyPath target = new PropertyPath("(Border.Background).(SolidColorBrush.Color)");
                PropertyPath targetFontSize = new PropertyPath("(Label.FontSize)");

                Storyboard str = new Storyboard();

                Storyboard.SetTarget(BackgroundAnimation, border);
                Storyboard.SetTargetProperty(BackgroundAnimation, target);

                Storyboard.SetTarget(FontSizeAnimation, label);
                Storyboard.SetTargetProperty(FontSizeAnimation, targetFontSize);

                str.Children.Add(BackgroundAnimation);
                str.Children.Add(FontSizeAnimation);

                str.Begin();
            }
        }
    }
}
