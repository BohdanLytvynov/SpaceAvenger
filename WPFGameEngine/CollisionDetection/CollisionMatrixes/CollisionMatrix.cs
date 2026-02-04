namespace WPFGameEngine.CollisionDetection.CollisionMatrixes
{
    [Flags]
    public enum CollisionLayer
    {
        None = 0,                   //     00000
        Player = 1 << 0,       // 1 //     00001
        Enemy = 1 << 1,        // 2 //     00010
        PlayerProjectile = 1 << 2, // 4 // 00100
        EnemyProjectile = 1 << 3,  // 8 // 01000
        Obstacle = 1 << 4      // 16 //    10000
    }

    public static class CollisionMatrix
    {
        //Collision matrix
        private static readonly Dictionary<CollisionLayer, CollisionLayer> m_matrix = new();

        public static Dictionary<CollisionLayer, CollisionLayer> Matrix { get => m_matrix; }

        static CollisionMatrix()
        {
            // Player -> 11010
            // Player collides with Enemy, Enemy Projectiles and Obstacles
            m_matrix[CollisionLayer.Player] = CollisionLayer.Enemy | CollisionLayer.EnemyProjectile | CollisionLayer.Obstacle;
            // Player projectile -> 10010
            // Player projectiles collides only with enemies and Obstacles
            m_matrix[CollisionLayer.PlayerProjectile] = CollisionLayer.Enemy | CollisionLayer.Obstacle;
            // Enemy -> 10101
            // Enemies collides with Player, Player Projectiles and Obstacles
            m_matrix[CollisionLayer.Enemy] = CollisionLayer.Player | CollisionLayer.PlayerProjectile | CollisionLayer.Obstacle;
            // Enemy Projectile -> 10001
            // Enemy Projectiles collides with Player and Obstacles
            m_matrix[CollisionLayer.EnemyProjectile] = CollisionLayer.Player | CollisionLayer.Obstacle;
            // Obstacle can collide with Enemy, Player, Enemy Projectiles, Player Projectiles
        }
        /// <summary>
        /// Determine can 2 Collision Layers collide
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool CanCollide(CollisionLayer a, CollisionLayer b)
        {
            if (m_matrix.TryGetValue(a, out var mask))
            {
                return (mask & b) != 0;
            }
            return false;
            //Example of collision layers that collide:
            //Player and Enemy Projectile -> 11010 & 00100 = 11010, that is not 00000
            //Example of collision layers that don't collide:
            //Player and Player Projectile -> 11010 & 00100 = 00000, no collision is possible
            //Enemy Projectile and Player -> 10001 & 00001 = 00001, that is not 00000
        }
    }
}
