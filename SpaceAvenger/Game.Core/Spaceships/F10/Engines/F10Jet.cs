using System.Windows.Media.Imaging;
using WPFGameEngine.WPF.GE.Animations;
using WPFGameEngine.WPF.GE.Component.Animators;
using WPFGameEngine.WPF.GE.Component.Sprites;
using WPFGameEngine.WPF.GE.GameObjects;
using WPFGameEngine.WPF.GE.Helpers;

namespace SpaceAvenger.Game.Core.Spaceships.F10.Engines
{
    public class F10Jet : GameObject
    {
        public F10Jet(string name) : base(name)
        {
            Sprite sprite = new Sprite();
            sprite.Load(App.Current.Resources, "F10_Jet_Normal");
            Animation JetNormal = new Animation((BitmapSource)sprite.Image, 6, 6, true, 1, 
                AnimationHelpers.BuildAnimationFrames_Seconds(36, 1.5));
            var animator = new Animator();
            animator.AddAnimation("Normal", JetNormal);
            RegisterComponent(animator)
            .RegisterComponent(sprite);
        }
    }
}
