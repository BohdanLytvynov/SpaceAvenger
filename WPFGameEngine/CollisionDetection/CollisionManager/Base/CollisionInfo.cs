using WPFGameEngine.WPF.GE.GameObjects;

namespace WPFGameEngine.CollisionDetection.CollisionManager.Base
{
    public class CollisionInfo
    {
        public List<IGameObject> ObjectsWithCollision { get; set; }
        public bool Resolved { get; private set; }

        public CollisionInfo()
        {
            ObjectsWithCollision = new List<IGameObject>();
            Resolved = false;
        }

        public void Add(IGameObject gameObject)
        {
            gameObject.Collider.CollisionResolved = false;
            ObjectsWithCollision.Add(gameObject);
        }

        public void Resolve()
        { 
            Resolved = true;
            foreach (IGameObject obj in ObjectsWithCollision)
            {
                obj.Collider.ResolveCollision();
            }
        }
    }
}
