using Asteroids.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids
{
    class Star : BaseObject
    {
        public Star(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
        }

        public override void Draw()
        {
            switch (index)
            {
                case 1:
                    Game.Buffer.Graphics.DrawImage(Resources.star1, Pos.X, Pos.Y, Size.Width, Size.Height);
                    break;
                default:
                    Game.Buffer.Graphics.DrawImage(Resources.star2, Pos.X, Pos.Y, Size.Width, Size.Height);
                    break;
            }
        }

        public override void Update()
        {
            Pos.X = Pos.X + Dir.X;
          
            if (Pos.X < 0)
            {
                Pos.X = 800 + Dir.X;
                var y = random.Next(10, 580);
                Pos.Y = y;
            }
        }
    }
}
