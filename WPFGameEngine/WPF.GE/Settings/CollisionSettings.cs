namespace WPFGameEngine.WPF.GE.Settings
{
    public static class CollisionSettings
    {
        public static double WorldXPosition { get; set; }
        public static double WorldYPosition { get; set; }
        public static double WorldWidth { get; set; }
        public static double WorldHeight { get; set; }
        public static int MaxQuadTreeDepth { get; set; } = 8;
        public static int MaxItemsPerNode { get; set; } = 16;
        public static int SpatialThreshold { get; set; } = 5000;
        public static int CollisionCheckDelay_MS { get; set; } = 16;

    }
}
