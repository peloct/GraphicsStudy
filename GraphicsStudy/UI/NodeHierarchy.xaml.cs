using System.Windows;
using System.Windows.Controls;

namespace GraphicsStudy.UI
{
    /// <summary>
    /// NodeHierarchy.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class NodeHierarchy : UserControl
    {
        public FbxSDK.Node CurSelected { get; private set; }

        public NodeHierarchy()
        {
            InitializeComponent();
            treeView.SelectedItemChanged += OnSelectedItemChanged;
        }

        private void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var item = e.NewValue as TreeViewItem;
            CurSelected = null;
            if (item != null)
                CurSelected = item.Tag as FbxSDK.Node;
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
            viewItem.Tag = node;

            for (int i = 0; i < node.GetChildCount(); ++i)
            {
                var child = node.GetChild(i);
                AddNode(viewItem.Items, child);
            }

            collection.Add(viewItem);
        }
    }
}
