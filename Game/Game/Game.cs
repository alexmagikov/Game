using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Game;

public class MyGame
{
    private string[]? map; 
    private char freeChar = '_';

    private int currentXPosition = -1;
    private int currentYPosition = -1;
    public void OnLeft(object sender, EventArgs args)
    {
        if (!(map[currentYPosition][currentXPosition - 1] == '#'))
        {
            Console.Write(freeChar);
            Console.CursorLeft--;
            Console.Write('@');
            currentXPosition--;

        }
    }
    public void OnRight(object sender, EventArgs args)
    {
        Console.CursorLeft++;
    }
    public void OnUp(object sender, EventArgs args)
    {
        Console.CursorTop++;
    }
    public void OnDown(object sender, EventArgs args)
    {
        Console.CursorTop--;
    }
    public void WithdrawMap()
    {
        string[] mapLines = File.ReadAllLines("C:\\Users\\User\\source\\repos\\Game\\Game\\Game\\map.txt");
        var counter = 0;
        var indexLine = 0;
        this.map = new string[mapLines.Length];
        foreach (var lines in mapLines)
        {
            Console.WriteLine(lines);
            this.map[indexLine] = new string(lines);
            var positionX = lines.IndexOf('@');
            if (positionX != -1)
            {
                this.currentXPosition = positionX;
                this.currentYPosition = counter;
                return;
            }
            counter++;
        }
        Console.CursorTop--;
        //for (int i = 0; i < currentXPosition; i++)
        //{
        //    Console.CursorLeft--;
        //}
        //for (int i = 0; i < currentYPosition; i++)
        //{
        //    Console.CursorTop--;
        //}
        return;
    }
}
