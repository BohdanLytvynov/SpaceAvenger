using WPFGameEngine.GameViewControl;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.Component.Animators;
using WPFGameEngine.WPF.GE.GameObjects;

namespace SpaceAvenger.Game.Core.Base
{
    public enum EngineState
    { 
        Starting = 0,
        Moving,
        Stopping,
        Idle
    }

    public abstract class JetBase : MapableObject
    {
        public Animator? EngineAnimator { get; private set; }
        public EngineState EngineState { get; protected set; }
        public virtual string StartEngineAnimationName { get; protected set; } = "Jet_Start";
        public virtual string StopEngineAnimationName { get; protected set; } = "Jet_Stop";
        public virtual string MovingEngineAnimationName { get; protected set; } = "Jet_Move";
        public virtual string IdleEngineAnimationName { get; protected set; } = "Jet_Idle";

        public override void StartUp(IGameObjectViewHost viewHost, IGameTimer gameTimer)
        {
            EngineAnimator = GetComponent<Animator>();
            EngineState = EngineState.Idle;
            base.StartUp(viewHost, gameTimer);
        }

        public override void Update()
        {
            switch (EngineState)
            {
                case EngineState.Starting:

                    if (EngineAnimator!.Current.IsCompleted)
                    {
                        EngineAnimator.SetAnimationForPlay(MovingEngineAnimationName, true);
                        EngineState = EngineState.Moving;
                    }

                    break;

                case EngineState.Moving:
                    //Infinity State
                    break;

                case EngineState.Stopping:

                    if (EngineAnimator!.Current.IsCompleted)
                    { 
                        EngineState = EngineState.Idle;
                    }
                    break;

                case EngineState.Idle:
                    EngineAnimator!.SetAnimationForPlay(IdleEngineAnimationName, true);
                    break;
            }

            base.Update();
        }

        public virtual void Start()
        {
            EngineAnimator!.SetAnimationForPlay(StartEngineAnimationName, true);
            EngineState = EngineState.Starting;
        }

        public virtual void Stop()
        {
            EngineAnimator!.SetAnimationForPlay(StopEngineAnimationName, true);
            EngineState = EngineState.Stopping;
        }


    }
}
