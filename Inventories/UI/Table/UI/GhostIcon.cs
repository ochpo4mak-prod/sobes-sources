using UnityEngine.UIElements;

public class GhostIcon : VisualElement
{
    public GhostIcon(Image img)
    {
        AddToClassList("table-node");
        name = "ghost-icon";

        pickingMode = PickingMode.Ignore;
        style.backgroundImage = img.sprite.texture;
        style.width = img.parent.style.width;
        style.height = img.parent.style.height;
        style.left = img.parent.worldBound.x;
        style.top = img.parent.worldBound.y;
    }
}