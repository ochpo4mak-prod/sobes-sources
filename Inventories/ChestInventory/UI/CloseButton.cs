using UnityEngine.UIElements;

public class CloseButton : Button
{
    public CloseButton()
    {
        name = "Close Button";
        AddToClassList("close-button");
    }
}