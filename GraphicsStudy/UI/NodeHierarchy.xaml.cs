using System.Windows.Controls;

namespace GraphicsStudy.UI
{
    /// <summary>
    /// NodeHierarchy.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class NodeHierarchy : UserControl
    {
        public NodeHierarchy()
        {
            InitializeComponent();
        }

        public void SetNode(FbxSDK.Node rootNode)
        {
            treeView.Items.Clear();
            AddNode(treeView.Items, rootNode);
        }

        public void AddNode(ItemCollection collection, FbxSDK.Node node)
        {
            TreeViewItem viewItem = new TreeViewItem();
            string name = node.GetName();

            int attributeCount = node.GetAttributeCount();

            if (attributeCount > 0)
            {
                string[] tags = new string[attributeCount];

                for (int i = 0; i < attributeCount; ++i)
                {
                    var attributeType = node.GetAttributeType(i);
                    if (attributeType == FbxSDK.NodeAttributeType.Camera)
                        tags[i] = "Camera";
                    else if (attributeType == FbxSDK.NodeAttributeType.Light)
                        tags[i] = "Light";
                    else if (attributeType == FbxSDK.NodeAttributeType.Mesh)
                        tags[i] = "Mesh";
                    else if (attributeType == FbxSDK.NodeAttributeType.Skeleton)
                        tags[i] = "Skeleton";
                    else
                        tags[i] = "Unkown";
                }

                name += string.Format(" ({0})", string.Join(", ", tags));
            }

            viewItem.Header = name;

            for (int i = 0; i < node.GetChildCount(); ++i)
            {
                var child = node.GetChild(i);
                AddNode(viewItem.Items, child);
            }

            collection.Add(viewItem);
        }
    }
}
