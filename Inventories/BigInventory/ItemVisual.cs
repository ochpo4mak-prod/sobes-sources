using UnityEngine;
using UnityEngine.UIElements;

public class ItemVisual : VisualElement
{
    private readonly ItemDescription m_Item;
    private Vector2 _startPosition;
    private bool _isDragging;
    private bool _havePlace;
    private Vector2 _finalPlacementPos;

    public ItemVisual(ItemDescription item)
    {
        m_Item = item;

        VisualElement icon = new VisualElement
        {
            style = { backgroundImage = m_Item.Icon.texture },
            name = "Icon"
        };

        Add(icon);

        icon.AddToClassList("visual-icon");
        AddToClassList("visual-icon-container");
        
        name = m_Item.Name;
        style.visibility = Visibility.Hidden;
        style.height = m_Item.Size.y * GameInventory.slotSize.y;
        style.width = m_Item.Size.x * GameInventory.slotSize.x;

        RegisterCallback<MouseMoveEvent>(OnMouseMoveEvent);
        RegisterCallback<MouseUpEvent>(OnMouseUpEvent);
    }

    private void OnMouseMoveEvent(MouseMoveEvent mouseEvent)
    {
        if (!_isDragging)
            return; 

        SetPosition(GetMousePosition(mouseEvent.mousePosition));

        _havePlace = GameInventory.sample.CheckPlacement(this);

        if (_havePlace)
            _finalPlacementPos = GameInventory.sample.CalculationPos(this);
    }

    private void OnMouseUpEvent(MouseUpEvent mouseEvent)
    {
        if (!_isDragging)
        {
            StartDrag();
            return;
        }

        _isDragging = false;

        if (_havePlace)
        {
            SetPosition(new Vector2(
                _finalPlacementPos.x - parent.worldBound.position.x,
                _finalPlacementPos.y - parent.worldBound.position.y));
            return;
        }

        SetPosition(new Vector2(_startPosition.x, _startPosition.y));
    }

    public Vector2 GetMousePosition(Vector2 mousePosition)
    {
        return new Vector2(mousePosition.x - (layout.width / 2) - parent.worldBound.position.x, mousePosition.y - (layout.height / 2) - parent.worldBound.position.y);
    }

    public void SetPosition(Vector2 pos)
    {
        style.left = pos.x;
        style.top = pos.y;
    }

    public void StartDrag()
    {
        _isDragging = true;
        _startPosition = worldBound.position - parent.worldBound.position;
        BringToFront();
    }
}