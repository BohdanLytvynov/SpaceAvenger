namespace WPFGameEngine.WPF.GE.GameObjects
{
    public interface ICollidable
    {
        public bool IsCollidable { get; }

        void ProcessCollision(List<IGameObject>? collisionInfo);
    }
}
