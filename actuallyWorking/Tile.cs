namespace working;

public class Tile
{
    private int start, destination, value, x, y;


    public Tile(int start, int destination, int value)
    {
        this.start = start;
        this.destination = destination;
        this.value = value;
        x = start % 4 * 14;
        y = start / 4 * 4;
    }

    public void Print()
    {
        Console.ForegroundColor = GetColor(value);
        Console.SetCursorPosition(x, y);
        Console.Write("[        ]");
        Console.SetCursorPosition(x, y+1);
        Console.Write($"[{value, 8}]");
        Console.SetCursorPosition(x, y+2);
        Console.Write("[        ]");
    }

    public void Move()
    {
        x += (destination % 4 * 14 - start % 4 * 14) / 4;
        y += (destination - start) / 4;
    }

    public void Rotate(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            start = GetNumber(start);
            destination = GetNumber(destination);
        }
        x = start % 4 * 14;
        y = start / 4 * 4;
    }

    public int GetNumber(int number)
    {
        return number switch
        {
            0 => 12,
            1 => 8,
            2 => 4,
            3 => 0,
            4 => 13,
            5 => 9,
            6 => 5,
            7 => 1,
            8 => 14,
            9 => 10,
            10 => 6,
            11 => 2,
            12 => 15,
            13 => 11,
            14 => 7,
            15 => 3,
            _ => -1
        };
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
}