using ViewModelBaseLibDotNetCore.VM;
using WPFGameEngine.WPF.GE.Geometry.Base;

namespace SpaceAvenger.Editor.ViewModels.GeometryConfigViewModel.GeometryConfigBase
{
    internal abstract class GeometryConfigViewModelBase : ViewModelBase
    {
        #region Fields
        private string m_geometryName;
        #endregion

        #region Properties
        public string GeometryName 
        { get => m_geometryName; set => Set(ref m_geometryName, value); }

        protected IShape2D Shape2D;
        #endregion

        #region Ctor
        public GeometryConfigViewModelBase(string geometryName, IShape2D shape2D)
        {
            m_geometryName = geometryName;
            Shape2D = shape2D;
            LoadCurrentGeometryProperties();
        }
        #endregion

        #region Methods
        protected abstract void LoadCurrentGeometryProperties();
        #endregion
    }
}
