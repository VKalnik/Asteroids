using Asteroids.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids
{
    class Medkit : BaseObject
    {
        public Medkit(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
        }

        public override void Draw()
        {

            Game.Buffer.Graphics.DrawImage(Resources.medkit, Pos.X, Pos.Y, Size.Width, Size.Height);
        }

        public override void Update()
        {
            Pos.X = Pos.X + Dir.X;
        }
    }
}
