namespace WPFGameEngine.WPF.GE.AnimationFrames
{
    public class AnimationFrame : IAnimationFrame
    {
        //Current frame lifespan
        public double Lifespan { get; init; }

        public AnimationFrame(double lifespan)
        {
            Lifespan = lifespan;
        }

        //Here can be the Easing Function 

        public override string ToString()
        {
            return $"Ls: {Lifespan}";
        }
    }
}
