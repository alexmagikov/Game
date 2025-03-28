using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Game;

public class MyGame
{
<<<<<<< HEAD
    public int currentXPosition = 0;
    public int currentYPosition = 0;
    public char currentChar;
    public int mapSizeX = 0;
    public int mapSizeY = 0;
=======
    private int currentXPosition = -1;
    private int currentYPosition = -1;
    private char currentChar;
>>>>>>> ae86374bcf527a89d727d505344c9d7ea7b10fcf
    public void OnLeft(object sender, EventArgs args)
    {
        Console.CursorLeft--;
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
        string[] map = File.ReadAllLines("map.txt");
        var counter = 0;
        foreach (var lines in map)
        {
            Console.WriteLine(lines);
            var positionX = lines.IndexOf('@');
            if (positionX != -1)
            {
                this.currentXPosition = positionX;
                this.currentYPosition = counter;
                return;
            }
            counter++;
        }
        return;
    }
}
