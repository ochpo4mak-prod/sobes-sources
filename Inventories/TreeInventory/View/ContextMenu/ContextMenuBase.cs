using UnityEngine;
using UnityEngine.UIElements;

public abstract class ContextMenuBase : VisualElement
{
    public ContextMenuBase(VisualElement container, Vector2 mousePosition)
    {
        Container = container;

        name = "Context Menu";
        AddToClassList("context-container");

        style.left = mousePosition.x;
        style.top = mousePosition.y;

        container.Add(this);
    }

    protected VisualElement Container { get; }
}