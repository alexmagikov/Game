namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private Bitmap buffer;
        private Graphics bufferGraphics;
        private Graphics screenGraphics;

        // Карта уровня (1 - стена, 0 - пустое пространство)
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

        // Позиция и направление игрока
        private float playerX = 2.5f;
        private float playerY = 2.5f;
        private float playerAngle = 0f;

        // Настройки отображения
        private const int cellSize = 64;
        private const int rayCount = 60;
        private const float fov = (float)(Math.PI / 3); // 60 градусов

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.ClientSize = new Size(800, 600);
            this.Text = "Simple Doom";

            buffer = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            bufferGraphics = Graphics.FromImage(buffer);
            screenGraphics = this.CreateGraphics();

            // Обработчики событий
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

            // Рендерим 3D вид
            Render3DView();

            // Рендерим мини-карту
            RenderMinimap();

            screenGraphics.DrawImage(buffer, 0, 0);
        }

        private void Render3DView()
        {
            int halfHeight = this.ClientSize.Height / 2;

            for (int i = 0; i < rayCount; i++)
            {
                // Вычисляем угол для текущего луча
                float rayAngle = playerAngle - fov / 2 + fov * i / rayCount;

                // Бросаем луч
                (float dist, bool isVertical) = CastRay(rayAngle);

                // Корректируем искажение "рыбий глаз"
                dist *= (float)Math.Cos(rayAngle - playerAngle);

                // Вычисляем высоту стены
                int wallHeight = (int)(this.ClientSize.Height / dist);

                // Вычисляем верхнюю и нижнюю точки стены
                int wallTop = halfHeight - wallHeight / 2;
                int wallBottom = halfHeight + wallHeight / 2;

                // Выбираем цвет в зависимости от типа стены
                Color wallColor = isVertical ? Color.DarkGray : Color.Gray;

                // Рисуем полосу стены
                using (Pen pen = new Pen(wallColor))
                {
                    int stripWidth = this.ClientSize.Width / rayCount;
                    bufferGraphics.DrawLine(pen,
                        i * stripWidth, wallTop,
                        i * stripWidth, wallBottom);
                }

                // Рисуем пол (простой вариант)
                using (Pen pen = new Pen(Color.DarkGreen))
                {
                    bufferGraphics.DrawLine(pen,
                        i * (this.ClientSize.Width / rayCount), wallBottom,
                        i * (this.ClientSize.Width / rayCount), this.ClientSize.Height);
                }

                // Рисуем потолок (простой вариант)
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
            // Нормализуем угол
            angle = NormalizeAngle(angle);

            // Направление луча
            float rayDirX = (float)Math.Cos(angle);
            float rayDirY = (float)Math.Sin(angle);

            // Позиция игрока на карте
            int mapX = (int)playerX;
            int mapY = (int)playerY;

            // Длина луча от текущей позиции до следующей линии x или y
            float sideDistX, sideDistY;

            // Длина луча от одной линии x или y до следующей
            float deltaDistX = Math.Abs(1 / rayDirX);
            float deltaDistY = Math.Abs(1 / rayDirY);

            // Направление шага (вправо/влево, вверх/вниз)
            int stepX, stepY;

            bool hit = false;
            bool isVertical = false;

            // Вычисляем шаг и начальную sideDist
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

            // Алгоритм DDA (Digital Differential Analysis)
            while (!hit)
            {
                // Переход к следующей линии
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

                // Проверяем, попал ли луч в стену
                if (mapX < 0 || mapX >= map.GetLength(0) ||
                    mapY < 0 || mapY >= map.GetLength(1))
                {
                    hit = true; // Выход за пределы карты
                }
                else if (map[mapX, mapY] > 0)
                {
                    hit = true; // Попали в стену
                }
            }

            // Вычисляем расстояние до стены
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

            // Рисуем фон мини-карты
            bufferGraphics.FillRectangle(Brushes.Black, minimapX, minimapY, minimapSize, minimapSize);

            // Рисуем клетки карты
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

            // Рисуем игрока на мини-карте
            int playerSize = 5;
            bufferGraphics.FillEllipse(Brushes.Red,
                minimapX + (int)(playerX * cellSizeMinimap) - playerSize / 2,
                minimapY + (int)(playerY * cellSizeMinimap) - playerSize / 2,
                playerSize, playerSize);

            // Рисуем направление взгляда игрока
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
                    // Движение вперед
                    float newX = playerX + (float)Math.Cos(playerAngle) * moveSpeed;
                    float newY = playerY + (float)Math.Sin(playerAngle) * moveSpeed;
                    if (map[(int)newX, (int)playerY] == 0) playerX = newX;
                    if (map[(int)playerX, (int)newY] == 0) playerY = newY;
                    break;

                case Keys.S:
                    // Движение назад
                    newX = playerX - (float)Math.Cos(playerAngle) * moveSpeed;
                    newY = playerY - (float)Math.Sin(playerAngle) * moveSpeed;
                    if (map[(int)newX, (int)playerY] == 0) playerX = newX;
                    if (map[(int)playerX, (int)newY] == 0) playerY = newY;
                    break;

                case Keys.A:
                    // Поворот влево
                    playerAngle -= rotSpeed;
                    break;

                case Keys.D:
                    // Поворот вправо
                    playerAngle += rotSpeed;
                    break;

                case Keys.Q:
                    // Страф влево
                    newX = playerX + (float)Math.Cos(playerAngle - Math.PI / 2) * moveSpeed;
                    newY = playerY + (float)Math.Sin(playerAngle - Math.PI / 2) * moveSpeed;
                    if (map[(int)newX, (int)playerY] == 0) playerX = newX;
                    if (map[(int)playerX, (int)newY] == 0) playerY = newY;
                    break;

                case Keys.E:
                    // Страф вправо
                    newX = playerX + (float)Math.Cos(playerAngle + Math.PI / 2) * moveSpeed;
                    newY = playerY + (float)Math.Sin(playerAngle + Math.PI / 2) * moveSpeed;
                    if (map[(int)newX, (int)playerY] == 0) playerX = newX;
                    if (map[(int)playerX, (int)newY] == 0) playerY = newY;
                    break;
            }

            // Перерисовываем сцену
            Render();
        }
    }
}
