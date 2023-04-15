using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ants
{
    public class Oras
    {
        public int x = 0;
        public int y = 0;

        public Oras(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int getX()
        {
            return x;
        }

        public int getY()
        {
            return y;
        }

        public void setX(int X)
        {
            x = X;
        }

        public void setY(int Y)
        {
            y = Y;
        }
    }
}
