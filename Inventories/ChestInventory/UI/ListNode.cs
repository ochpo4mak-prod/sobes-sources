using Unity.Mathematics;
using UnityEngine.UIElements;

public class ListNode : VisualElement
{
    public ListNode(ContentToIcon content, IBoxNodesCollection collection, IBox node)
    {
        Node = node;
        Size = content.Size;
        Collection = collection;
        Icon = new Image { sprite = content.Sprite };

        name = "List Node";
        AddToClassList("list-node");
        Add(Icon);

        RegisterCallback<MouseDownEvent>(OnMouseDownEvent);
    }

    public IBox Node { get; }
    public int2 Size { get; }
    public Image Icon { get; }
    public IBoxNodesCollection Collection { get; }

    public void OnMouseDownEvent(MouseDownEvent mouseEvent)
    {
        if (mouseEvent.clickCount == 2)
            Collection.RemoveNode(this);
    }
}