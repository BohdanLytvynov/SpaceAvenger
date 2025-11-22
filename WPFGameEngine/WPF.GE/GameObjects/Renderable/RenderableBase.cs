using System.Windows.Media;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.Component.RelativeTransforms;
using WPFGameEngine.WPF.GE.GameObjects.Transformable;
using WPFGameEngine.WPF.GE.Math.Matrixes;
using WPFGameEngine.WPF.GE.Math.Sizes;
using WPFGameEngine.WPF.GE.Settings;

namespace WPFGameEngine.WPF.GE.GameObjects.Renderable
{
    public abstract class RenderableBase : UpdatableBase, IRenderable
    {
        public bool IsVisible { get; set; }

        public override void StartUp(IGameObjectViewHost viewHost, IGameTimer gameTimer)
        {
            IsVisible = true;
            base.StartUp(viewHost, gameTimer);
        }

        //Need to move code that is dependent on System.Windows.Media
        //And all related classes like Bitmapsource, DrawingContext, Matrix, SizeF,
        //GetLocalTransform matrix
        public virtual void Render(DrawingContext dc, Matrix3x3 parent)
        {
            if (!Enabled) return;

            if (!IsVisible) return;

            if (Texture == null) return;

            var actualWidth = Transform.ActualSize.Width;
            var actualHeight = Transform.ActualSize.Height;
            var globalMatrix = Transform.GetLocalTransformMatrix();

            if (parent != Matrix3x3.Identity)
            {
                globalMatrix *= parent;
            }

            Matrix m = new Matrix();
            m.M11 = globalMatrix.M11;
            m.M12 = globalMatrix.M12;
            m.M21 = globalMatrix.M21;
            m.M22 = globalMatrix.M22;
            m.OffsetX = globalMatrix.OffsetX;
            m.OffsetY = globalMatrix.OffsetY;

            dc.PushTransform(new MatrixTransform(m));

            var Xcenter = Transform.ActualCenterPosition.X;
            var Ycenter = Transform.ActualCenterPosition.Y;

            dc.DrawImage(Texture, new System.Windows.Rect
                (0, 0, actualWidth, actualHeight));

            if (GESettings.DrawGizmo)
            {
                //Draw Gizmo
                dc.DrawLine(
                    GESettings.XAxisColor,
                    new System.Windows.Point(Xcenter, Ycenter),
                    new System.Windows.Point(Xcenter + actualWidth * (1 - Transform.CenterPosition.X), Ycenter));

                dc.DrawLine(
                    GESettings.YAxisColor,
                    new System.Windows.Point(Xcenter, Ycenter),
                    new System.Windows.Point(Xcenter, Ycenter + actualHeight * (1 - Transform.CenterPosition.Y)));

                dc.DrawEllipse(
                    GESettings.GizmoCenterBrush,
                    GESettings.GizmoCenterPen,
                    new System.Windows.Point(Xcenter, Ycenter),
                    GESettings.GizmoCenterXRadius * Transform.Scale.Width,
                    GESettings.GizmoCenterYRadius * Transform.Scale.Height);
            }

            if (GESettings.DrawBorders)
            {
                dc.DrawRectangle(
                    GESettings.BorderRectangleBrush,
                    GESettings.BorderRectanglePen,
                    new System.Windows.Rect(
                        0, 0,
                        actualWidth,
                        actualHeight)
                    );
            }

            dc.Pop();

            foreach (var item in Children)
            {
                if (item is ITransformable transformable)
                {
                    var childTransform = transformable.Transform as IRelativeTransform;
                    if (childTransform != null)
                    {
                        childTransform.ActualParentSize = new Size(actualWidth, actualHeight);
                    }
                }

                if(item is IRenderable renderable)
                    renderable.Render(dc, globalMatrix);
            }
        }

        public void Hide()
        {
            IsVisible = false;
        }

        public void Show()
        {
            IsVisible = true;
        }
    }
}
