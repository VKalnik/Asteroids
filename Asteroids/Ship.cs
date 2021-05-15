using Asteroids.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids
{
    class Ship : BaseObject
    {
        private int energy = 100;
        public static event EventHandler DieEvent; 

        public Ship(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
        }

        public void EnergyLow(int damage)
        {
            energy -= damage;
        }

        public void EnergyUp(int heal)
        {
            energy += heal;
        }

        public int Energy
        {
            get { return energy; }
        }

        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(Resources.ship, Pos.X, Pos.Y, Size.Width, Size.Height);
        }

        public override void Update()
        {
            
        }

        public void Up()
        {
            if (Pos.Y>0)
            {
                Pos.Y = Pos.Y - Dir.Y;
            }
        }
        public void Down()
        {
            if (Pos.Y < Game.Height - 40)
            {
                Pos.Y = Pos.Y + Dir.Y;
            }
        }

        public void Die()
        {
            if (DieEvent != null)
            {
                DieEvent.Invoke(this, new EventArgs());
            }
        }
    }
}
