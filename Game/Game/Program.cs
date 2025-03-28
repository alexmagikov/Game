using Game;

var path = "C:\\Users\\User\\source\\repos\\Game\\Game\\Game\\map.txt";
using (var reader = new StreamReader(path));

var eventLoop = new EventLoop();
var game = new MyGame();
eventLoop.LeftHendler += game.OnLeft;
eventLoop.RightHendler += game.OnRight;
eventLoop.UpHendler += game.OnUp;
eventLoop.DownHendler += game.OnDown;

