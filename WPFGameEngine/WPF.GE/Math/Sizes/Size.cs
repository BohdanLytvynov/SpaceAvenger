namespace WPFGameEngine.WPF.GE.Math.Sizes
{
    public struct Size
    {
        public float Width { get; set; }
        public float Height { get; set; }

        public Size(float width, float height)
        {
            Width = width;
            Height = height;
        }

        public Size(double width, double height)
        {
            Width = (float)width;
            Height = (float)height;
        }

        public Size()
        {
            Width = 0;
            Height = 0;
        }

        public static bool operator == (Size l, Size r)
        { 
            return l.Width == r.Width && l.Height == r.Height;
        }

        public static bool operator !=(Size l, Size r)
        {
            return !(l == r);
        }

        public static Size operator *(Size l, float n)
        {
            return new Size(l.Width * n, l.Height * n);
        }

        public static Size operator *(Size l, Size r)
        {
            return new Size(l.Width * r.Width, l.Height * r.Height);
        }

        public void CheckNegativeSize()
        {
            if (Width < 0)
                Width = 0;
            if (Height < 0)
                Height = 0;
        }

        public override string ToString()
        {
            return $"Width: {Width}, Height: {Height}";
        }
    }
}
