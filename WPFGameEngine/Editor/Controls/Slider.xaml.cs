using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace WPFGameEngine.Editor.Controls
{
    /// <summary>
    /// Interaction logic for Slider.xaml
    /// </summary>
    public partial class Slider : UserControl, INotifyPropertyChanged, IDataErrorInfo
    {
        #region Fields
        private string m_IntegerPartValueString;
        private string m_FloatPartValueString;

        private string m_MaximumString;
        private string m_MinimumString;
        #endregion

        #region Properties
        public string IntegerPartValueString
        {
            get => m_IntegerPartValueString;
            set
            {
                Set(ref m_IntegerPartValueString, value);
            }
        }

        public string FloatPartValueString
        {
            get => m_FloatPartValueString;
            set
            {
                Set(ref m_FloatPartValueString, value);
            }
        }

        public string MaximumString
        {
            get => m_MaximumString;
            set => Set(ref m_MaximumString, value);
        }

        public string MinimumString
        {
            get => m_MinimumString;
            set => Set(ref m_MinimumString, value);
        }
        #endregion

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

        public bool FloatValue
        {
            get { return (bool)GetValue(FloatValueProperty); }
            set { SetValue(FloatValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FloatValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FloatValueProperty;

        public string Deliminator
        {
            get { return (string)GetValue(DeliminatorProperty); }
            set { SetValue(DeliminatorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Deliminator.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DeliminatorProperty;

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

        public Style IntegerPartTextBoxStyle
        {
            get { return (Style)GetValue(IntegerPartTextBoxStyleProperty); }
            set { SetValue(IntegerPartTextBoxStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IntegerPartTextBoxStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IntegerPartTextBoxStyleProperty;

        public Style FloatPartTextBoxStyleProperty
        {
            get { return (Style)GetValue(FloatPartTextBoxStylePropertyProperty); }
            set { SetValue(FloatPartTextBoxStylePropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FloatPartTextBoxStyleProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FloatPartTextBoxStylePropertyProperty;

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

        public Style DeliminatorLabelStyle
        {
            get { return (Style)GetValue(DeliminatorLabelStyleProperty); }
            set { SetValue(DeliminatorLabelStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DeliminatorLabelStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DeliminatorLabelStyleProperty;

        #region IDataErrorInfo Implementation
        public string Error => throw new NotImplementedException();

        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;

                switch (columnName)
                {
                    case nameof(IntegerPartValueString):
                        int v;
                        if (!int.TryParse(IntegerPartValueString, out v))
                        {
                            error = "Not a number!";
                        }
                        else
                        {
                            var value = this.Value.ToString();
                            var arr = value.Split(',');
                            if (value.Contains(','))
                            {
                                this.Value = double.Parse(IntegerPartValueString + "," + arr[1]);
                            }
                            else
                            {
                                this.Value = double.Parse(IntegerPartValueString);
                            }
                        }
                        break;
                    case nameof(FloatPartValueString):
                        if (!isNumbers(FloatPartValueString, out error)) { }
                        else
                        {
                            var value = this.Value.ToString();
                            var arr = value.Split(',');
                            if (value.Contains(','))
                            {
                                this.Value = double.Parse(arr[0] + "," + FloatPartValueString);
                            }
                        }
                        break;
                    case nameof(MinimumString):
                        if (!isNumberValid(MinimumString, out error)) { }
                        else
                        {
                            this.Minimum = double.Parse(MinimumString);
                        }
                        break;
                    case nameof(MaximumString):
                        if (!isNumberValid(MaximumString, out error)) { }
                        else
                        {
                            this.Maximum = double.Parse(MaximumString);
                        }
                        break;
                }
                return error;
            }
        }

        private bool isNumbers(string value, out string error)
        {
            error = string.Empty;
            foreach (var item in value)
            {
                if (!Char.IsDigit(item))
                {
                    error = "Not a number!";
                    return false;
                }
            }

            return true;
        }

        private bool isNumberValid(string value, out string error)
        {
            error = string.Empty;
            var res = double.TryParse(value, out double _);
            if (!res)
                error = "Not a number!";
            return res;
        }

        #endregion

        // Using a DependencyProperty as the backing store for SliderStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SliderStyleProperty;

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public virtual bool Set<T>(ref T field, T value, [CallerMemberName] string propName = "")
        {
            if (field == null) throw new ArgumentNullException(nameof(field));

            if (value.Equals(field))
            {
                return false;
            }
            else
            {
                field = value;
                OnPropertyChanged(propName);
                return true;
            }
        }
        #endregion

        #endregion

        #region Ctor

        static Slider()
        {
            FloatValueProperty =
            DependencyProperty.Register("FloatValue", typeof(bool),
            typeof(Slider), new PropertyMetadata(true, OnFloatValuePropertyChanged));

            DeliminatorProperty =
            DependencyProperty.Register("Deliminator", typeof(string),
            typeof(Slider), new PropertyMetadata(",", OnDeliminatorPropertyChanged));

            LabelStyleProperty =
            DependencyProperty.Register("LabelStyle", typeof(Style),
                typeof(Slider), new PropertyMetadata(default, OnLabelStyleChanged));

            IntegerPartTextBoxStyleProperty =
            DependencyProperty.Register("IntegerPartTextBoxStyle", typeof(Style),
                typeof(Slider), new PropertyMetadata(default, OnIntegerPartTextBoxStyleChanged));

            FloatPartTextBoxStylePropertyProperty =
            DependencyProperty.Register("FloatPartTextBoxStyleProperty",
                typeof(Style), typeof(Slider), new PropertyMetadata(default, OnFloatPartTextBoxStyleChanged));

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

            DeliminatorLabelStyleProperty =
            DependencyProperty.Register("DeliminatorLabelStyle", typeof(Style),
            typeof(Slider), new PropertyMetadata(default, OnDeliminatorLabelpropertyChanged));
        }

        private static void OnDeliminatorLabelpropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var This = (Slider)d;
            var val = (Style)e.NewValue;
            if (val == null)
                return;
            This.DeliminatorLabelStyle = (Style)e.NewValue;
        }

        private static void OnMinimumTextBoxStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var This = (Slider)d;
            var val = (Style)e.NewValue;
            if (val == null)
                return;
            This.MinimumTextBoxStyle = (Style)e.NewValue;
        }

        private static void OnMaximumTextBoxStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var This = (Slider)d;
            var val = (Style)e.NewValue;
            if (val == null)
                return;
            This.MaximumTextBoxStyle = (Style)e.NewValue;
        }

        private static void OnMinimumLabelStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var This = (Slider)d;
            var val = (Style)e.NewValue;
            if (val == null)
                return;
            This.MinimumLabelStyle = (Style)e.NewValue;
        }

        private static void OnMaximumLabelStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var This = (Slider)d;
            var val = (Style)e.NewValue;
            if (val == null)
                return;
            This.MaximumLabelStyle = (Style)e.NewValue;
        }

        private static void OnFloatValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var This = (Slider)d;
            This.FloatValue = (bool)e.NewValue;
            if (This.FloatValue)
            {
                This.FloatPart.Visibility = Visibility.Visible;
                This.DeliminatorLabel.Visibility = Visibility.Visible;
            }
            else
            {
                This.FloatPart.Visibility = Visibility.Collapsed;
                This.DeliminatorLabel.Visibility = Visibility.Collapsed;
            }
        }

        private static void OnBorderStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var This = (Slider)d;
            var val = (Style)e.NewValue;
            if (val == null)
                return;
            This.BorderStyle = (Style)e.NewValue;
        }

        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var This = (Slider)d;
            var newValue = (double)e.NewValue;
            if (This.Value != newValue)
                This.Value = newValue;

            var currentValue = This.Value.ToString();
            var arr = currentValue.Split('.');
            if (currentValue.Contains('.'))
            {
                This.IntegerPartValueString = arr[0];
                This.FloatPartValueString = arr[1];
            }
            else
            {
                This.IntegerPartValueString = arr[0];
            }
        }

        private static void OnDeliminatorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var This = (Slider)d;
            This.Deliminator = e.NewValue.ToString();
        }

        public Slider()
        {
            m_IntegerPartValueString = "0";
            m_FloatPartValueString = "0";
            m_MaximumString = "0";
            m_MinimumString = "0";
            InitializeComponent();
        }

        #endregion

        #region Methods

        private static void OnLabelTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var This = (Slider)d;
            This.LabelText = e.NewValue.ToString();
        }

        private static void OnStackHorizontalStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var This = (Slider)d;
            var val = (Style)e.NewValue;
            if (val == null)
                return;
            This.StackHorizontal.Style = (Style)e.NewValue;
        }

        private static void OnStackVerticalStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var This = (Slider)d;
            var val = (Style)e.NewValue;
            if (val == null)
                return;
            This.StackVertical.Style = (Style)e.NewValue;
        }

        private static void OnLabelStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var This = (Slider)d;
            var val = (Style)e.NewValue;
            if (val == null)
                return;
            This.Label.Style = (Style)e.NewValue;
        }

        private static void OnFloatPartTextBoxStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var This = (Slider)d;
            var val = (Style)e.NewValue;
            if (val == null)
                return;
            This.IntegerPart.Style = (Style)e.NewValue;
        }

        private static void OnIntegerPartTextBoxStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var This = (Slider)d;
            var val = (Style)e.NewValue;
            if (val == null)
                return;
            This.FloatPart.Style = (Style)e.NewValue;
        }

        private static void OnSliderStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var This = (Slider)d;
            var val = (Style)e.NewValue;
            if (val == null)
                return;
            This.SliderValue.Style = (Style)e.NewValue;
        }

        private static void OnMaximumPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var This = (Slider)d;
            var val = (double)e.NewValue;
            if (This.Maximum != val)
                This.Maximum = val;
            This.MaximumString = val.ToString();
            This.SliderValue.Maximum = val;
        }

        private static void OnMinimumPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var This = (Slider)d;
            var val = (double)e.NewValue;
            if (This.Minimum != val)
                This.Minimum = val;
            This.MinimumString = val.ToString();
            This.SliderValue.Minimum = (double)e.NewValue;
        }
        #endregion
    }
}
