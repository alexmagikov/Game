using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Game;

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



private string[] map;
    private Point playerPosition;
    private Image playerImage;
    private Image wallImage;
    private int size = 64;

    public Form1()
    {
        InitializeComponent();
        this.DoubleBuffered = true;
        this.Text = "Tile Map Game";

        string projectDir = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        string mapPath = Path.Combine(projectDir, "map.txt");

        Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
        int maxWidth = workingArea.Width;
        int maxHeight = workingArea.Height;
        size = Math.Max(maxHeight / map.Length, maxHeight / map[0].Length);

        LoadMap(mapPath);
        this.Paint += ControlPaint;
        this.Invalidate();
    }



    private void LoadMap(string filePath)
    {
        map = File.ReadAllLines(filePath);

        for (int y = 0; y < map.Length; y++)
        {
            for (int x = 0; x < map[y].Length; x++)
            {
                if (map[y][x] == '@')
                {
                    playerPosition = new Point(x, y);
                }
            }
        }
    }

    private void ControlPaint(object sender, System.Windows.Forms.PaintEventArgs e)
    {
        //e.Graphics.DrawString(map[1][0].ToString(), new Font("Arial", 10), System.Drawing.Brushes.Blue, new Point(30, 30));
        for (int y = 0; y < map.Length; y++)
        {
            for (int x = 0; x < map[0].Length; x++)
            {
                Rectangle rect = new Rectangle(
                    x * size,
                    y * size,
                    size,
                    size);
                e.Graphics.FillRectangle(Brushes.Black, rect);
                switch (map[y][x])
                {
                    case '#':
                        e.Graphics.FillRectangle(Brushes.Black, rect);
                        break;
                    case '_':
                        e.Graphics.FillRectangle(Brushes.White, rect);
                        break;
                    case ' ':
                        e.Graphics.DrawRectangle(Pens.Gray, rect);
                        break;
                }
            }
        }
    }
