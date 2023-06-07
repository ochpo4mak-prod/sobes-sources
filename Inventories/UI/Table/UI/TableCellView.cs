using Unity.Mathematics;
using UnityEngine.UIElements;
using Assets.Scripts.Table.UI;

public class TableCellView : VisualElement
{
    public TableCellView(int pixelSize, int2 position, ChestInvController chestInvController)
    {
        style.width = pixelSize;
        style.height = pixelSize;
        Position = position;
        ChestInvController = chestInvController;
        AddToClassList("slot-icon");
        RegisterCallback<MouseUpEvent>(OnMouseUpEvent);
    }

    public int2 Position { get; }
    public ChestInvController ChestInvController { get; }

    private void OnMouseUpEvent(MouseUpEvent mouseEvent)
    {
        if (DragAndDrop.IsGrabbing && !DragAndDrop.IsValidDataType<TableCellView>())
        {
            ChestInvController.resultDragPos = Position;
            ChestInvController.movingToItem = false;
        }
    }
}