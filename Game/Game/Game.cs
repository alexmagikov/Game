using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Game;

public class Game
{
    private int currentXPosition = -1;
    private int currentYPosition = -1;
    private char currentChar;
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

    }
    public void OnDown(object sender, EventArgs args)
    {

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
