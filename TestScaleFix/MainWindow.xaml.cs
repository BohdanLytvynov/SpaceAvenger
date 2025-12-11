using SpaceAvenger.Editor.Mock;
using SpaceAvenger.Services;
using SpaceAvenger.Services.ResourceLoader;
using SpaceAvenger.Services.WpfGameViewHost;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFGameEngine.Timers;
using WPFGameEngine.WPF.GE.Component.Sprites;
using WPFGameEngine.WPF.GE.Component.Transforms;
using WPFGameEngine.WPF.GE.Settings;

namespace TestScaleFix
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WpfGameObjectViewHost host;

        public MainWindow()
        {
            InitializeComponent();

            GESettings.DrawBorders = true;
            GESettings.DrawGizmo = true;
            GameObjectMock go = new GameObjectMock();
            var rl = new WPFResourceLoader(Constants.PATH_TO_CONTENT);
            var sprite = new Sprite(rl);
            sprite.Load("F1_Corvette");
            go.RegisterComponent(sprite);
            var t = new TransformComponent();
            t.Scale = new WPFGameEngine.WPF.GE.Math.Sizes.Size(0.7f, 0.7f);
            go.RegisterComponent(t);
            host = new WpfGameObjectViewHost(new GameTimer());
            host.AddObject(go);
            host.StartGame();
            content.Content = host;
        }
    }
}