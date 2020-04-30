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

namespace CustomControls
{
    public class MContainer : ContentControl
    {
        static MContainer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MContainer), new FrameworkPropertyMetadata(typeof(MContainer)));
        }


        public int Radius
        {
            get { return (int)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(int), typeof(MContainer), new PropertyMetadata(0,null,new CoerceValueCallback(CoerceRadius)));

        private static object CoerceRadius(DependencyObject d, object basevalue)
        {
            if ((int)basevalue < 0)
            {
                return 0;
            }

            return basevalue;
        }


    }
}
