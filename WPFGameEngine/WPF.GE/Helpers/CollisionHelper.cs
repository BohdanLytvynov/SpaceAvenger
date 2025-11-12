using WPFGameEngine.WPF.GE.Geometry.Base;
using WPFGameEngine.WPF.GE.Geometry.Realizations;
using SMath = System.Math;

namespace WPFGameEngine.WPF.GE.Helpers
{
    public static class CollisionHelper
    {
        public static bool Intersects(IShape2D a, IShape2D b)
        {
            // 1. AABB Check (Fast Reject)
            if (!IntersectsAABB(a.GetBounds(), b.GetBounds()))
            {
                return false;
            }
            
            if (a is Circle circleA && b is Circle circleB)
                return Intersects(circleA, circleB);

            if (a is Rectangle rectA && b is Rectangle rectB)
                return Intersects(rectA, rectB);
            
            if (a is Circle circle && b is Rectangle rect)
                return Intersects(circle, rect);

            if (a is Rectangle rectOther && b is Circle circleOther)
                return Intersects(circleOther, rectOther);

            // Add logic Triangle-X

            throw new NotImplementedException("Type is not implemented!");
        }
        
        private static bool IntersectsAABB(Rectangle a, Rectangle b)
        {
            return a.LeftUpperCorner.X < b.LeftUpperCorner.X + b.Width &&
                   a.LeftUpperCorner.X + a.Width > b.LeftUpperCorner.X &&
                   a.LeftUpperCorner.Y < b.LeftUpperCorner.Y + b.Height &&
                   a.LeftUpperCorner.Y + a.Height > b.LeftUpperCorner.Y;
        }

        public static bool Intersects(Circle a, Circle b)
        {
            float dx = a.Center.X - b.Center.X;
            float dy = a.Center.Y - b.Center.Y;
            float distanceSquared = dx * dx + dy * dy;
            float radiiSum = a.Radius + b.Radius;

            return distanceSquared <= radiiSum * radiiSum;
        }

        /// <summary>
        /// AABB Check for 2 Rectangles
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Intersects(Rectangle a, Rectangle b)
        {
            return IntersectsAABB(a, b); // AABB-тест и есть коллизия двух AABB
        }

        public static bool Intersects(Circle circle, Rectangle rect)
        {
            // 1. Find Point that is the closest one to the center of the Circle
            float closestX = SMath.Clamp(circle.Center.X, rect.LeftUpperCorner.X, rect.LeftUpperCorner.X + rect.Width);
            float closestY = SMath.Clamp(circle.Center.Y, rect.LeftUpperCorner.Y, rect.LeftUpperCorner.Y + rect.Height);

            // 2. Calculate distance between this nearest point and the Center of the Circle
            float dx = circle.Center.X - closestX;
            float dy = circle.Center.Y - closestY;
            float distanceSquared = dx * dx + dy * dy;

            // 3. Collision condition
            return distanceSquared < circle.Radius * circle.Radius;
        }

        
        public static bool Intersects(Triangle triangle, Circle circle)
        {
            //To do
            return false;
        }
    }
}
