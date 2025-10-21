using System.Windows;

namespace WPFGameEngine.Editor.Controls.Proxy.MinMax
{
    public class MinMaxProxy : FrameworkElement
    {

        public double MinIn
        {
            get { return (double)GetValue(MinInProperty); }
            set { SetValue(MinInProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MinIn.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinInProperty;

        public double MinOut
        {
            get { return (double)GetValue(MinOutProperty); }
            set { SetValue(MinOutProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MinOut.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinOutProperty;

        public double MaxIn
        {
            get { return (double)GetValue(MaxInProperty); }
            set { SetValue(MaxInProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxIn.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxInProperty;

        public double MaxOut
        {
            get { return (double)GetValue(MaxOutProperty); }
            set { SetValue(MaxOutProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxOut.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxOutProperty;

        static MinMaxProxy()
        {
            MinInProperty =
            DependencyProperty.Register("MinIn", typeof(double),
                typeof(MinMaxProxy), new PropertyMetadata((double)0, OnMinInPropertyChanged));

            MinOutProperty =
            DependencyProperty.Register("MinOut", typeof(double),
                typeof(MinMaxProxy), new PropertyMetadata((double)0));

            MaxInProperty =
            DependencyProperty.Register("MaxIn", typeof(double), 
            typeof(MinMaxProxy), new PropertyMetadata((double)0, OnMaxInPropertyChanged));

            MaxOutProperty =
            DependencyProperty.Register("MaxOut", typeof(double), 
            typeof(MinMaxProxy), new PropertyMetadata((double)0));
        }

        private static void OnMaxInPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var This = (MinMaxProxy)d;
            This.SetValue(MaxOutProperty, e.NewValue);
        }

        private static void OnMinInPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var This = (MinMaxProxy)d;
            This.SetValue(MinOutProperty, e.NewValue);
        }
    }
}
