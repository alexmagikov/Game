using Game;

var eventLoop = new EventLoop();
var game = new MyGame();
game.WithdrawMap();
eventLoop.LeftHendler += game.OnLeft;
eventLoop.RightHendler += game.OnRight;
eventLoop.UpHendler += game.OnUp;
eventLoop.DownHendler += game.OnDown;
eventLoop.Run();