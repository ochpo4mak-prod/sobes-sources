using UnityEngine.UIElements;

public abstract class ModalWindowBase : VisualElement
{
    public ModalWindowBase(VisualElement container)
    {
        name = "Modal Window";

        var title = new Label();
        var backgroundContainer = new VisualElement();

        Title = title;
        Container = container;

        AddToClassList("modal-container");
        title.AddToClassList("modal-title");
        backgroundContainer.AddToClassList("modal-background");

        container.Add(backgroundContainer);
        backgroundContainer.Add(this);
        Add(title);
    }

    protected Label Title { get; }
    protected VisualElement Container { get; }
}