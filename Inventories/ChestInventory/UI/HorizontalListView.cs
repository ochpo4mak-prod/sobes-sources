using UnityEngine.UIElements;
using System.Collections.Generic;

public class HorizontalListView : ScrollView
{
    public HorizontalListView(IBoxNodesCollection collection, List<ContentToIcon> contents)
    {
        name = "Horizontal List View";
        AddToClassList("horizontal-list-view");
        verticalScrollerVisibility = ScrollerVisibility.Hidden;
        mode = ScrollViewMode.Horizontal;

        Contents = contents;
        Collection = collection;

        Collection.AddedEvent += OnNodeAdded;
        Collection.RemovedEvent += OnNodeRemoved;
    }

    public IBoxNodesCollection Collection { get; }
    public List<ContentToIcon> Contents { get; }

    private void OnNodeAdded(IBox node)
    {
        foreach (var content in Contents)
            if (content.Type == node.Type)
                Add(new ListNode(content, Collection, node));
    }

    private void OnNodeRemoved(ListNode node)
    {
        Remove(node);
    }
}