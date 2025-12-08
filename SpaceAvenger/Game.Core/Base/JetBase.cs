using System.Diagnostics;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.Component.Animators;
using WPFGameEngine.WPF.GE.GameObjects;

namespace SpaceAvenger.Game.Core.Base
{
    public enum EngineState
    { 
        Idle = 0,
        Starting,
        Moving,
        Stopping,
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
            EngineAnimator.SetAnimationForPlay(IdleEngineAnimationName);
            EngineAnimator.Start();
            base.StartUp(viewHost, gameTimer);
        }

        public override void Update()
        {
            switch (EngineState)
            {
                case EngineState.Idle:

                    EngineAnimator!.SetAnimationForPlay(IdleEngineAnimationName, true);

                    break;
                case EngineState.Starting:

                    if (EngineAnimator!.Current.IsCompleted 
                        && EngineAnimator!.Current_Animation_Name.Equals(StartEngineAnimationName))
                    { 
                        EngineState = EngineState.Moving;
                        EngineAnimator!.SetAnimationForPlay(MovingEngineAnimationName, true);
                    }

                    break;
                case EngineState.Moving:
                    break;
                case EngineState.Stopping:

                    if (EngineAnimator!.Current_Animation_Name.Equals(StopEngineAnimationName)
                        && EngineAnimator!.Current.IsCompleted)
                    {
                        EngineState = EngineState.Idle;
                    }

                    break;
                default:
                    break;
            }

            base.Update();
        }

        public virtual void Start()
        {
            if (EngineState == EngineState.Moving || EngineState == EngineState.Starting)
                return;

            EngineAnimator!.SetAnimationForPlay(StartEngineAnimationName, true);
            EngineState = EngineState.Starting;
            //Debug.WriteLine("In Start Method");
        }

        public virtual void Stop()
        {
            if(EngineState == EngineState.Stopping || EngineState == EngineState.Idle)
                return;

            EngineAnimator!.SetAnimationForPlay(StopEngineAnimationName, true);
            EngineState = EngineState.Stopping;
            //Debug.WriteLine("In Stop Method");
        }


    }
}
