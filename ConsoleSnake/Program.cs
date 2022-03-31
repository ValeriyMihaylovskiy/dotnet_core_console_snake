namespace ConsoleSnake
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            int bestScore = 0;

            Vector[] sizes = new Vector[]
            {
                new Vector(6, 6),
                new Vector(12, 10),
                new Vector(16, 12),
                new Vector(20, 20),
                new Vector(30, 20),
            };

            while (true)
            {
                Console.CursorVisible = true;
                Console.Clear();
                for (int i = 0; i < sizes.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {sizes[i].X}x{sizes[i].Y}");
                }
                Console.Write("\nChoose board size:");
                if (int.TryParse(Console.ReadLine(), out int choise) == false) continue;
                if (choise <= 0 || choise > sizes.Length) continue;

                Console.CursorVisible = false;
                var game = new Game(sizes[choise - 1], 500);
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