using Pastel;
using System.Diagnostics;
using System.Drawing;

namespace ConsoleSnake
{
    public class Game
    {
        private readonly string _snakePart, _applePart, _borderPart, _emptyPart;
        private readonly int _boardSizeX, _boardSizeY, _frameTime;
        private Vector _apple;
        private Vector _currentDirection;
        private List<Vector> _snake;
        public int Score { get; private set; }

        public Game(Vector boardSize, int frameTime)
        {
            if (boardSize.X < 5 || boardSize.Y < 5)
                throw new ArgumentException("Min board size is 5x5");

            if (frameTime <= 0)
                throw new ArgumentException("Delay cann't be zero");

            _boardSizeX = boardSize.X;
            _boardSizeY = boardSize.Y;
            _frameTime = frameTime;

            _snakePart = "██".Pastel(Color.GreenYellow);
            _applePart = "██".Pastel(Color.Red);
            _borderPart = "▒▒";
            _emptyPart = "  ".Pastel(Color.Black);
        }

        public bool Play()
        {
            Console.Clear();
            Reset();
            Draw();

            var watch = new Stopwatch();
            watch.Start();

            while (true)
            {
                Vector newDirection = _currentDirection;

                while (watch.ElapsedMilliseconds <= _frameTime)
                {
                    if (_currentDirection != newDirection) continue;
                    newDirection = ReadInputDirection();
                }

                watch.Restart();

                Console.SetCursorPosition(0, 0);
                _currentDirection = newDirection;
                MoveSnake();

                if (CheckIfSnakeHasEatApple())
                {
                    IncreaseSnake();
                    Score++;
                    if (_snake.Count == _boardSizeX * _boardSizeY)
                        return true;
                    CreateNewApple();
                }

                if (CheckSnakeEatItself())
                {
                    return false;
                }

                Draw();
            }
        }

        private bool CheckSnakeEatItself()
        {
            for (int i = 1; i < _snake.Count; i++)
            {
                if (_snake[0] == _snake[i])
                    return true;
            }

            return false;
        }

        private Vector ReadInputDirection()
        {
            if (!Console.KeyAvailable) return _currentDirection;

            var key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.RightArrow && _currentDirection.X != -1)
                return new Vector(1, 0);

            else if (key == ConsoleKey.DownArrow && _currentDirection.Y != -1)
                return new Vector(0, 1);

            else if (key == ConsoleKey.LeftArrow && _currentDirection.X != 1)
                return new Vector(-1, 0);

            else if (key == ConsoleKey.UpArrow && _currentDirection.Y != 1)
                return new Vector(0, -1);

            return _currentDirection;
        }

        private bool CheckIfSnakeHasEatApple()
        {
            return _snake[0] == _apple;
        }

        private void IncreaseSnake()
        {
            // Add new part with position that already exist in snake's parts array
            // for new part skip one draw call
            var lastPart = _snake[_snake.Count - 1];
            _snake.Add(lastPart);
        }

        private void MoveSnake()
        {
            // Calculating new head position
            int x = _snake[0].X + _currentDirection.X;
            int y = _snake[0].Y + _currentDirection.Y;

            // x clamp
            if (x < 0) x = _boardSizeX - 1;
            else if (x == _boardSizeX) x = 0;

            // y clamp
            if (y < 0) y = _boardSizeY - 1;
            else if (y == _boardSizeY) y = 0;

            var lastPartPosition = new Vector(x, y);

            // Moving snake's parts
            for (int i = 0; i < _snake.Count; i++)
            {
                Vector temp = _snake[i];
                _snake[i] = lastPartPosition;
                lastPartPosition = temp;
            }
        }

        private void Reset()
        {
            _snake = new List<Vector>()
            {
                new Vector(2, 0),
                new Vector(1, 0),
                new Vector(0, 0),
            };
            CreateNewApple();
            Score = 0;
            _currentDirection = new Vector(1, 0);
        }

        private int GetFreePositionsCount()
        {
            return _boardSizeX * _boardSizeY - _snake.Count - 1; // -1 is apple position
        }

        private List<Vector> GetFreePositions()
        {
            var list = new List<Vector>();

            for (int y = 0; y < _boardSizeY; y++)
            {
                for (int x = 0; x < _boardSizeX; x++)
                {
                    var p = new Vector(x, y);
                    if (p == _apple || _snake.Contains(p)) continue;
                    list.Add(p);
                }
            }

            return list;
        }

        private void CreateNewApple()
        {
            List<Vector> positions = GetFreePositions();
            var random = new Random();
            int index = random.Next(positions.Count);
            _apple = positions[index];
        }

        private bool IsPositionOutOfBoard(Vector position)
        {
            return
                position.X == -1 ||
                position.Y == -1 ||
                position.X == _boardSizeX ||
                position.Y == _boardSizeY;
        }

        private void Draw()
        {
            string board = "";

            for (int y = -1; y <= _boardSizeY; y++)
            {
                for (int x = -1; x <= _boardSizeX; x++)
                {
                    var p = new Vector(x, y);

                    if (IsPositionOutOfBoard(p))
                        board += _borderPart;

                    else if (p == _apple)
                        board += _applePart;

                    else if (_snake.Contains(p))
                        board += _snakePart;

                    else
                        board += _emptyPart;   
                }

                board += "\n";
            }

            board += "\nScore:" + Score;
            Console.Write(board);
        }
    }
}
