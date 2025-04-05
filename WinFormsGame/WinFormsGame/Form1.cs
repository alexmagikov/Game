using System.Text;

namespace WinFormsGame
{
    public partial class Form1 : Form
    {
        private string[] map;
        private Point playerPosition;
        private Image playerImage;
        private Panel controlsPanel;
        private int size = 64;

        public Form1()
        {
            InitializeComponent();

            this.DoubleBuffered = true;

            string projectDir = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            string mapPath = Path.Combine(projectDir, "map.txt");
            string imgPath = Path.Combine(projectDir, "z.png");
            playerImage = Image.FromFile(imgPath);
            LoadMap(mapPath);

            this.MinimumSize = new Size(map[0].Length * size, map.Length * size + size * 4);
            this.MaximumSize = new Size(map[0].Length * size, map.Length * size + size * 4);
            this.Size = new Size(map[0].Length * size, (map.Length + 1) * size + size * 4);

            CreateControlsPanel();
            this.Paint += ControlPaint;
            this.KeyDown += OnKeyDown;

        }

        private void CreateControlsPanel()
        {
            controlsPanel = new Panel
            {
                Width = size * 6,
                Height = size * 3,
                TabStop = false,
            };
 
            var arrowButtons = new[]
            {
                new { Key = Keys.Left, Text = "←", X = 1, Y = 1 },
                new { Key = Keys.Up, Text = "↑", X = 2, Y = 0 },
                new { Key = Keys.Down, Text = "↓", X = 2, Y = 2 },
                new { Key = Keys.Right, Text = "→", X = 3, Y = 1 }
            };


            foreach (var btnInfo in arrowButtons)
            {
                var button = new Button
                {
                    Text = btnInfo.Text,
                    Size = new Size(size, size),
                    Location = new Point(
                        btnInfo.X * size,
                        btnInfo.Y * size),
                    Tag = btnInfo.Key,
                };

                button.Click += (sender, e) =>
                {
                    var key = (Keys)((Button)sender).Tag;
                    OnKeyDown(this, new KeyEventArgs(key));
                };

                controlsPanel.Controls.Add(button);
                controlsPanel.Left = (this.ClientSize.Width - controlsPanel.Width) / 2;
                controlsPanel.Top = this.ClientSize.Height - controlsPanel.Height;
                this.Controls.Add(controlsPanel);
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            var currentX = playerPosition.X;
            var currentY = playerPosition.Y;
            var isCanGo = false;
            switch (e.KeyCode)
            {
                case Keys.Up:
                    if (playerPosition.Y - 1 != 0
                        && map[playerPosition.Y - 1][playerPosition.X] != '#')
                    {
                        playerPosition.Y -= 1;
                        isCanGo = true;
                    }
                    break;
                case Keys.Down:
                    if (playerPosition.Y + 1 != map.Length
                        && map[playerPosition.Y + 1][playerPosition.X] != '#')
                    {
                        playerPosition.Y += 1;
                        isCanGo = true;
                    }
                    break;
                case Keys.Left:
                    if (playerPosition.X - 1 != 0
                        && map[playerPosition.Y][playerPosition.X - 1] != '#')
                    {
                        playerPosition.X -= 1;
                        isCanGo = true;
                    }
                    break;
                case Keys.Right:
                    if (playerPosition.X + 1 != map[0].Length
                        && map[playerPosition.Y][playerPosition.X + 1] != '#')
                    {
                        playerPosition.X += 1;
                        isCanGo = true;
                    }
                    break;
            }

            if (isCanGo)
            {
                var sb = new StringBuilder(map[currentY]);
                sb[currentX] = '_';
                map[currentY] = sb.ToString();
                sb = new StringBuilder(map[playerPosition.Y]);
                sb[playerPosition.X] = '@';
                map[playerPosition.Y] = sb.ToString();
                this.Invalidate();
            }
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
                    Rectangle rect = new Rectangle( x * size, y * size, size, size);
                    e.Graphics.FillRectangle(Brushes.Black, rect);
                    switch (map[y][x])
                    {
                        case '#':
                            e.Graphics.FillRectangle(Brushes.Black, rect);
                            break;
                        case '_':
                            e.Graphics.FillRectangle(Brushes.White, rect);
                            break;
                        case '@':
                            e.Graphics.FillRectangle(Brushes.White, rect);
                            e.Graphics.DrawImage(playerImage, new Rectangle(x * size, y * size, size, size));
                            break;
                    }
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Up || keyData == Keys.Down ||
                keyData == Keys.Left || keyData == Keys.Right)
            {
                OnKeyDown(this, new KeyEventArgs(keyData));
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
