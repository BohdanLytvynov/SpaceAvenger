using System.Numerics;
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
            //if (!IntersectsAABB(a.GetBounds(), b.GetBounds()))
            //{
            //    return false;
            //}
            //Do more precision check
            if (a is Circle circleA && b is Circle circleB)
                return Intersects(circleA, circleB);
            if (a is Circle && b is Rectangle)
                return Intersects((Circle)a, (Rectangle)b);
            if (a is Rectangle && b is Circle)
                return Intersects((Circle)b, (Rectangle)a);
            if (a is Circle && b is Triangle)
                return Intersects((Circle)a, (Triangle)b);
            if (a is Triangle && b is Circle)
                return Intersects((Circle)b, (Triangle)a);

            return SATIntersects(a, b);
        }

        /// <summary>
        /// Checks collision of 2 Rectangles
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static bool IntersectsAABB(Rectangle a, Rectangle b)
        {
            return a.LeftUpperCorner.X < b.LeftUpperCorner.X + b.Size.Width &&
                   a.LeftUpperCorner.X + a.Size.Width > b.LeftUpperCorner.X &&
                   a.LeftUpperCorner.Y < b.LeftUpperCorner.Y + b.Size.Height &&
                   a.LeftUpperCorner.Y + a.Size.Height > b.LeftUpperCorner.Y;
        }

        /// <summary>
        /// Checks collision between 2 Circles
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Intersects(Circle a, Circle b)
        {
            float dx = a.CenterPosition.X - b.CenterPosition.X;
            float dy = a.CenterPosition.Y - b.CenterPosition.Y;
            float distanceSquared = dx * dx + dy * dy;
            float radiiSum = a.Radius + b.Radius;
            return distanceSquared <= radiiSum * radiiSum;
        }

        /// <summary>
        /// Checks the intersection between Circle and Rectangle
        /// </summary>
        /// <param name="circle"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static bool Intersects(Circle circle, Rectangle rect)
        {
            var closestWorld = GetClosestPointOfRectangle(rect, circle.CenterPosition);
            Vector2 disp = circle.CenterPosition - closestWorld;
            float dd = disp.LengthSquared();
            float rr = circle.Radius * circle.Radius;
            return dd <= rr;
        }

        /// <summary>
        /// Checks the intersection between Circle and Triangle
        /// </summary>
        /// <param name="circle"></param>
        /// <param name="triangle"></param>
        /// <returns></returns>
        public static bool Intersects(Circle circle, Triangle triangle)
        {
            Vector2 closestPoint = GetClosestPointToTriangle(triangle.LeftUpperCorner, triangle.B, triangle.C, circle.CenterPosition);
            Vector2 disp = circle.CenterPosition - closestPoint;
            float dd = disp.LengthSquared();
            float rr = circle.Radius * circle.Radius;
            return dd <= rr;
        }

        /// <summary>
        /// Gets Closest Point of the Rectangle to the point
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Vector2 GetClosestPointOfRectangle(Rectangle rect, Vector2 point)
        {
            Vector2 dir = point - rect.CenterPosition;
            float localX = Vector2.Dot(dir, rect.Basis.X);
            float localY = Vector2.Dot(dir, rect.Basis.Y);

            float clampedX = SMath.Max(-rect.Size.Width / 2, SMath.Min(rect.Size.Width / 2, localX));
            float clampedY = SMath.Max(-rect.Size.Height / 2, SMath.Min(rect.Size.Height / 2, localY));

            return rect.CenterPosition +
                (rect.Basis.X * clampedX) + (rect.Basis.Y * clampedY);
        }

        /// <summary>
        /// Gets the Closest Point on the segment AB to the P point
        /// </summary>
        /// <param name="A">Start of the line Segment</param>
        /// <param name="B">End of the line Segment</param>
        /// <param name="P">Point</param>
        /// <returns></returns>
        public static Vector2 ClosestPointOnSegment(Vector2 A, Vector2 B, Vector2 P)
        {
            Vector2 AB = B - A;
            Vector2 AP = P - A;

            float lengthSq = AB.LengthSquared();
            if (lengthSq == 0.0f)
            {
                return A;
            }

            float t = Vector2.Dot(AP, AB) / lengthSq;

            if (t < 0.0f)
                return A;

            if (t > 1.0f)
                return B;

            return A + AB * t;
        }

        /// <summary>
        /// Gets the closest point on the Triangle
        /// </summary>
        /// <param name="A">Vertex 1.</param>
        /// <param name="B">Vertex 2.</param>
        /// <param name="C">Vertex 3.</param>
        /// <param name="P">External Point</param>
        /// <returns></returns>
        public static Vector2 GetClosestPointToTriangle(Vector2 A, Vector2 B, Vector2 C, Vector2 P)
        {
            Vector2 closestAB = ClosestPointOnSegment(A, B, P);
            Vector2 closestBC = ClosestPointOnSegment(B, C, P);
            Vector2 closestCA = ClosestPointOnSegment(C, A, P);

            float distSqAB = (closestAB - P).LengthSquared();
            float distSqBC = (closestBC - P).LengthSquared();
            float distSqCA = (closestCA - P).LengthSquared();

            float minDistSq = distSqAB;
            Vector2 closestPoint = closestAB;

            if (distSqBC < minDistSq)
            {
                minDistSq = distSqBC;
                closestPoint = closestBC;
            }

            if (distSqCA < minDistSq)
            {
                closestPoint = closestCA;
            }

            return closestPoint;
        }

        /// <summary>
        /// Separate Axis Theorem
        /// </summary>
        /// <param name="shape1"></param>
        /// <param name="shape2"></param>
        /// <returns></returns>
        public static bool SATIntersects(IShape2D shape1, IShape2D shape2)
        {
            var normalsA = shape1.GetNormals();
            var normalsB = shape2.GetNormals();
            var normalsAll = normalsA.Concat(normalsB).Distinct().ToList();

            foreach (var normal in normalsAll)
            {
                (float minA, float maxA) = Project(shape1, normal);
                (float minB, float maxB) = Project(shape2, normal);

                if (maxA < minB || maxB < minA)
                    return false;
            }

            return true;
        }

        public static (float min, float max) Project(IShape2D shape, Vector2 axis)
        {
            float min = float.MaxValue;
            float max = float.MinValue;
            var vertexes = shape.GetVertexes();

            foreach (var vertex in vertexes)
            {
                float projection = Vector2.Dot(vertex, axis);
                if (projection < min) min = projection;
                if (projection > max) max = projection;
            }

            return (min, max);
        }
    }
}