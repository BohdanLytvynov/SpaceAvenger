using WPFGameEngine.WPF.GE.AnimationFrames;

namespace WPFGameEngine.WPF.GE.Helpers
{
    public static class AnimationHelpers
    {
        private const double SECONDS_TO_MILISECONDS = 1000;

        public static List<IAnimationFrame> BuildAnimationFrames(uint anim_frames_count,
            double lifeSpan_Miliseconds,
            Action<IAnimationFrame, int> configureFrame = null)
        {
            List<IAnimationFrame> frames = new List<IAnimationFrame>();

            for (int i = 0; i < anim_frames_count; i++)
            {
                frames.Add(new AnimationFrame(lifeSpan_Miliseconds));

                configureFrame?.Invoke(frames[i], i);
            }

            return frames;
        }

        public static List<IAnimationFrame> BuildAnimationFrames_Seconds(uint anim_frames_count,
            double lifeSpan_Seconds,
            Action<IAnimationFrame, int> configureFrame = null)
        {
            List<IAnimationFrame> frames = new List<IAnimationFrame>();

            for (int i = 0; i < anim_frames_count; i++)
            {
                frames.Add(new AnimationFrame(lifeSpan_Seconds));

                configureFrame?.Invoke(frames[i], i);
            }

            return frames;
        }
    }
}
