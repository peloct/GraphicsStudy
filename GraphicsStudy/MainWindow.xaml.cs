using System.Windows;
using GraphicsStudy.UI;
using Microsoft.Win32;

namespace GraphicsStudy
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private FbxSDK.Manager fbxManager;
        private NodeHierarchy nodeHierarchy;

        public MainWindow()
        {
            InitializeComponent();
            fbxManager = FbxSDK.Manager.Create();
        }

        private void MenuItem_LoadFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "FBX files (*.fbx)|*.fbx";
            if (openFileDialog.ShowDialog() == true)
            {
                var scene = fbxManager.CreateSceneFromFile(openFileDialog.FileName);

                if (nodeHierarchy != null)
                {
                    Window window = nodeHierarchy.Parent as Window;
                    window.Close();
                }

                nodeHierarchy = new NodeHierarchy();
                nodeHierarchy.SetNode(scene.GetRootNode());

                Window hierarchyWindow = new Window()
                {
                    Title = "Node Hierarchy",
                    WindowStyle = WindowStyle.ToolWindow,
                    Content = nodeHierarchy
                };

                hierarchyWindow.Show();
                hierarchyWindow.Closed += (obj, arg) =>
                {
                    nodeHierarchy = null;
                };

                sceneViewer.ShowScene(nodeHierarchy, scene);
            }
        }
    }
}
