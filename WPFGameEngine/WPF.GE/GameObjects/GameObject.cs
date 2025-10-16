using System.Drawing;
using System.Numerics;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.WPF.GE.Animations;
using WPFGameEngine.WPF.GE.Component.Animators;
using WPFGameEngine.WPF.GE.Component.Base;
using WPFGameEngine.WPF.GE.Component.Sprites;
using WPFGameEngine.WPF.GE.Exceptions;
using WPFGameEngine.WPF.GE.Settings;

namespace WPFGameEngine.WPF.GE.GameObjects
{
    public abstract class GameObject : IGameObject
    {
        #region Nested Classes
        public class ZIndexGameObjectComparer : IComparer<IGameObject>
        {
            public int Compare(IGameObject? x, IGameObject? y) => x.ZIndex.CompareTo(y.ZIndex);
        }
        #endregion

        #region Fields
        private static int m_globId;

        private Dictionary<string, IComponent> m_components;
        private List<IGameObject> m_children;
        #endregion

        #region Propeties

        public Vector2 Position { get; set; }

        public double Rotation { get; set; }//Degree

        public SizeF Scale { get; set; }
        public int Id { get; init; }
        public bool Enabled { get; set; }
        public double ZIndex { get; set; }
        public string Name { get; set; }

        #endregion

        #region Ctor
        public GameObject(string name)
        {
            Init();
            Position = new Vector2(0, 0);
            Rotation = 0;
            Scale = new SizeF(1, 1);
            Id = ++m_globId;

            if (string.IsNullOrEmpty(name))
            {
                Name = name + $"_{Id}";
            }
            else
            {
                Name = name;
            }
        }

        public GameObject(string name, Vector2 position, double rotation, SizeF scale)
        {
            Init();
            Position = position;
            Rotation = rotation;
            Scale = scale;
            Name = name;
            Id = ++m_globId;

            if (string.IsNullOrEmpty(name))
            {
                Name = name + $"_{Id}";
            }
            else
            {
                Name = name;
            }
        }
        #endregion

        #region Methods

        public virtual void Update(List<IGameObject> world)
        {
            if (!Enabled) return;
            Animation? animation = GetComponent<Animation>(nameof(Animation), false);
            Animator? animator = GetComponent<Animator>(nameof(Animator), false);

            if (animator != null)
            {
                animator.Update();
            }
            else if (animation != null)
            {
                animation.Update();
            }

            foreach (var child in m_children)
            {
                child.Update(world);
            }

            //Here must be a custom logic that must be implemented in Derived classes
        }

        public virtual void Render(DrawingContext dc, Matrix parent = default)
        {
            if (!Enabled) return;

            Sprite? sprite = GetComponent<Sprite>(nameof(Sprite));
            Animation? animation = GetComponent<Animation>(nameof(Animation), false);
            Animator? animator = GetComponent<Animator>(nameof(Animator), false);
            //Get An Image for Render
            BitmapSource bitmapSource = null;
            if (animation != null)
            {
                bitmapSource = animation.GetCurrentFrame();
            }
            else if (animator != null)
            {
                bitmapSource = animator.GetCurrentFrame();
            }
            else
            {
                bitmapSource = (BitmapSource)sprite.Image;
            }

            if (bitmapSource == null)
                return;

            //Calculate the center of the Image
            float Xcenter = (float)(bitmapSource.Width * Scale.Width) / 2;
            float Ycenter = (float)(bitmapSource.Height * Scale.Height) / 2;
            //Get matrix for current game object
            var globalMatrix = GetGlobalTransformMatrix(new Vector2(Xcenter, Ycenter));

            if (parent != default)
            {
                globalMatrix.Append(parent);
            }

            dc.PushTransform(new MatrixTransform(globalMatrix));

            dc.DrawImage(bitmapSource, new System.Windows.Rect(0, 0, bitmapSource.Width * Scale.Width,
                bitmapSource.Height * Scale.Height));

            if (GESettings.DrawGizmo)
            {
                //Draw Gizmo
                dc.DrawLine(
                    GESettings.XAxisColor,
                    new System.Windows.Point(Xcenter, Ycenter),
                    new System.Windows.Point(Xcenter + (bitmapSource.Width * Scale.Width) / 2, Ycenter));

                dc.DrawLine(
                    GESettings.YAxisColor,
                    new System.Windows.Point(Xcenter, Ycenter),
                    new System.Windows.Point(Xcenter, Ycenter + (bitmapSource.Height * Scale.Height) / 2));

                dc.DrawEllipse(
                    GESettings.GizmoCenterBrush,
                    GESettings.GizmoCenterPen,
                    new System.Windows.Point(Xcenter, Ycenter),
                    GESettings.GizmoCenterXRadius * Scale.Width,
                    GESettings.GizmoCenterYRadius * Scale.Height);
            }

            if (GESettings.DrawBorders)
            {
                dc.DrawRectangle(
                    GESettings.BorderRectangleBrush,
                    GESettings.BorderRectanglePen,
                    new System.Windows.Rect(
                        0, 0,
                        bitmapSource.Width * Scale.Width,
                        bitmapSource.Height * Scale.Height)
                    );
            }

            if (GESettings.DrawColliders)
            {

            }

            dc.Pop();

            foreach (var item in m_children)
            {
                item.Render(dc, globalMatrix);
            }
        }

        private void Init()
        {
            m_components = new Dictionary<string, IComponent>();
            m_children = new List<IGameObject>();
            Enabled = true;
            ZIndex = 0;
        }

        public IGameObject RegisterComponent(IComponent component)
        {
            if (component == null)
                throw new ArgumentNullException(nameof(component));

            if (m_components.ContainsKey(component.Name))
                throw new ComponentAlreadyRegisteredException(component.Name);

            m_components.Add(component.Name, component);

            return this;
        }

        public IGameObject UnregisterComponent(IComponent component)
        {
            if (component == null)
                throw new ArgumentNullException(nameof(component));

            if (m_components.ContainsKey(component.Name))
                m_components.Remove(component.Name);

            return this;
        }

        public TComponent? GetComponent<TComponent>(string componentName, bool throwException = true)
            where TComponent : IComponent
        {
            if (throwException && !m_components.ContainsKey(componentName))
                throw new ComponentNotFoundException(componentName);

            IComponent component = null;
            if (m_components.TryGetValue(componentName, out component))
            {
                return (TComponent)component;
            }
            return default;
        }

        public Matrix GetGlobalTransformMatrix(Vector2 center)
        {
            //Create I matrix, diagonal is 1
            Matrix matrix = Matrix.Identity;
            //Move to center of the texture
            matrix.Translate(-center.X, -center.Y);
            //Apply scale
            matrix.Scale(Scale.Width, Scale.Height);
            //Apply Rotation
            matrix.Rotate(Rotation);
            //Move back to initial origin
            matrix.Translate(center.X, center.Y);
            //Apply Translate in the World
            matrix.Translate(Position.X, Position.Y);

            return matrix;
        }

        public void AddChild(GameObject child)
        {
            if (child == null) throw new ArgumentNullException(nameof(child));
            m_children.Add(child);
        }

        public void RemoveChild(GameObject child)
        {
            if (child == null) throw new ArgumentNullException(nameof(child));
            m_children.Remove(child);
        }

        #endregion
    }
}
