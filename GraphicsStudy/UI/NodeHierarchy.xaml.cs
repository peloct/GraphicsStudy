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
            var attribute = node.GetAttribute();

            if (attribute != null)
            {
                var attributeType = attribute.GetAttributeType();
                name += string.Format(" ({0})", attributeType);
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
