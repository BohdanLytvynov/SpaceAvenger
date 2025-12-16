using System.Numerics;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.Component.RelativeTransforms;
using WPFGameEngine.WPF.GE.GameObjects.Transformable;
using WPFGameEngine.WPF.GE.GameObjects.Updatable;
using WPFGameEngine.WPF.GE.Math.Sizes;

namespace WPFGameEngine.WPF.GE.GameObjects
{
    public abstract class UpdatableBase : TransformableBase, IUpdatable
    {
        public IGameObjectViewHost GameView { get; private set; }
        public IGameTimer GameTimer { get; private set; }

        protected UpdatableBase() : base()
        {

        }

        public virtual void StartUp(IGameObjectViewHost viewHost, IGameTimer gameTimer)
        { 
            GameView = viewHost ?? throw new ArgumentNullException(nameof(viewHost));
            GameTimer = gameTimer ?? throw new ArgumentNullException(nameof(gameTimer));

            Texture = GetTexture();

            if (Texture == null) return;

            Size originalSize = new Size((float)Texture.Width, (float)Texture.Height);

            Transform.OriginalObjectSize = originalSize;

            AIModule?.Init(GameView, this);

            foreach (var child in Children)
            {
                if (child is ITransformable transformable)
                {
                    var childTransform = transformable.Transform as IRelativeTransform;
                    if (childTransform != null)
                    {
                        childTransform.OriginalParentSize = originalSize;
                    }
                }

                if (child is IUpdatable updatable)
                    updatable.StartUp(viewHost, gameTimer);
            }
        }

        public virtual void Update()
        {
            if (!Enabled) return;

            Texture = GetTexture();

            if (Texture == null) return;

            Size originalSize = new Size((float)Texture.Width, (float)Texture.Height);

            Transform.OriginalObjectSize = originalSize;

            if (Animator != null)
            {
                Animator.Update(GameTimer);
            }
            else if (Animation != null)
            {
                Animation.Update(GameTimer);
            }

            AIModule?.Process(this);

            foreach (var child in Children)
            {
                if (child is ITransformable transformable)
                {
                    var childTransform = transformable.Transform as IRelativeTransform;
                    if (childTransform != null)
                    {
                        childTransform.OriginalParentSize = originalSize;
                    }
                }

                if (child is IUpdatable updatable)
                    updatable.Update();
            }
        }
    }
}
