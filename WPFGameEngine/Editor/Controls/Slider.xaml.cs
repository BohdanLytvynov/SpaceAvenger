using System.Windows;
using System.Windows.Controls;

namespace WPFGameEngine.Editor.Controls
{
    /// <summary>
    /// Interaction logic for Slider.xaml
    /// </summary>
    public partial class Slider : UserControl
    {
        #region DependencyProperties

        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Maximum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaximumProperty;

        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Minimum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinimumProperty;

        public string LabelText
        {
            get { return (string)GetValue(LabelTextProperty); }
            set { SetValue(LabelTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LabelText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelTextProperty;

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty;

        public Style BorderStyle
        {
            get { return (Style)GetValue(BorderStyleProperty); }
            set { SetValue(BorderStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BorderStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BorderStyleProperty;

        public Style StackVerticalStyle
        {
            get { return (Style)GetValue(StackVerticalStyleProperty); }
            set { SetValue(StackVerticalStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StackVerticalStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StackVerticalStyleProperty;

        public Style StackHorizontalStyle
        {
            get { return (Style)GetValue(StackHorizontalStyleProperty); }
            set { SetValue(StackHorizontalStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StackHorizontalStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StackHorizontalStyleProperty;

        public Style LabelStyle
        {
            get { return (Style)GetValue(LabelStyleProperty); }
            set { SetValue(LabelStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LabelStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelStyleProperty;

        public Style TextboxValueStyle
        {
            get { return (Style)GetValue(TextboxValueStyleProperty); }
            set { SetValue(TextboxValueStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextboxValueStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextboxValueStyleProperty =
            DependencyProperty.Register("TextboxValueStyle", typeof(Style), 
                typeof(Slider), new PropertyMetadata(default, OnTextboxValueStylePropertyChanged));
        
        public Style SliderStyle
        {
            get { return (Style)GetValue(SliderStyleProperty); }
            set { SetValue(SliderStyleProperty, value); }
        }

        public Style MaximumLabelStyle
        {
            get { return (Style)GetValue(MaximumLabelStyleProperty); }
            set { SetValue(MaximumLabelStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaximumLabelStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaximumLabelStyleProperty;

        public Style MinimumLabelStyle
        {
            get { return (Style)GetValue(MinimumLabelStyleProperty); }
            set { SetValue(MinimumLabelStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MinimumLabelStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinimumLabelStyleProperty;

        public Style MaximumTextBoxStyle
        {
            get { return (Style)GetValue(MaximumTextBoxStyleProperty); }
            set { SetValue(MaximumTextBoxStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaximumTextBoxStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaximumTextBoxStyleProperty;

        public Style MinimumTextBoxStyle
        {
            get { return (Style)GetValue(MinimumTextBoxStyleProperty); }
            set { SetValue(MinimumTextBoxStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MinimumTextBoxStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinimumTextBoxStyleProperty;
                         
        // Using a DependencyProperty as the backing store for SliderStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SliderStyleProperty;
       
        #endregion

        #region Ctor

        static Slider()
        {
            LabelStyleProperty =
            DependencyProperty.Register("LabelStyle", typeof(Style),
                typeof(Slider), new PropertyMetadata(default, OnLabelStyleChanged));

            StackVerticalStyleProperty =
            DependencyProperty.Register("StackVerticalStyle", typeof(Style),
                typeof(Slider), new PropertyMetadata(default, OnStackVerticalStyleChanged));

            StackHorizontalStyleProperty =
            DependencyProperty.Register("StackHorizontalStyle", typeof(Style),
                typeof(Slider), new PropertyMetadata(default, OnStackHorizontalStyleChanged));

            SliderStyleProperty =
            DependencyProperty.Register("SliderStyle", typeof(Style),
            typeof(Slider), new PropertyMetadata(default, OnSliderStyleChanged));

            ValueProperty =
            DependencyProperty.Register("Value", typeof(double),
                typeof(Slider), new PropertyMetadata(0.0, OnValuePropertyChanged));

            LabelTextProperty =
            DependencyProperty.Register("LabelText", typeof(string),
            typeof(Slider), new PropertyMetadata(string.Empty, OnLabelTextPropertyChanged));

            BorderStyleProperty =
            DependencyProperty.Register("BorderStyle", typeof(Style),
            typeof(Slider), new PropertyMetadata(default, OnBorderStylePropertyChanged));

            MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(double),
                typeof(Slider), new PropertyMetadata(0.0, OnMaximumPropertyChanged));

            MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(double),
                typeof(Slider), new PropertyMetadata(0.0, OnMinimumPropertyChanged));

            MaximumLabelStyleProperty =
            DependencyProperty.Register("MaximumLabelStyle", typeof(Style),
                typeof(Slider), new PropertyMetadata(default, OnMaximumLabelStylePropertyChanged));

            MinimumLabelStyleProperty =
            DependencyProperty.Register("MinimumLabelStyle", typeof(Style),
                typeof(Slider), new PropertyMetadata(default, OnMinimumLabelStylePropertyChanged));

            MaximumTextBoxStyleProperty =
            DependencyProperty.Register("MaximumTextBoxStyle", typeof(Style),
                typeof(Slider), new PropertyMetadata(default, OnMaximumTextBoxStylePropertyChanged));

            MinimumTextBoxStyleProperty =
            DependencyProperty.Register("MinimumTextBoxStyle", typeof(Style),
                typeof(Slider), new PropertyMetadata(default, OnMinimumTextBoxStylePropertyChanged));
        }

        private static void OnMinimumTextBoxStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var This = (Slider)d;
            var val = (Style)e.NewValue;
            if (val == null)
                return;
            This.SetValue(MinimumTextBoxStyleProperty, e.NewValue);
            This.Min.Style = val;
        }

        private static void OnMaximumTextBoxStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var This = (Slider)d;
            var val = (Style)e.NewValue;
            if (val == null)
                return;
            This.SetValue(MaximumTextBoxStyleProperty, e.NewValue);
            This.Max.Style = val;
        }

        private static void OnMinimumLabelStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var This = (Slider)d;
            var val = (Style)e.NewValue;
            if (val == null)
                return;
            This.SetValue(MinimumLabelStyleProperty, e.NewValue);
            This.MinLabel.Style = val;
        }

        private static void OnMaximumLabelStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var This = (Slider)d;
            var val = (Style)e.NewValue;
            if (val == null)
                return;
            This.SetValue(MaximumLabelStyleProperty, e.NewValue);
            This.MaxLabel.Style = val;
        }
       
        private static void OnBorderStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var This = (Slider)d;
            var val = (Style)e.NewValue;
            if (val == null)
                return;
            This.SetValue(BorderStyleProperty, e.NewValue);
            This.Border.Style = val;
        }

        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var This = (Slider)d;
            This.SetValue(ValueProperty, e.NewValue);
        }
       
        public Slider()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        private static void OnLabelTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var This = (Slider)d;
            This.SetValue(LabelTextProperty, e.NewValue);
        }

        private static void OnStackHorizontalStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var This = (Slider)d;
            var val = (Style)e.NewValue;
            if (val == null)
                return;
            This.SetValue(StackHorizontalStyleProperty, e.NewValue);
            This.StackHorizontal.Style = (Style)e.NewValue;
        }

        private static void OnStackVerticalStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var This = (Slider)d;
            var val = (Style)e.NewValue;
            if (val == null)
                return;
            This.SetValue(StackVerticalStyleProperty, e.NewValue);
            This.StackVertical.Style = (Style)e.NewValue;
        }

        private static void OnLabelStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var This = (Slider)d;
            var val = (Style)e.NewValue;
            if (val == null)
                return;
            This.SetValue(LabelStyleProperty, e.NewValue);
            This.Label.Style = (Style)e.NewValue;
        }

        private static void OnSliderStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var This = (Slider)d;
            var val = (Style)e.NewValue;
            if (val == null)
                return;
            This.SetValue(SliderStyleProperty, e.NewValue);
            This.SliderValue.Style = (Style)e.NewValue;
        }

        private static void OnMaximumPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var This = (Slider)d;
            This.SetValue(MaximumProperty, e.NewValue);
        }

        private static void OnMinimumPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var This = (Slider)d;
            This.SetValue(MinimumProperty, e.NewValue);
        }

        private static void OnTextboxValueStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var This = ((Slider)d);
            This.SetValue(TextboxValueStyleProperty, e.NewValue);
            if (e.NewValue == null)
                return;
            This.SetValue(TextboxValueStyleProperty, e.NewValue);
            This.TextValue.Style = (Style)e.NewValue;
        }

        #endregion

        private void Control_Loaded(object sender, RoutedEventArgs e)
        {
            var old = this.Maximum;
            this.Maximum = -100;
            this.Maximum = old;

            old = this.Minimum;
            this.Minimum = -100;
            this.Minimum = old;
        }
    }
}
