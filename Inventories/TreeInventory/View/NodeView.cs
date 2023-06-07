using Taxonomy.Model;
using UnityEngine.UIElements;

public class NodeView : Foldout
{
    private IGraphNode _graphNode;
    private VisualElement _container;
    private ITreeFolder _treeFolder;
    public TreeNodeView treeNodeView;

    public NodeView(IGraphNode graphNode, VisualElement container, ITreeFolder treeFolder)
    {
        value = false;
        text = graphNode.Name;
        name = "Node View";
        AddToClassList("foldout");

        if (graphNode is ITreeNode)
            AddToClassList("tree-node");
        else
            AddToClassList("node");

        _graphNode = graphNode;
        _container = container;
        _treeFolder = treeFolder;

        var elementForEvent = hierarchy.ElementAt(0).hierarchy.ElementAt(0);
        elementForEvent.RegisterCallback<MouseDownEvent>(OnMouseDownEvent);
    }

    private void OnMouseDownEvent(MouseDownEvent evt)
    {
        _container.parent.ElementAt(1).Clear();

        try
        {
            _container.parent.Remove(_container.parent.ElementAt(2));
        }
        catch { }

        if (evt.button == 1)
        {
            new NodeContextMenu(_container.parent, evt.mousePosition, DeleteNode, _graphNode, this);
        }

        if (evt.clickCount >= 2)
        {
            if (_graphNode is ITreeNode treeNode)
            {
                var parentElement = parent;

                while (true)
                {
                    if (parentElement.parent is not TreeFolderView)
                        break;

                    parentElement = parentElement.parent;
                }

                var haveFolder = true;
                foreach (var folderView in TreeInventoryController.folderViewsList)
                {
                    if (folderView.name == parentElement.name)
                        folderView.style.display = DisplayStyle.None;

                    if (folderView.name == treeNode.Name)
                    {
                        folderView.style.display = DisplayStyle.Flex;
                        haveFolder = false;
                    }
                }
                if (haveFolder)
                    treeNodeView = new TreeNodeView(_container, treeNode);

                new HistoryView(_container.hierarchy.ElementAt(0), treeNode);
            }
            else
                new InformationView(_container.parent.ElementAt(1), _graphNode);
        }
        else
            new InformationView(_container.parent.ElementAt(1), _graphNode);
    }

    private void DeleteNode(string id)
    {
        _treeFolder.Nodes.RemoveNode(id);
    }
}
