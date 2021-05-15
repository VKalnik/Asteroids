using Asteroids.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids
{
    class Comet : BaseObject
    {
        public Comet(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
        }
        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(Resources.comet1, Pos.X, Pos.Y, Size.Width, Size.Height);
        }

        public override void Update()
        {
            Pos.X = Pos.X + Dir.X;

            if (Pos.X < 0)
            {
                Pos.X = 800 + Dir.X;
                var y = random.Next(10, 520);
                Pos.Y = y;
            }
        }
    }
}
