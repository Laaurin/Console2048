using System;
using System.Diagnostics;

namespace working;

class Programm
{
    public static void Main(String[] args)
    {
        int[] a =
        {
            0, 0, 0, 0,
            0, 0, 2, 0,
            0, 0, 2, 0,
            0, 2, 2, 0,
        };

        int[] b =
        {
            4, 4, 4, 4,
            0, 2, 0, 0,
            0, 2, 0, 2,
            0, 0, 0, 2
        };

        Game game = new Game(a);
        
        game.Play();
        
        Console.SetCursorPosition(0, 25);

    }
}