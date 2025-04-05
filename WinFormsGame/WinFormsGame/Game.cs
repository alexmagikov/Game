namespace WinFormsApp1;

public class MyGame
{
    private string[]? map;
    private const char FreeChar = '_';
    private const char WallChar = '#';
    private const char PlayerChar = '@';

    private int currentXPosition = -1;
    private int currentYPosition = -1;

    public void OnLeft(object sender, EventArgs args)
    {
        if (currentXPosition >= 0 && map != null &&
            currentYPosition >= 0 && currentYPosition < map.Length &&
            map[currentYPosition][currentXPosition - 1] != WallChar)
        {
            UpdatePosition(currentXPosition - 1, currentYPosition);
        }
    }

    public void OnRight(object sender, EventArgs args)
    {
        if (map != null && currentXPosition >= 0 &&
            currentXPosition < map[currentYPosition].Length - 1 &&
            map[currentYPosition][currentXPosition + 1] != WallChar)
        {
            UpdatePosition(currentXPosition + 1, currentYPosition);
        }
    }

    public void OnUp(object sender, EventArgs args)
    {
        if (currentYPosition > 0 && map != null &&
            currentXPosition >= 0 && currentXPosition < map[currentYPosition - 1].Length &&
            map[currentYPosition - 1][currentXPosition] != WallChar)
        {
            UpdatePosition(currentXPosition, currentYPosition - 1);
        }
    }

    public void OnDown(object sender, EventArgs args)
    {
        if (map != null && currentYPosition < map.Length - 1 &&
            currentXPosition >= 0 && currentXPosition < map[currentYPosition + 1].Length &&
            map[currentYPosition + 1][currentXPosition] != WallChar)
        {
            UpdatePosition(currentXPosition, currentYPosition + 1);
        }
    }

    private void UpdatePosition(int newX, int newY)
    {

        Console.SetCursorPosition(currentXPosition, currentYPosition);
        Console.Write(FreeChar);

        currentXPosition = newX;
        currentYPosition = newY;
        Console.SetCursorPosition(currentXPosition, currentYPosition);
        Console.Write(PlayerChar);
        Console.SetCursorPosition(currentXPosition, currentYPosition);
    }

    public void WithdrawMap()
    {
        string projectDir = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        string mapPath = Path.Combine(projectDir, "map.txt");
        string[] mapLines = File.ReadAllLines(mapPath);
        if (mapLines.Length == 0)
        {
            throw new FormatException("Null map");
        }
        this.map = new string[mapLines.Length];

        for (int i = 0; i < mapLines.Length; i++)
        {
            Console.WriteLine(mapLines[i]);
            this.map[i] = mapLines[i];

            int positionX = mapLines[i].IndexOf(PlayerChar);
            if (positionX != -1)
            {
                this.currentXPosition = positionX;
                this.currentYPosition = i;
            }
        }
        Console.SetCursorPosition(currentXPosition, currentYPosition);
        if (currentXPosition == -1 || currentYPosition == -1)
        {
            throw new Exception("Player character not found on the map.");
        }
    }
}
