using SpaceAvenger.Game.Core.Base;
using SpaceAvenger.Game.Core.Base.Interfaces;
using System;
using System.Numerics;
using System.Windows;
using WPFGameEngine.Extensions;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.WPF.GE.AI.Base;
using WPFGameEngine.WPF.GE.Component.Transforms;
using WPFGameEngine.WPF.GE.GameObjects;
using WPFGameEngine.WPF.GE.GameObjects.Transformable;
using WPFGameEngine.WPF.GE.Math.Basis;

namespace SpaceAvenger.Game.Core.AI
{
    /// <summary>
    /// Controls the move direction of the Ship
    /// </summary>
    internal enum MoveDir
    { 
        Down = 0,
        Up,
    }

    public class SpaceShipControlModule : AIModuleBase
    {
        //Current move direction
        private MoveDir m_moveDir;
        //Current window
        private Window m_mainWindow;
        //lower horizontal scene border
        private float m_MinY;
        //upper horizontal scene border
        private float m_MaxY;
        //left scene border
        private float m_MinX;
        //rigth scene border
        private float m_MaxX;
        private float m_Offset;
        //current X position 
        private float m_currX;

        //Randomizer
        private Random m_Random;
        /// <summary>
        /// Delegate for time calculation required to fly to another side of the window
        /// </summary>
        public Func<float, float> GetTime { get; set; }
        /// <summary>
        /// The function for moving left, must be Set during the SpaceShip Configuration
        /// </summary>
        public Func<Basis2D, float, Vector2> MoveLeft { get; set; }
        /// <summary>
        /// The function for moving right
        /// </summary>
        public Func<Basis2D, float, Vector2> MoveRight { get; set; }
        /// <summary>
        /// The function for moving forward
        /// </summary>
        public Func<Basis2D, float, Vector2> MoveForward { get; set; }
        /// <summary>
        /// The function for moving backward
        /// </summary>
        public Func<Basis2D, float, Vector2> MoveBackward { get; set; }

        public SpaceShipControlModule()
        {
            m_Random = new Random();
            m_currX = 0f;
            m_Offset = 40f;
        }
        /// <summary>
        /// Method for Initializing the AI module
        /// </summary>
        /// <param name="gameView">The World</param>
        /// <param name="gameObject">Current GameObject</param>
        public override void Init(IGameObjectViewHost gameView, IGameObject gameObject)
        {
            base.Init(gameView, gameObject);

            ITransformable t = gameObject as ITransformable;
            if (t == null) return;

            m_mainWindow = App.Current.MainWindow;
            var worldMatrix = t.GetWorldTransformMatrix();
            WPFGameEngine.WPF.GE.Math.Sizes.Size wScale = t.GetWorldScale(worldMatrix);

            m_MinY = 0f - (wScale.Height + m_Offset);
            m_MaxY = (float)m_mainWindow.ActualHeight + m_Offset;
            m_MinX = 0f;
            m_MaxX = (float)m_mainWindow.ActualWidth - wScale.Width;
            m_currX = (float)m_Random.NextDouble() * m_MaxX;
            m_moveDir = MoveDir.Down;

            t.Translate(new Vector2(m_currX, m_MinY));

            var angle = worldMatrix.GetBasis().X.GetAngleDeg(new Vector2(0f, 1f));

            if (angle != 0)
            { 
                t.Rotate(angle);
            }
        }

        public override void Process(IGameObject gameObject)
        {
            base.Process(gameObject);

            SpaceShipBase spaceShip = gameObject as SpaceShipBase;
            if (spaceShip == null) return;
            if(!spaceShip.IsAlive) return;

            float delta = (float)GameView.GameTimer.deltaTime.TotalSeconds;
            var worldMatrix = spaceShip.GetWorldTransformMatrix();
            var basis = worldMatrix.GetBasis();
            var worldScale = spaceShip.GetWorldScale(worldMatrix);
            Vector2 translate = Vector2.Zero;
            Vector2 currTranslate = spaceShip.Transform.Position;

            switch (m_moveDir)
            {
                case MoveDir.Down:

                    if (currTranslate.Y < m_MaxY)
                    {
                        translate += MoveForward(basis, delta);
                    }
                    else
                    {
                        //Calculate new X pos

                        //First half of the Window
                        if (currTranslate.X >= m_MinX &&
                            currTranslate.X < m_mainWindow.ActualWidth / 2)
                        {
                            m_currX = ((float)(m_mainWindow.ActualWidth / 2) - worldScale.Width) + 
                                (float)(m_Random.NextDouble() * m_mainWindow.ActualWidth / 2);
                        }
                        //Second half of the Window
                        else if (currTranslate.X >= m_mainWindow.ActualWidth / 2
                            && currTranslate.X <= m_MaxX)
                        {
                            m_currX = (float)(m_Random.NextDouble() * m_mainWindow.ActualWidth / 2);
                        }

                        spaceShip.Translate(new Vector2(m_currX, currTranslate.Y));

                        var angle = basis.X.GetAngleDeg(new Vector2(0f, -1f));

                        if (angle > 0)
                        {
                            spaceShip.Rotate(spaceShip.Transform.Rotation + (-angle));
                        }

                        m_MaxY = (float)m_mainWindow.ActualHeight
                            + m_Offset + worldScale.Height;
                        m_MinY = 0f - m_Offset;
                        m_moveDir = MoveDir.Up;
                    }

                break;

                case MoveDir.Up:

                    if (currTranslate.Y > m_MinY)
                    {
                        translate += MoveForward(basis, delta);
                    }
                    else
                    {
                        //Calculate new X pos

                        //First half of the Window
                        if (currTranslate.X >= m_MinX &&
                            currTranslate.X < m_mainWindow.ActualWidth / 2)
                        {
                            m_currX = ((float)(m_mainWindow.ActualWidth / 2) - worldScale.Width) +
                                (float)(m_Random.NextDouble() * m_mainWindow.ActualWidth / 2);
                        }
                        //Second half of the Window
                        else if (currTranslate.X >= m_mainWindow.ActualWidth / 2
                            && currTranslate.X <= m_MaxX)
                        {
                            m_currX = (float)(m_Random.NextDouble() * m_mainWindow.ActualWidth / 2);
                        }

                        spaceShip.Translate(new Vector2(m_currX, currTranslate.Y));
                        var angle = worldMatrix.GetBasis().X.GetAngleDeg(new Vector2(0f, 1f));

                        if (angle != 0)
                        {
                            spaceShip.Rotate(spaceShip.Transform.Rotation + angle);
                        }

                        m_MinY = 0f - (worldScale.Height + m_Offset);
                        m_MaxY = (float)m_mainWindow.ActualHeight + m_Offset;

                        m_moveDir = MoveDir.Down;
                    }

                break;
            }

            Vector2 newPos = spaceShip.Transform.Position + translate;

            float Xclamped = Math.Clamp(newPos.X, m_MinX, m_MaxX);
            float Yclamped = Math.Clamp(newPos.Y, m_MinY, m_MaxY);

            Vector2 finalPos = new Vector2(Xclamped, Yclamped);

            spaceShip.Translate(finalPos);

            var player = GameView.GetObject(o => o.Metadata.Contains("Player"));
            var playerTransform = player.GetComponent<TransformComponent>();
            if (player == null) return;

            var battleShip = gameObject as IBattleShip;

            if (battleShip == null) return;

            var distance = (playerTransform.Position - spaceShip.Transform.Position).LengthSquared();

            if (distance <= battleShip.DetectionDistance * battleShip.DetectionDistance)
            {
                var pl_t = (player as ITransformable);
                if (pl_t != null)
                {
                    var wm = pl_t.GetWorldTransformMatrix();
                    var center = pl_t.GetWorldCenter(wm);
                    battleShip.AimWeapons(center, true);

                    if (battleShip.WeaponsAimed)
                    {
                        battleShip.ShootWeapons(center);
                    }
                }
                
            }

            //Shoot Weapons
            if (distance <= battleShip.DetectionDistance * battleShip.DetectionDistance)
            {
                battleShip.ShootWeapons(playerTransform.Position);
            }
        }
    }
}
