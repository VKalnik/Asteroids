using Asteroids.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids
{
    class Asteroid : BaseObject
    {
        public Asteroid(Point pos, Point dir, Size size) : base(pos, dir, size) 
        {
        }

        public override void Draw()
        {
            switch (index)
            {
                case 1:
                    Game.Buffer.Graphics.DrawImage(Resources.meteorBrown_big1, Pos.X, Pos.Y, Size.Width, Size.Height);
                    break;
                case 2:
                    Game.Buffer.Graphics.DrawImage(Resources.meteorBrown_big2, Pos.X, Pos.Y, Size.Width, Size.Height);
                    break;
                case 3:
                    Game.Buffer.Graphics.DrawImage(Resources.meteorBrown_big3, Pos.X, Pos.Y, Size.Width, Size.Height);
                    break;
                default:
                    Game.Buffer.Graphics.DrawImage(Resources.meteorBrown_big4, Pos.X, Pos.Y, Size.Width, Size.Height);
                    break;
            }
        }

        public override void Update()
        {
            Pos.X = Pos.X + Dir.X;

            if (Pos.X < 0)
            {
                Pos.X = 800 + Dir.X;
                var y = random.Next(5, 550);
                Pos.Y = y;
            }

            Pos.Y = Pos.Y + Dir.Y;
            if (Pos.Y < 0) Dir.Y = -Dir.Y;
            if (Pos.Y > Game.Height - 25) Dir.Y = -Dir.Y;
        }
    }
}
