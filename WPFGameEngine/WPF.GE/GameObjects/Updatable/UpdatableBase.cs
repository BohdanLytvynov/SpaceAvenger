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

            //Calculate actual size of the Image
            float actualWidth = (float)Texture.Width * Transform.Scale.Width;
            float actualHeight = (float)Texture.Height * Transform.Scale.Height;

            //Negative Value Protection
            actualWidth = actualWidth < 0 ? 0 : actualWidth;
            actualHeight = actualHeight < 0 ? 0 : actualHeight;

            Transform.ActualSize = new Size(actualWidth, actualHeight);

            foreach (var child in Children)
            {
                if (child is ITransformable transformable)
                {
                    var childTransform = transformable.Transform as IRelativeTransform;
                    if (childTransform != null)
                    {
                        childTransform.ActualParentSize = new Size(actualWidth, actualHeight);
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

            //Calculate actual size of the Image
            float actualWidth = (float)Texture.Width * Transform.Scale.Width;
            float actualHeight = (float)Texture.Height * Transform.Scale.Height;

            //Negative Value Protection
            actualWidth = actualWidth < 0 ? 0 : actualWidth;
            actualHeight = actualHeight < 0 ? 0 : actualHeight;

            Transform.ActualSize = new Size(actualWidth, actualHeight);
            //Calculate the center of the Image
            float Xcenter = actualWidth * Transform.CenterPosition.X;
            float Ycenter = actualHeight * Transform.CenterPosition.Y;

            Transform.ActualCenterPosition = new Vector2(Xcenter, Ycenter);
            //Get matrix for current game object

            if (Animator != null)
            {
                Animator.Update(GameTimer);
            }
            else if (Animation != null)
            {
                Animation.Update(GameTimer);
            }

            foreach (var child in Children)
            {
                if (child is ITransformable transformable)
                {
                    var childTransform = transformable.Transform as IRelativeTransform;
                    if (childTransform != null)
                    {
                        childTransform.ActualParentSize = new Size(actualWidth, actualHeight);
                    }
                }

                if (child is IUpdatable updatable)
                    updatable.Update();
            }
        }
    }
}
