using Asteroids.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


namespace Asteroids
{
    abstract class BaseObject : ICollision
    {
        protected Point Pos;
        protected Point Dir;
        protected Size Size;
        protected int index;
        protected static Random random = new Random();

        public Rectangle Rect => new Rectangle(Pos, Size);

        public bool Collision(ICollision obj)
        {
            return obj.Rect.IntersectsWith(Rect);
        }

        public BaseObject(Point pos, Point dir, Size size)
        {
            try
            {
                Pos = pos;

                if (Pos.X < 0 || Pos.X > 800 || Pos.Y < 0 || Pos.Y > 800)
                {
                    throw new GameObjectException("Объект создан вне экрана игры");
                }
            }
            catch (GameObjectException ex)
            {
                Debug.WriteLine(ex);
            }

            Dir = dir;
            Size = size;
            index = random.Next(1, 4);
        }

        public abstract void Draw();

        public virtual void Update()
        {
            //Pos.X = Pos.X + Dir.X;
            //Pos.Y = Pos.Y + Dir.Y;

            //if (Pos.X < 0) Dir.X = -Dir.X;
            //if (Pos.X > Game.Width) Dir.X = -Dir.X;

            //if (Pos.Y < 0) Dir.Y = -Dir.Y;
            //if (Pos.Y > Game.Height) Dir.Y = -Dir.Y;


            //Pos.X = Pos.X + Dir.X;
            //if (Pos.X < 0)
            //{
            //    Pos.X = 800 + Dir.X;
            //    Random random = new Random();
            //    var y = random.Next(5, 550);
            //    Pos.Y = y;
            //}
        }   
    }
}
