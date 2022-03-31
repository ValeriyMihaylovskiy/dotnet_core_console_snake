namespace ConsoleSnake
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Console.CursorVisible = false;
            int bestScore = 0;

            while (true)
            {
                var game = new Game(new Vector(6, 6), 500);

                bool result = game.Play();

                Console.Clear();

                if (result == true)
                {
                    Console.WriteLine("You won!!!");
                }
                else
                {
                    if (game.Score > bestScore) bestScore = game.Score;
                    Console.WriteLine($"You lose with {game.Score} score");
                    Console.WriteLine($"Best result: {bestScore}");
                }

                Console.WriteLine("Press any key to restart");
                Console.ReadKey();
            }
        }
    }
}