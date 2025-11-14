using System.Windows.Media;

namespace WPFGameEngine.WPF.GE.Settings
{
    public static class GESettings
    {
        public static bool DrawGizmo { get; set; }
        public static bool DrawBorders { get; set; }
        public static bool DrawColliders { get; set; }
        public static Brush BorderRectangleBrush { get; set; } = Brushes.Transparent;
        public static Pen BorderRectanglePen { get; set; } = new Pen(Brushes.DarkOrange, 2);
        public static Brush ColliderPointFillBrush { get; set; } = Brushes.LightGreen;
        public static Pen ColliderPointPen { get; set; } = new Pen(Brushes.Black, 2);
        public static Brush ColliderFillBrush { get; set; } = Brushes.Transparent;
        public static Pen ColliderBorderPen { get; set; } = new Pen(Brushes.LightGreen, 2);
        public static Brush GizmoCenterBrush { get; set; } = Brushes.Orange;
        public static Pen GizmoCenterPen { get; set; } = new Pen(Brushes.Black, 2);
        public static double GizmoCenterXRadius { get; set; } = 5;
        public static double GizmoCenterYRadius { get; set; } = 5;
        public static Pen XAxisColor { get; set; } = new Pen(Brushes.Red, 3);
        public static Pen YAxisColor { get;set; } = new Pen(Brushes.Green, 3);
    }
}
