using Taxonomy.Model;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class HistoryView : Label
{
    private ITreeNode _treeNode;
    private VisualElement _container;

    public HistoryView(VisualElement container, ITreeNode treeNode)
    {
        _treeNode = treeNode;
        _container = container;

        if (container.childCount > 0)
        {
            var arrow = new Label() { text = ">" };
            arrow.AddToClassList("history-text");
            container.Add(arrow);
        }

        AddToClassList("history-text");
        text = treeNode.Name;
        container.Add(this);

        RegisterCallback<MouseDownEvent>(OnMouseDownEvent);
    }

    private void OnMouseDownEvent(MouseDownEvent evt)
    {
        _container.parent.parent.ElementAt(1).Clear();

        var registryContainer = _container.parent;
        var openTreeLabel = (Label)_container.ElementAt(_container.childCount - 1);

        if (openTreeLabel.text == text)
            return;

        var haveFolder = true;
        foreach (var folderView in TreeInventoryController.folderViewsList)
        {
            if (folderView.name == openTreeLabel.text)
                folderView.style.display = DisplayStyle.None;

            if (folderView.name == _treeNode.Name)
            {
                folderView.style.display = DisplayStyle.Flex;
                haveFolder = false;
            }
        }
        if (haveFolder)
            new TreeNodeView(registryContainer, _treeNode);

        var indexes = new List<int>();
        for (var i = _container.IndexOf(this) + 1; i < _container.childCount; i++)
            indexes.Add(i);

        indexes.Reverse();
        foreach (var index in indexes)
            _container.Remove(_container.hierarchy.ElementAt(index));
    }
}
