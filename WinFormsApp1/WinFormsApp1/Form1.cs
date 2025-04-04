namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private Bitmap buffer;
        private Graphics bufferGraphics;
        private Graphics screenGraphics;

        // ����� ������ (1 - �����, 0 - ������ ������������)
        private int[,] map = new int[,]
        {
            {1, 1, 1, 1, 1},
            {1, 0, 0, 0, 1},
            {1, 0, 0, 0, 1},
            {1, 0, 0, 0, 1},
            {1, 0, 0, 0, 1},
            {1, 0, 0, 0, 1},
            {1, 0, 0, 0, 1},
            {1, 0, 0, 0, 1},
            {1, 1, 1, 1, 1},
        };

        // ������� � ����������� ������
        private float playerX = 2.5f;
        private float playerY = 2.5f;
        private float playerAngle = 0f;

        // ��������� �����������
        private const int cellSize = 64;
        private const int rayCount = 60;
        private const float fov = (float)(Math.PI / 3); // 60 ��������

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.ClientSize = new Size(800, 600);
            this.Text = "Simple Doom";

            buffer = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            bufferGraphics = Graphics.FromImage(buffer);
            screenGraphics = this.CreateGraphics();

            // ����������� �������
            this.KeyDown += Form1_KeyDown;
            this.Paint += Form1_Paint;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Render();
        }

        private void Render()
        {
            bufferGraphics.Clear(Color.Black);

            // �������� 3D ���
            Render3DView();

            // �������� ����-�����
            RenderMinimap();

            screenGraphics.DrawImage(buffer, 0, 0);
        }

        private void Render3DView()
        {
            int halfHeight = this.ClientSize.Height / 2;

            for (int i = 0; i < rayCount; i++)
            {
                // ��������� ���� ��� �������� ����
                float rayAngle = playerAngle - fov / 2 + fov * i / rayCount;

                // ������� ���
                (float dist, bool isVertical) = CastRay(rayAngle);

                // ������������ ��������� "����� ����"
                dist *= (float)Math.Cos(rayAngle - playerAngle);

                // ��������� ������ �����
                int wallHeight = (int)(this.ClientSize.Height / dist);

                // ��������� ������� � ������ ����� �����
                int wallTop = halfHeight - wallHeight / 2;
                int wallBottom = halfHeight + wallHeight / 2;

                // �������� ���� � ����������� �� ���� �����
                Color wallColor = isVertical ? Color.DarkGray : Color.Gray;

                // ������ ������ �����
                using (Pen pen = new Pen(wallColor))
                {
                    int stripWidth = this.ClientSize.Width / rayCount;
                    bufferGraphics.DrawLine(pen,
                        i * stripWidth, wallTop,
                        i * stripWidth, wallBottom);
                }

                // ������ ��� (������� �������)
                using (Pen pen = new Pen(Color.DarkGreen))
                {
                    bufferGraphics.DrawLine(pen,
                        i * (this.ClientSize.Width / rayCount), wallBottom,
                        i * (this.ClientSize.Width / rayCount), this.ClientSize.Height);
                }

                // ������ ������� (������� �������)
                using (Pen pen = new Pen(Color.DarkBlue))
                {
                    bufferGraphics.DrawLine(pen,
                        i * (this.ClientSize.Width / rayCount), 0,
                        i * (this.ClientSize.Width / rayCount), wallTop);
                }
            }
        }

        private (float distance, bool isVertical) CastRay(float angle)
        {
            // ����������� ����
            angle = NormalizeAngle(angle);

            // ����������� ����
            float rayDirX = (float)Math.Cos(angle);
            float rayDirY = (float)Math.Sin(angle);

            // ������� ������ �� �����
            int mapX = (int)playerX;
            int mapY = (int)playerY;

            // ����� ���� �� ������� ������� �� ��������� ����� x ��� y
            float sideDistX, sideDistY;

            // ����� ���� �� ����� ����� x ��� y �� ���������
            float deltaDistX = Math.Abs(1 / rayDirX);
            float deltaDistY = Math.Abs(1 / rayDirY);

            // ����������� ���� (������/�����, �����/����)
            int stepX, stepY;

            bool hit = false;
            bool isVertical = false;

            // ��������� ��� � ��������� sideDist
            if (rayDirX < 0)
            {
                stepX = -1;
                sideDistX = (playerX - mapX) * deltaDistX;
            }
            else
            {
                stepX = 1;
                sideDistX = (mapX + 1.0f - playerX) * deltaDistX;
            }

            if (rayDirY < 0)
            {
                stepY = -1;
                sideDistY = (playerY - mapY) * deltaDistY;
            }
            else
            {
                stepY = 1;
                sideDistY = (mapY + 1.0f - playerY) * deltaDistY;
            }

            // �������� DDA (Digital Differential Analysis)
            while (!hit)
            {
                // ������� � ��������� �����
                if (sideDistX < sideDistY)
                {
                    sideDistX += deltaDistX;
                    mapX += stepX;
                    isVertical = false;
                }
                else
                {
                    sideDistY += deltaDistY;
                    mapY += stepY;
                    isVertical = true;
                }

                // ���������, ����� �� ��� � �����
                if (mapX < 0 || mapX >= map.GetLength(0) ||
                    mapY < 0 || mapY >= map.GetLength(1))
                {
                    hit = true; // ����� �� ������� �����
                }
                else if (map[mapX, mapY] > 0)
                {
                    hit = true; // ������ � �����
                }
            }

            // ��������� ���������� �� �����
            float distance;
            if (!isVertical)
            {
                distance = (mapX - playerX + (1 - stepX) / 2) / rayDirX;
            }
            else
            {
                distance = (mapY - playerY + (1 - stepY) / 2) / rayDirY;
            }

            return (Math.Abs(distance), isVertical);
        }

        private void RenderMinimap()
        {
            int minimapSize = 150;
            int minimapX = this.ClientSize.Width - minimapSize - 10;
            int minimapY = 10;

            // ������ ��� ����-�����
            bufferGraphics.FillRectangle(Brushes.Black, minimapX, minimapY, minimapSize, minimapSize);

            // ������ ������ �����
            int cellSizeMinimap = minimapSize / Math.Max(map.GetLength(0), map.GetLength(1));

            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    if (map[x, y] > 0)
                    {
                        bufferGraphics.FillRectangle(Brushes.Gray,
                            minimapX + x * cellSizeMinimap,
                            minimapY + y * cellSizeMinimap,
                            cellSizeMinimap, cellSizeMinimap);
                    }
                }
            }

            // ������ ������ �� ����-�����
            int playerSize = 5;
            bufferGraphics.FillEllipse(Brushes.Red,
                minimapX + (int)(playerX * cellSizeMinimap) - playerSize / 2,
                minimapY + (int)(playerY * cellSizeMinimap) - playerSize / 2,
                playerSize, playerSize);

            // ������ ����������� ������� ������
            bufferGraphics.DrawLine(Pens.Yellow,
                minimapX + (int)(playerX * cellSizeMinimap),
                minimapY + (int)(playerY * cellSizeMinimap),
                minimapX + (int)((playerX + Math.Cos(playerAngle)) * cellSizeMinimap),
                minimapY + (int)((playerY + Math.Sin(playerAngle)) * cellSizeMinimap));
        }

        private float NormalizeAngle(float angle)
        {
            while (angle < 0) angle += 2 * (float)Math.PI;
            while (angle >= 2 * (float)Math.PI) angle -= 2 * (float)Math.PI;
            return angle;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            float moveSpeed = 0.1f;
            float rotSpeed = 0.1f;

            switch (e.KeyCode)
            {
                case Keys.W:
                    // �������� ������
                    float newX = playerX + (float)Math.Cos(playerAngle) * moveSpeed;
                    float newY = playerY + (float)Math.Sin(playerAngle) * moveSpeed;
                    if (map[(int)newX, (int)playerY] == 0) playerX = newX;
                    if (map[(int)playerX, (int)newY] == 0) playerY = newY;
                    break;

                case Keys.S:
                    // �������� �����
                    newX = playerX - (float)Math.Cos(playerAngle) * moveSpeed;
                    newY = playerY - (float)Math.Sin(playerAngle) * moveSpeed;
                    if (map[(int)newX, (int)playerY] == 0) playerX = newX;
                    if (map[(int)playerX, (int)newY] == 0) playerY = newY;
                    break;

                case Keys.A:
                    // ������� �����
                    playerAngle -= rotSpeed;
                    break;

                case Keys.D:
                    // ������� ������
                    playerAngle += rotSpeed;
                    break;

                case Keys.Q:
                    // ����� �����
                    newX = playerX + (float)Math.Cos(playerAngle - Math.PI / 2) * moveSpeed;
                    newY = playerY + (float)Math.Sin(playerAngle - Math.PI / 2) * moveSpeed;
                    if (map[(int)newX, (int)playerY] == 0) playerX = newX;
                    if (map[(int)playerX, (int)newY] == 0) playerY = newY;
                    break;

                case Keys.E:
                    // ����� ������
                    newX = playerX + (float)Math.Cos(playerAngle + Math.PI / 2) * moveSpeed;
                    newY = playerY + (float)Math.Sin(playerAngle + Math.PI / 2) * moveSpeed;
                    if (map[(int)newX, (int)playerY] == 0) playerX = newX;
                    if (map[(int)playerX, (int)newY] == 0) playerY = newY;
                    break;
            }

            // �������������� �����
            Render();
        }
    }
}
