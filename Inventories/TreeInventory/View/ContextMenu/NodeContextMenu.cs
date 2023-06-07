using System;
using UnityEngine;
using Taxonomy.Model;
using UnityEngine.UIElements;

public class NodeContextMenu : ContextMenuBase
{
    private Action<string> _removeDelegate;
    private IGraphNode _graphNode;
    private NodeView _nodeView;

    public NodeContextMenu(VisualElement container, Vector2 mousePosition, Action<string> removeDelegate, IGraphNode graphNode, NodeView nodeView)
        : base(container, mousePosition)
    {
        _removeDelegate = removeDelegate;
        _graphNode = graphNode;
        _nodeView = nodeView;

        var deleteButton = new Button() { text = "Delete Node" };
        deleteButton.AddToClassList("context-button");
        deleteButton.clicked += ClickOnDeleteButton;
        Add(deleteButton);
    }

    private void ClickOnDeleteButton()
    {
        try
        {
            TreeInventoryController.folderViewsList.Remove(_nodeView.treeNodeView.treeFolderView);
        }
        catch {}

        _nodeView.parent.Remove(_nodeView);

        _removeDelegate.Invoke(_graphNode.Id);

        Container.Remove(Container.ElementAt(2));
    }
}