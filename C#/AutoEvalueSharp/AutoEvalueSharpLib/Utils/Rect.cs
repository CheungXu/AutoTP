namespace AutoEvalueSharpLib.Utils
{
    public struct Rect
    {
        public Rect(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public int X;
        public int Y;
        public int Width;
        public int Height;
    }
}
