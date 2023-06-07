using System.Threading;
using UnityEngine.UIElements;

public class PreviewNode : VisualElement
{
    public PreviewNode() { }

    public PreviewNode(ListNode listNode, BoxNodesCollection horizontalListCollection, ChestContainer chestContainer, VisualElement container)
    {
        name = "Preview Node";

        var closeButton = new CloseButton();
        var searchText = new Label();

        SearchText = searchText;
        Node = listNode.Node;
        Container = container;
        ChestContainer = chestContainer;
        HorizontalListCollection = horizontalListCollection;

        AddToClassList("preview-node");
        searchText.AddToClassList("search-text");

        Add(listNode.Icon);
        Add(closeButton);
        Add(searchText);

        closeButton.clicked += OnCloseButtonClick;
    }

    public IBox Node { get; }
    public Label SearchText { get; }
    public VisualElement Container { get; }
    public ChestContainer ChestContainer { get; }
    public BoxNodesCollection HorizontalListCollection { get; }

    private void OnCloseButtonClick()
    {
        var cancelationTokenSource = new CancellationTokenSource();
        var cancelationToken = cancelationTokenSource.Token;
        HorizontalListCollection.AddNode(Node, cancelationToken);

        Container.Remove(ChestContainer);
    }
}