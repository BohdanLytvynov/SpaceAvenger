using System.Numerics;
using WPFGameEngine.WPF.GE.Geometry.Base;
using WPFGameEngine.WPF.GE.Geometry.Realizations;
using SMath = System.Math;

namespace WPFGameEngine.WPF.GE.Helpers
{
    public static class RayCastHelper
    {
        /// <summary>
        /// Special struct that has the Hit info
        /// </summary>
        public struct RaycastHit
        {
            /// <summary>
            /// Is Collision happened?
            /// </summary>
            public bool IsHit { get; set; }
            /// <summary>
            /// Point of Collision
            /// </summary>
            public Vector2 Point { get; set; }
            /// <summary>
            /// Surface Normal
            /// </summary>
            public Vector2 Normal { get; set; }
            /// <summary>
            /// Parameter t [0;1]
            /// </summary>
            public float T { get; set; }

            /// <summary>
            /// Main ctor
            /// </summary>
            /// <param name="isHit"></param>
            /// <param name="point"></param>
            /// <param name="normal"></param>
            /// <param name="t"></param>
            public RaycastHit(bool isHit, Vector2 point, Vector2 normal, float t)
            {
                IsHit = isHit;
                Point = point;
                Normal = normal;
                T = t;
            }
        }

        public static class RaycastHelper
        {
            /// <summary>
            /// Calculate determinant, of the matrix, that is built from the 2 Vectors a and b
            /// </summary>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <returns></returns>
            private static float Det(Vector2 a, Vector2 b) => a.X * b.Y - a.Y * b.X;
            /// <summary>
            /// Intersects Ray with the Circle
            /// </summary>
            /// <param name="start">Start point of the projectile in a frame</param>
            /// <param name="end">End point of the projectile in a frame</param>
            /// <param name="center">Center of the Circle</param>
            /// <param name="radius">Radius of the Circle</param>
            /// <returns></returns>
            public static RaycastHit RayIntersectsCircle(Vector2 start, Vector2 end, Vector2 center, float radius)
            {
                Vector2 v = end - start;//Vector from start to end (disp vector) in a current frame (Ray)
                Vector2 u = center - start;//Vector from center of the Circle to the start point
                float vLenSq = v.LengthSquared();//squared length of v

                if (vLenSq < GEConstants.Epsilon) return default;

                float t = Vector2.Dot(u, v) / vLenSq;//t = (u * v)/|v|^2, numeric value of prj of u on v
                t = SMath.Clamp(t, 0f, 1f); //clamped value[0 ; 1]

                Vector2 closestPoint = start + v * t;//Closest point to the Circle on a v 
                float distSq = Vector2.DistanceSquared(closestPoint, center);//Dist from the center to the closest point

                if (distSq <= radius * radius)//Ray intersects the Circle
                {
                    Vector2 hitPoint = closestPoint;
                    return new RaycastHit
                    (
                        true,
                        hitPoint,
                        Vector2.Normalize(hitPoint - center),
                        t
                    );
                }

                return new RaycastHit(false, Vector2.Zero, Vector2.Zero, 0f);
            }
            /// <summary>
            /// Intersection of the ray and the shape
            /// </summary>
            /// <param name="rayStart"></param>
            /// <param name="rayEnd"></param>
            /// <param name="shape"></param>
            /// <returns></returns>
            /// <exception cref="InvalidOperationException"></exception>
            public static RaycastHit RayIntersectsShape(Vector2 rayStart, Vector2 rayEnd, 
                IShape2D shape)
            {
                if (shape is Circle circle)
                {
                    return RayIntersectsCircle(rayStart, rayEnd, circle.CenterPosition, circle.Radius);
                }
                else
                {
                    return RayIntersectsPolygon(rayStart, rayEnd, shape);
                }

                throw new InvalidOperationException("Unsupported shape for raycast!");
            }

            /// <summary>
            /// Intersection of the Ray and Segment
            /// </summary>
            /// <param name="start">Start point of the projectile in a frame</param>
            /// <param name="end">End point of the projectile in a frame</param>
            /// <param name="v1">Start point of the edge</param>
            /// <param name="v2">End point of the edge</param>
            /// <param name="hit">Information about the hit</param>
            /// <returns>Intersection happened</returns>
            public static bool IntersectSegments(Vector2 start, Vector2 end, Vector2 v1, Vector2 v2, out RaycastHit hit)
            {
                hit = default;
                Vector2 r = end - start;//Vector from start to end (disp vector) in a current frame (Ray)
                Vector2 s = v2 - v1;//Vector from start point of the edge to the end point of the same edge
                float det = Det(r, s);//Determinant calculations. If det == 0 -> r || s, no solutions for the system

                if (SMath.Abs(det) < GEConstants.Epsilon) return false; // r || s

                float t = Det(v1 - start, s) / det;//normalized point on a Ray
                float u = Det(v1 - start, r) / det;//normalized point on the Edge

                if (t >= 0 && t <= 1 && u >= 0 && u <= 1)
                {
                    Vector2 p = start + t * r;//Intersection point
                    Vector2 normal = Vector2.Normalize(new Vector2(-s.Y, s.X));//normal to the edge
                    hit = new RaycastHit(true, p, normal, t);
                    return true;
                }

                return false;
            }

            /// <summary>
            /// Ray intersects with a Polygon
            /// </summary>
            /// <param name="start">Start point of the projectile in a frame</param>
            /// <param name="end">End point of the projectile in a frame</param>
            /// <param name="polygon">Collider Shape</param>
            /// <returns></returns>
            public static RaycastHit RayIntersectsPolygon(Vector2 start, Vector2 end, IShape2D polygon)
            {
                RaycastHit closestHit = new RaycastHit { T = float.MaxValue };
                closestHit.T = float.MaxValue;
                var vertices = polygon.GetVertexes();//Get all polygon verteces
                int count = vertices.Count;
                //We iterate here all edges point by point, building vectors from them
                for (int i = 0; i < count; i++)
                {
                    Vector2 v1 = vertices[i];
                    Vector2 v2 = vertices[(i + 1) % count];//loop closure
                    //Check intersection between ray and edge
                    if (IntersectSegments(start, end, v1, v2, out RaycastHit currentHit))
                    {
                        // we need the first intersection of the polygon with the ray
                        if (currentHit.T < closestHit.T)
                        {
                            closestHit = currentHit;
                        }
                    }
                }

                return closestHit.IsHit ? closestHit : default;
            }
        }
    }
}
