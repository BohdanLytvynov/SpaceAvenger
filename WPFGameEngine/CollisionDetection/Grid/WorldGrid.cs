namespace WPFGameEngine.CollisionDetection.Grid
{
    public class WorldGrid
    {
        public double CellHeight { get; protected set; }
        public double CellWidth { get; protected set; }

        public Cell[,] Grid { get; protected set; }

        public WorldGrid(double cellHeight, 
            double cellWidth, 
            double winWidth,
            double winHeight)
        {
            CellHeight = cellHeight;
            CellWidth = cellWidth;
            int rows = (int)(winHeight / cellHeight);
            int cols = (int)(winWidth / cellWidth);
            Grid = new Cell[rows, cols];
        }
    }
}
