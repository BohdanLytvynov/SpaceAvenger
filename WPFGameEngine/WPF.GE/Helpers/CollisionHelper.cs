using System.Numerics;
using WPFGameEngine.WPF.GE.Geometry.Base;
using WPFGameEngine.WPF.GE.Geometry.Realizations;
using SMath = System.Math;

namespace WPFGameEngine.WPF.GE.Helpers
{
    public struct CollisionResult
    {
        public bool Intersects;
        public Vector2 MTV;
        public float Overlap;
    }

    public static class CollisionHelper
    {
        public static CollisionResult Intersects(IShape2D a, IShape2D b)
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
        public static CollisionResult Intersects(Circle a, Circle b)
        {
            Vector2 delta = b.CenterPosition - a.CenterPosition;
            float distanceSquared = delta.LengthSquared();
            float radiiSum = a.Radius + b.Radius;

            if (distanceSquared > radiiSum * radiiSum)
            {
                return new CollisionResult { Intersects = false };
            }

            float distance = MathF.Sqrt(distanceSquared);

            float overlap = (distance == 0) ? a.Radius + b.Radius : radiiSum - distance;

            Vector2 normal = (distance == 0) ? new Vector2(0, 1) : Vector2.Normalize(delta);

            return new CollisionResult
            {
                Intersects = true,
                Overlap = overlap,
                MTV = normal * overlap
            };
        }

        /// <summary>
        /// Checks the intersection between Circle and Rectangle
        /// </summary>
        /// <param name="circle"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static CollisionResult Intersects(Circle circle, Rectangle rect)
        {
            // 1. Находим ближайшую точку на прямоугольнике к центру круга
            // Используем Clamp для ограничения координат центра круга границами прямоугольника
            float closestX = SMath.Clamp(circle.CenterPosition.X, rect.LeftUpperCorner.X, rect.RightLowerCorner.X);
            float closestY = SMath.Clamp(circle.CenterPosition.Y, rect.LeftUpperCorner.Y, rect.RightLowerCorner.Y);

            Vector2 closestPoint = new Vector2(closestX, closestY);
            Vector2 direction = circle.CenterPosition - closestPoint;
            float distanceSquared = direction.LengthSquared();
            float radiusSquared = circle.Radius * circle.Radius;

            // Проверка: есть ли столкновение вообще?
            if (distanceSquared > radiusSquared && !IsPointInside(circle.CenterPosition, rect))
            {
                return new CollisionResult { Intersects = false };
            }

            float distance = MathF.Sqrt(distanceSquared);
            Vector2 normal;
            float overlap;

            // СЛУЧАЙ А: Центр круга ВНУТРИ прямоугольника
            if (distance < float.Epsilon)
            {
                // Ищем кратчайшее расстояние до одной из 4-х сторон
                float distLeft = circle.CenterPosition.X - rect.LeftUpperCorner.X;
                float distRight = rect.RightLowerCorner.X - circle.CenterPosition.X;
                float distTop = circle.CenterPosition.Y - rect.LeftUpperCorner.Y;
                float distBottom = rect.RightLowerCorner.Y - circle.CenterPosition.Y;

                float minOverlap = MathF.Min(MathF.Min(distLeft, distRight), MathF.Min(distTop, distBottom));

                if (minOverlap == distLeft) normal = new Vector2(-1, 0);
                else if (minOverlap == distRight) normal = new Vector2(1, 0);
                else if (minOverlap == distTop) normal = new Vector2(0, -1);
                else normal = new Vector2(0, 1);

                overlap = minOverlap + circle.Radius;
            }
            // СЛУЧАЙ Б: Центр круга СНАРУЖИ, но он касается или пересекает грань
            else
            {
                normal = Vector2.Normalize(direction);
                overlap = circle.Radius - distance;
            }

            return new CollisionResult
            {
                Intersects = true,
                Overlap = overlap,
                MTV = normal * overlap
            };
        }

        // Вспомогательный метод для проверки вхождения точки в Rect
        private static bool IsPointInside(Vector2 point, Rectangle rect)
        {
            return point.X >= rect.LeftUpperCorner.X && point.X <= rect.RightLowerCorner.X &&
                   point.Y >= rect.LeftUpperCorner.Y && point.Y <= rect.RightLowerCorner.Y;
        }

        private static (Vector2 normal, float overlap) GetInternalTriangleExit(Circle circle, Triangle tri)
        {
            Vector2[] vertices = { tri.LeftUpperCorner, tri.B, tri.C };
            float minDistance = float.MaxValue;
            Vector2 bestNormal = Vector2.Zero;

            for (int i = 0; i < 3; i++)
            {
                Vector2 v1 = vertices[i];
                Vector2 v2 = vertices[(i + 1) % 3];

                // Вектор стороны и нормаль к ней
                Vector2 edge = v2 - v1;
                // Получаем перпендикуляр (нормаль), направленный наружу
                Vector2 edgeNormal = Vector2.Normalize(new Vector2(-edge.Y, edge.X));

                // Проверяем, в ту ли сторону смотрит нормаль (должна от центра треугольника)
                Vector2 centerTri = (tri.LeftUpperCorner + tri.B + tri.C) / 3f;
                if (Vector2.Dot(edgeNormal, v1 - centerTri) < 0) edgeNormal = -edgeNormal;

                // Расстояние от центра круга до прямой (проекция на нормаль)
                float dist = SMath.Abs(Vector2.Dot(circle.CenterPosition - v1, edgeNormal));

                if (dist < minDistance)
                {
                    minDistance = dist;
                    bestNormal = edgeNormal;
                }
            }

            // Итоговый overlap: расстояние до края + радиус
            return (bestNormal, minDistance + circle.Radius);
        }

        /// <summary>
        /// Checks the intersection between Circle and Triangle
        /// </summary>
        /// <param name="circle"></param>
        /// <param name="triangle"></param>
        /// <returns></returns>
        public static CollisionResult Intersects(Circle circle, Triangle triangle)
        {
            // Используем ваши вершины (предположим, это LeftUpperCorner, B и C)
            Vector2 closestPoint = GetClosestPointToTriangle(
                triangle.LeftUpperCorner,
                triangle.B,
                triangle.C,
                circle.CenterPosition
            );

            Vector2 direction = circle.CenterPosition - closestPoint;
            float distanceSquared = direction.LengthSquared();
            float radiusSquared = circle.Radius * circle.Radius;

            // 1. Ранний выход, если столкновения нет
            if (distanceSquared > radiusSquared)
            {
                return new CollisionResult { Intersects = false };
            }

            float distance = MathF.Sqrt(distanceSquared);
            Vector2 normal;
            float overlap;

            // 2. Специфический случай: центр круга ВНУТРИ треугольника
            // Если ближайшая точка совпадает с центром, расстояние равно 0
            if (distance < float.Epsilon)
            {
                // Находим кратчайшее расстояние до сторон треугольника для выталкивания
                (normal, overlap) = GetInternalTriangleExit(circle, triangle);
            }
            else
            {
                // Стандартный случай: выталкиваем от ближайшей точки к центру
                normal = direction / distance;
                overlap = circle.Radius - distance;
            }

            return new CollisionResult
            {
                Intersects = true,
                Overlap = overlap,
                MTV = normal * overlap
            };
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
        public static CollisionResult SATIntersects(IShape2D shape1, IShape2D shape2)
        {
            float minOverlap = float.MaxValue;
            Vector2 smallestAxis = Vector2.Zero;

            var normalsA = shape1.GetNormals();
            var normalsB = shape2.GetNormals();
            var normalsAll = normalsA.Concat(normalsB).Distinct();

            foreach (var normal in normalsAll)
            {
                (float minA, float maxA) = Project(shape1, normal);
                (float minB, float maxB) = Project(shape2, normal);

                if (maxA < minB || maxB < minA)
                {
                    return new CollisionResult { Intersects = false };
                }

                float overlap = SMath.Min(maxA, maxB) - SMath.Max(minA, minB);

                if (overlap < minOverlap)
                {
                    minOverlap = overlap;
                    smallestAxis = normal;
                }
            }

            Vector2 center1 = shape1.CenterPosition;
            Vector2 center2 = shape2.CenterPosition;
            if (Vector2.Dot(center2 - center1, smallestAxis) < 0)
            {
                smallestAxis = -smallestAxis;
            }

            return new CollisionResult
            {
                Intersects = true,
                Overlap = minOverlap,
                MTV = smallestAxis * minOverlap
            };
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