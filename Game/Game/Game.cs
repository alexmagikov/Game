namespace Game;

public class MyGame
{
    public int currentXPosition = 0;
    public int currentYPosition = 0;
    public char currentChar;
    public int mapSizeX = 0;
    public int mapSizeY = 0;
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
    public void withdrawMap()
    {

    }
}
