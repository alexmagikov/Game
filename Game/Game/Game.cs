using System.ComponentModel.DataAnnotations;

namespace Game;

public class Game
{
    public void OnLeft(object sender, EventArgs args)
    => Console.WriteLine("Going left");
    public void OnRight(object sender, EventArgs args)
    => Console.WriteLine("Going right");
    public void withdrawMap()
    {
        string[] map = File.ReadAllLines("map.txt");

        foreach (var lines in map)
        {
            Console.WriteLine(lines);
        }
    }
}
