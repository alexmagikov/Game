
var path = "C:\\Users\\User\\source\\repos\\Game\\Game\\Game\\map.txt";
using (var reader = new StreamReader(path));

var eventLoop = new EventLoop();


public class Game
{

}

public class EventLoop
{
    public void Run()
    {
        while (true)
        {
            var key = Console.ReadKey(true);
            switch (key) 
            {
                case ConsoleKey.LeftArrow:
                    LeftHandler
            }
    }
}