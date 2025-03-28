﻿using System.ComponentModel.DataAnnotations;

namespace Game;

public class Game
{
    public int currentXPosition = 0;
    public int currentYPosition = 0;
    public char currentChar;
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
    public static (int positionX, int positionY) WithdrawMap()
    {
        string[] map = File.ReadAllLines("map.txt");
        var counter = 0;
        foreach (var lines in map)
        {
            Console.WriteLine(lines);
            var positionX = lines.IndexOf('@');
            if (positionX != -1)
            {
                return (positionX, counter);
            }
            counter++;
        }
        return (-1, -1);
    }
}
