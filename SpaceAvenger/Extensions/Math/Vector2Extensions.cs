using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SpaceAvenger.Extensions.Math
{
    public static class Vector2Extensions
    {
        public static Vector2 Multiply(this Vector2 v, float number)
        {
            return new Vector2(v.X * number, v.Y * number);
        }
    }
}
