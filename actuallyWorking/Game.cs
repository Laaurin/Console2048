using System.Runtime.InteropServices;

namespace working;

public class Game
{
    private int[] _grid, copy;

    private List<Tile> _movingTiles;

    public Game(int[] grid)
    {
        _grid = grid;
        copy = new int[16];
        _movingTiles = new List<Tile>();
    }

    public Game()
    {
        _movingTiles = new List<Tile>();
        copy = new int[16];
    }

    public void Play()
    {
        bool playing = true;
        char key;
        while (playing)
        {
            if (Console.KeyAvailable)
            {
                key = Console.ReadKey(true).KeyChar;

                switch (key)
                {
                    case 'a':
                        Left(); Spawn();
                        break;
                    case 'd':
                        Right(); Spawn();
                        break;
                    case 'w':
                        Up(); Spawn();
                        break;
                    case 's':
                        Down(); Spawn();
                        break;
                }
                Print(_grid);
            }
        }
    }

    public void Print(int[] grid)
    {
        Console.SetCursorPosition(0, 0);
        for (int i = 0; i < 16; i+=4)
        {
            if (i % 4 == 0 && i != 0)
            {
                Console.WriteLine("                                                        ");
            }

            for (int j = 0; j < 4; j++)
            {
                Console.ForegroundColor = GetColor(grid[i + j]);
                Console.Write($"[        ]    ");
            }

            Console.WriteLine();
            
            for (int j = 0; j < 4; j++)
            {
                Console.ForegroundColor = GetColor(grid[i + j]);
                if(grid[i+j] == 0) Console.Write("[        ]    ");
                else Console.Write($"[{grid[i + j],8}]    ");
            }

            Console.WriteLine();

            for (int j = 0; j < 4; j++)
            {
                Console.ForegroundColor = GetColor(grid[i + j]);
                Console.Write($"[        ]    ");
            }

            Console.WriteLine();
        }

    }
    private static ConsoleColor GetColor(int value)
    {
        switch (value)
        {
            case 0:
                return ConsoleColor.DarkGray;
            case 2:
                return ConsoleColor.Gray;
            case 4:
                return ConsoleColor.White;
            case 8:
                return ConsoleColor.DarkYellow;
            case 16:
                return ConsoleColor.DarkMagenta;
            case 32:
                return ConsoleColor.Red;
            case 64:
                return ConsoleColor.DarkRed;
            case 128:
            case 256:
            case 512:
            case 1024:
            case 2048:
                return ConsoleColor.Yellow;
            default:
                return ConsoleColor.Black;
        }
    }
    
    public void Print3()
    {
        for (int i = 0; i < 16; i++)
        {
            int x = (i % 4)*14;
            int y = (i / 4)*4 + 15;
        
            Console.SetCursorPosition(x, y);
            Console.Write("[        ]");
            Console.SetCursorPosition(x, y+1);
            Console.Write($"[{_grid[i], 8}]");
            Console.SetCursorPosition(x, y+2);
            Console.Write("[        ]");
        }
    }
    
    public void Print2()
    {
        for (int i = 0; i < 16; i++)
        {
            if(i % 4 == 0) Console.Write("   ");
            Console.Write($"{_grid[i], 3}");
        }
    }

    public void PrintEmpty()
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.SetCursorPosition(0, 0);
        Console.WriteLine("[        ]    [        ]    [        ]    [        ]");
        Console.WriteLine("[        ]    [        ]    [        ]    [        ]");
        Console.WriteLine("[        ]    [        ]    [        ]    [        ]");
        Console.WriteLine();
        Console.WriteLine("[        ]    [        ]    [        ]    [        ]");
        Console.WriteLine("[        ]    [        ]    [        ]    [        ]");
        Console.WriteLine("[        ]    [        ]    [        ]    [        ]");
        Console.WriteLine();
        Console.WriteLine("[        ]    [        ]    [        ]    [        ]");
        Console.WriteLine("[        ]    [        ]    [        ]    [        ]");
        Console.WriteLine("[        ]    [        ]    [        ]    [        ]");
        Console.WriteLine();
        Console.WriteLine("[        ]    [        ]    [        ]    [        ]");
        Console.WriteLine("[        ]    [        ]    [        ]    [        ]");
        Console.WriteLine("[        ]    [        ]    [        ]    [        ]");
    }

    public void Compress()
    {
        for (int i = 0; i < 16; i+=4)
        {
            for (int j = 2; j >= 0; j--)
            {
                if (_grid[i + j] == 0) continue;
        
                for (int k = 1; k <= 3-j; k++)
                {
                    if (_grid[i + j + k] != 0) break;
                    _grid[i + j + k] = _grid[i + j + k - 1];
                    _grid[i + j + k - 1] = 0;
                }
            }
        }
    }

    public void Compress2()
    {
        copy = _grid.Clone() as int[];
        int bound = 15;
        int destination = -1;
        for (int i = 15; i > -1; i--)
        {
            if (i <= bound - 4) bound -= 4;
            if (_grid[i] > 0 && destination != -1 && destination <= bound)
            {
                _movingTiles.Add(new Tile(i, destination, _grid[i]));
                copy[i] = 0;
                _grid[destination] = _grid[i];
                _grid[i] = 0;
                for (int j = destination; j > 0; j--)
                {
                    if (_grid[j] == 0 && j <= bound)
                    {
                        destination = j;
                        break;
                    }
                }
            }
            else if (_grid[i] == 0 && (destination > bound || destination == -1)) destination = i;
            // Print2();
            // Console.Write($" destination: {destination} bound: {bound} i: {i}");
            // Console.WriteLine();
        }
    }

    public void Merge()
    {
        for (int i = 15; i > 0; i--)
        {
            if (i % 4 == 0) continue;
            if (_grid[i] != 0 && _grid[i] == _grid[i - 1])
            {
                _grid[i] *= 2;
                _grid[i - 1] = 0;
                i--;
            }
        }
    }
    
    public void Rotate(int[] grid)
    {
        int a = 0, b = 12, c = 15, d = 3;
        // (_grid[0], _grid[12]) = (_grid[12], _grid[0]);
        // (_grid[15], _grid[12]) = (_grid[12], _grid[15]);
        // (_grid[15], _grid[3]) = (_grid[3], _grid[15]);
        //
        // (_grid[1], _grid[8]) = (_grid[8], _grid[1]);
        // (_grid[14], _grid[8]) = (_grid[8], _grid[14]);
        // (_grid[14], _grid[7]) = (_grid[7], _grid[14]);
        //
        // (_grid[2], _grid[4]) = (_grid[4], _grid[2]);
        // (_grid[13], _grid[4]) = (_grid[4], _grid[13]);
        // (_grid[13], _grid[11]) = (_grid[11], _grid[13]);

        (grid[5], grid[9]) = (grid[9], grid[5]);
        (grid[10], grid[9]) = (grid[9], grid[10]);
        (grid[10], grid[6]) = (grid[6], grid[10]);


        for (int i = 0; i < 3; i++)
        {
            (grid[a], grid[b]) = (grid[b], grid[a]);
            (grid[c], grid[b]) = (grid[b], grid[c]);
            (grid[c], grid[d]) = (grid[d], grid[c]);
            a++;
            b -= 4;
            c--;
            d += 4;
        }

    }

    public void AnimateMove()
    {
        for (int i = 0; i < 5; i++)
        {
            Print(copy);
            PrintAnimationFrame();
            Thread.Sleep(100);
        }
        //Print(_grid);
        _movingTiles.Clear();
    }

    public void PrintAnimationFrame()
    {
        foreach (var tile in _movingTiles)
        {
            tile.Print();
            tile.Move();
        }
    }
    
    public void Spawn()
    {
        Random rnd = new Random();
        List<int> index = new List<int>();
        for (int i = 0; i < 16; i++)
        {
            if (_grid[i] == 0) index.Add(i);
        }

        int value = rnd.Next(1, 11) == 10 ? 4 : 2;
        _grid[index[rnd.Next(0, index.Count)]] = value;
    }

    //dumm gelöst, vielleicht kopieren von grid und compress trennen um unnötiges rotieren zu vermeiden
    public void Up()
    {
        Rotate(_grid);
        Compress2();
        Rotate(copy);
        Rotate(copy);
        Rotate(copy);
        foreach (var tile in _movingTiles)
        {
            tile.Rotate(1);
        }
        AnimateMove();
        Merge();
        Compress2();
        Rotate(copy);
        Rotate(copy);
        Rotate(copy);
        Rotate(_grid);
        Rotate(_grid);
        Rotate(_grid);
        foreach (var tile in _movingTiles)
        {
            tile.Rotate(1);
        }
        AnimateMove();
        Print(_grid);
    }
    
    public void Down()
    {
        Rotate(_grid);
        Rotate(_grid);
        Rotate(_grid);
        Compress2();
        Rotate(copy);
        
        foreach (var tile in _movingTiles)
        {
            tile.Rotate(3);
        }
        AnimateMove();
        Merge();
        Compress2();
        Rotate(copy);
        Rotate(_grid);
        foreach (var tile in _movingTiles)
        {
            tile.Rotate(3);
        }
        AnimateMove();
        Print(_grid);
    }
    
    public void Right()
    {
        Compress2();
        AnimateMove();
        Merge();
        Compress2();
        AnimateMove();
        Print(_grid);
    }
    
    public void Left()
    {
        Rotate(_grid);
        Rotate(_grid);
        Compress2();
        Rotate(copy);
        Rotate(copy);
        
        foreach (var tile in _movingTiles)
        {
            tile.Rotate(2);
        }
        AnimateMove();
        Merge();
        Compress2();
        Rotate(copy);
        Rotate(copy);
        Rotate(_grid);
        Rotate(_grid);
        foreach (var tile in _movingTiles)
        {
            tile.Rotate(2);
        }
        AnimateMove();
        Print(_grid);

    }
    
    
}