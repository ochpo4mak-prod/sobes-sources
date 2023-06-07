using UnityEngine.UIElements;
using Assets.Scripts.Table.UI;

public class NodeContainer : VisualElement
{
    public NodeContainer()
    {
        name = "Node Container";
        AddToClassList("node-container");
    }
}