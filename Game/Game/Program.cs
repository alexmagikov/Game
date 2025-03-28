using Game;

//var path = "C:\\Users\\User\\source\\repos\\Game\\Game\\Game\\map.txt";
//using (var reader = new StreamReader(path));

var eventLoop = new EventLoop();
var game = new MyGame();
eventLoop.LeftHendler += game.OnLeft;
eventLoop.RightHendler += game.OnRight;
eventLoop.UpHendler += game.OnUp;
eventLoop.DownHendler += game.OnDown;


public class Game
{

}

public class EventLoop
{
    public event EventHandler<EventArgs> LeftHendler = (sender, args) => { };
    public event EventHandler<EventArgs> RightHendler = (sender, args) => { };
    public event EventHandler<EventArgs> UpHendler = (sender, args) => { };
    public event EventHandler<EventArgs> DownHendler = (sender, args) => { };
    public void Run()
    {
        while (true)
        {
            var key = Console.ReadKey(true);
            switch (key.Key) 
            {
                case ConsoleKey.LeftArrow:
                    LeftHendler(this, EventArgs.Empty);
                    break;
                case ConsoleKey.RightArrow:
                    RightHendler(this, EventArgs.Empty);
                    break;
                case ConsoleKey.UpArrow:
                    UpHendler(this, EventArgs.Empty);
                    break;
                case ConsoleKey.DownArrow:
                    DownHendler(this, EventArgs.Empty);
                    break;
            }
    }
}