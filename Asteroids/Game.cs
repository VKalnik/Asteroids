using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Asteroids.Properties;

namespace Asteroids
{
    static class Game
    {
        private static BufferedGraphicsContext _context;
        public static BufferedGraphics Buffer;
        static List<BaseObject> _asteroids = new List<BaseObject>();
        static BaseObject[] _stars;
        static List<BaseObject> _comets = new List<BaseObject>();
        static List<Bullet> _bullets = new List<Bullet>();
        static Ship _ship;
        static Medkit _medkit;
        static Random random = new Random();
        static Timer timer;
        static int _score = 0;
        static bool medkitFlag;
        static Logger logger = GameLogger.DebugLogger;
        static int numberOfAsteroids = 3;
        static int currentOfAsteroids = numberOfAsteroids;
        static int numberOfComets = 1;
        static int numberOfStars = 40;
        static int Tick = 60;


        public static int Width { get; set; }
        public static int Height { get; set; }

        public static void CreateAsteroids(int n)
        {
            for (int i = 0; i < n; i++)
            {
                var size = random.Next(25, 55);
                var startPosX = random.Next(500, 799);
                var startPosY = random.Next(2, 590);
                var speed = random.Next(5, 10);
                _asteroids.Add(new Asteroid(new Point(startPosX, startPosY), new Point(-speed - 5, -speed / 3 - 1), new Size(size, size)));
                logger.Invoke($"Создаём астероид {i} Х: {_asteroids[i].Rect.X}, Y: {_asteroids[i].Rect.Y}, Size: {size}");
            }
        }

        public static void Init(Form form)
        {
            Graphics g = form.CreateGraphics();
            _context = BufferedGraphicsManager.Current;

            try
            {
                Width = form.ClientSize.Width;
                Height = form.ClientSize.Height;
                if (Height < 0 || Height > 1000 || Width < 0 || Width > 1000)
                {
                    throw new ArgumentOutOfRangeException("Неверный размер окна игры");
                }
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Debug.WriteLine(ex);
                Console.WriteLine(ex);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                Console.WriteLine(ex);
            }

            Buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height));

            Load();

            timer = new Timer();
            timer.Interval = Tick;
            timer.Start();
            timer.Tick += OnTime;
            form.KeyDown += Form_KeyDown;
        }

        private static void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Space)
            {
                { 
                    _bullets.Add(new Bullet(new Point(_ship.Rect.X + 10, _ship.Rect.Y + 15), new Point(0, 5), new Size(54, 9)));
                    logger.Invoke($"Создаём пулю. X: {_ship.Rect.X + 10}, Y: {_ship.Rect.Y + 15}");
                }
            }
            if (e.KeyCode == Keys.Up)
            {
                _ship.Up();
            }
            if (e.KeyCode == Keys.Down)
            {
                _ship.Down();
            }
        }

        private static void OnTime(object sender, EventArgs e)
        {
            Draw();
            Update();
        }

        public static void Draw()
        {
            Buffer.Graphics.Clear(Color.Black);

            Buffer.Graphics.DrawImage(Resources.background, new Rectangle(0, 0, 800, 600));

            foreach (BaseObject star in _stars)
                star.Draw();

            Buffer.Graphics.DrawImage(Resources.planet, new Rectangle(100, 100, 200, 200));

            foreach (Asteroid asteroid in _asteroids)
            {
                asteroid.Draw();
                logger.Invoke($"Астероид рисуем. X: {asteroid.Rect.X}, Y: {asteroid.Rect.Y}");

            }
                

            foreach (var comet in _comets)
                comet.Draw();

            foreach (var bullet in _bullets)
            {
                bullet.Draw();
            }

            if (_medkit != null)
            {
                _medkit.Draw();
            }

            if (_ship != null)
            {
                _ship.Draw();

                Buffer.Graphics.DrawString("Energy: " + _ship.Energy, SystemFonts.DefaultFont, Brushes.White, 0, 0);
                Buffer.Graphics.DrawString("Score: " + _score, SystemFonts.DefaultFont, Brushes.White, 0, 15);
            }

            Buffer.Render();
        }

        public static void Load()
        {

            //logger += GameLogger.ConsoleLogger; // Лог в Консоль
            //logger += GameLogger.FileLogger; // Лог в лог-файл 

            logger.Invoke("СТАРТ ИГРЫ");

            _ship = new Ship(new Point(10, 400), new Point(5, 5), new Size(45, 30));
            logger.Invoke($"Создаём корабль Х: {_ship.Rect.X}, Y: {_ship.Rect.Y}");
            
            Ship.DieEvent += Ship_DieEvent;

            CreateAsteroids(numberOfAsteroids);

            _stars = new BaseObject[numberOfStars];
            for (int i = 0; i < _stars.Length; i++)
            {
                var size = random.Next(5, 10);
                var startPosX = random.Next(2, 799);
                var startPosY = random.Next(2, 590);
                var speed = random.Next(1, 3);
                _stars[i] = new Star(new Point(startPosX, startPosY), new Point(-speed, 0), new Size(size, size));
                logger.Invoke($"Создаём звезду {i} Х: {_stars[i].Rect.X}, Y: {_stars[i].Rect.Y}, Size: {size}");
            }

            for (int i = 0; i < numberOfComets; i++)
            {
                var size = random.Next(90, 100);
                var startPosX = 800;
                var startPosY = random.Next(50, 550);
                var speed = random.Next(15, 25);
                _comets.Add(new Comet(new Point(startPosX, startPosY), new Point(-speed, 0), new Size(size * 3, size / 2)));
                logger.Invoke($"Создаём комету {i} Х: {_comets[i].Rect.X}, Y: {_comets[i].Rect.Y}");
            }
        }

        private static void Ship_DieEvent(object sender, EventArgs e)
        {
            timer.Stop();
            logger.Invoke($"Смерть корабля. X: {_ship.Rect.X}, Y: {_ship.Rect.Y}");
            Buffer.Graphics.DrawString("Game Over", new Font(FontFamily.GenericSansSerif, 60,FontStyle.Underline), Brushes.White, 200, 200);
            Buffer.Render();
        }

        public static void Update()
        {

            for (int i = _asteroids.Count - 1; i >= 0; i--)
            {
                if (_asteroids[i].Collision(_ship))
                {
                    logger.Invoke($"Столкновение корабля с астероидом {i}. X: {_asteroids[i].Rect.X}, Y: {_asteroids[i].Rect.Y}");
                    System.Media.SystemSounds.Asterisk.Play();
                    _ship.EnergyLow(_asteroids[i].Rect.Width);
                    _asteroids.RemoveAt(i);
                    _score ++;
                    currentOfAsteroids--;
                    medkitFlag = true;
                    if (_ship.Energy <= 0)
                        _ship.Die();
                }
                else
                {
                    for (int j = _bullets.Count - 1; j >= 0; j--)
                    {
                        if (_asteroids[i].Collision(_bullets[j]))
                        {
                            logger.Invoke($"Пересечение астероида {i} с пулей. X: {_bullets[j].Rect.X}, Y: {_bullets[j].Rect.Y}");
                            System.Media.SystemSounds.Hand.Play();
                            _asteroids.RemoveAt(i);
                            _score ++;
                            currentOfAsteroids--;
                            medkitFlag = true;
                            _bullets.RemoveAt(j);
                            break;
                        }
                    }
                }
            }
            for (int i = _comets.Count - 1; i >= 0; i--)
            {
                if (_comets[i].Collision(_ship))
                {
                    logger.Invoke($"Столкновение корабля с кометой. X: {_ship.Rect.X}, Y: {_ship.Rect.Y}");
                    System.Media.SystemSounds.Asterisk.Play();
                    _ship.EnergyLow(_comets[i].Rect.Width/10);
                    if (_ship.Energy <= 0)
                        _ship.Die();
                }
                else
                {
                    for (int j = _bullets.Count - 1; j >= 0; j--)
                    {
                        if (_comets[i].Collision(_bullets[j]))
                        {
                            logger.Invoke($"Пересечение кометы с пулей. X: {_bullets[j].Rect.X}, Y: {_bullets[j].Rect.Y}");
                            System.Media.SystemSounds.Hand.Play();
                            _bullets.RemoveAt(j);
                            break;
                        }
                    }
                }
            }

            foreach (var star in _stars)
                star.Update();

            foreach (var comet in _comets)
                comet.Update();

            foreach (var asteroid in _asteroids)
            {
                asteroid.Update();
                logger.Invoke($"Астероид летит. X: {asteroid.Rect.X}, Y: {asteroid.Rect.Y}");
            }
            
            foreach (var bullet in _bullets)
            {
                bullet.Update();
                logger.Invoke($"Пуля летит. X: {bullet.Rect.X}, Y: {bullet.Rect.Y}");
            }
            
            for (int i = _bullets.Count - 1; i >= 0; i--)
            {
                if (_bullets[i].Rect.X > Width )
                {
                    logger.Invoke($"Вылет пули за правую границу экрана. X: {_bullets[i].Rect.X}, Y: {_bullets[i].Rect.Y}");
                    _bullets.RemoveAt(i);
                }
            }

            for (int i = _bullets.Count - 1; i >= 0; i--)
            {
                if (_bullets[i] != null && _medkit != null && _medkit.Collision(_bullets[i]))
                {
                    logger.Invoke($"Столкновение пули с аптечкой. X: {_bullets[i].Rect.X}, Y: {_bullets[i].Rect.Y}");
                    _bullets.RemoveAt(i);
                    _medkit = null;
                    medkitFlag = false;
                    _ship.EnergyUp(50);
                    System.Media.SystemSounds.Beep.Play();
                }
            }

            if (_medkit == null && _score % 2 == 0 && medkitFlag == true)
            {
                _medkit = new Medkit(new Point(800, random.Next(0, 550)), new Point(-10, 0), new Size(30, 25));
                logger.Invoke($"Создание аптечки. X: {_medkit.Rect.X}, Y: {_medkit.Rect.Y}");
            }

            if (_medkit != null)
            {
                _medkit.Update();
                if (_medkit.Rect.X < 0)
                {
                    logger.Invoke($"Вылет аптечки за левую границу экрана. X: {_medkit.Rect.X}, Y: {_medkit.Rect.Y}");
                    _medkit = null;
                    medkitFlag = false;
                }
            }

            if (_medkit != null && _ship.Collision(_medkit))
            {
                _ship.EnergyUp(50);
                logger.Invoke($"Столновение аптечки с кораблём. X: {_medkit.Rect.X}, Y: {_medkit.Rect.Y}");
                _medkit = null;
                medkitFlag = false;
                System.Media.SystemSounds.Beep.Play();
            }

            if (currentOfAsteroids == 0)
            {
                numberOfAsteroids++;
                currentOfAsteroids = numberOfAsteroids;
                CreateAsteroids(numberOfAsteroids);
            }
        }
    }
}
