using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Asteroids
{
    class GameObjectException : Exception
    {
        public GameObjectException(string message) : base(message)
        {
            Debug.WriteLine(message);
        }
    }
}
