namespace ConsoleSnake
{
    public struct Vector
    {
        public readonly int X, Y;

        public Vector(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static bool operator ==(Vector v1, Vector v2)
        {
            return v1.X == v2.X && v1.Y == v2.Y;
        }

        public static bool operator !=(Vector v1, Vector v2)
        {
            return v1.X != v2.X || v1.Y != v2.Y;
        }
    }
}