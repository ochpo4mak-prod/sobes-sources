using UnityEngine;
using System.Linq;
using System.Threading;
using Unity.Mathematics;
using Assets.Scripts.Table;
using UnityEngine.UIElements;
using Assets.Scripts.Table.UI;
using System.Collections.Generic;

public class InventoryController : MonoBehaviour
{
    public int2 resultDragPos;
    public bool movingToItem = true;
    public TableView activeTableForDrag;
    public List<ContentToIconPair> Contents;
    public ITableNodesCollection collectionForDrag;
    public ITableNodesCollection previusDragCollection;

    private ITableNodesCollection _groundNodesCollection;
    private ITableNodesCollection _inventoryNodesCollection;

    private VisualElement m_Root;
    private VisualElement m_Container;

    private int2 _startDragPos;
    private GhostIcon _ghostIcon;
    private Image _draggableImage;
    private TableNodeView _draggableNode;
    
    private void Start()
    {
        m_Root = GetComponentInChildren<UIDocument>().rootVisualElement;
        m_Container = m_Root.Q<VisualElement>("Container");

        m_Container.RegisterCallback<MouseUpEvent>(OnMouseUpEvent);
        m_Container.RegisterCallback<MouseDownEvent>(OnMouseDownEvent);
        m_Container.RegisterCallback<MouseMoveEvent>(OnMouseMoveEvent);

        _groundNodesCollection = new TableNodesCollection(ENodeContent.Default);
        _inventoryNodesCollection = new TableNodesCollection(ENodeContent.Default);

        var tableGroundView = new TableView(_groundNodesCollection, Contents, 70, new int2(10, 15), new ChestInvController(), new SearchView(), new PreviewNode());
        var tableInventoryView = new TableView(_inventoryNodesCollection, Contents, 70, new int2(10, 15), new ChestInvController(), new SearchView(), new PreviewNode());

        m_Container.Add(tableGroundView);
        m_Container.Add(tableInventoryView);

        AddItemToTable(ENodeContent.Pistol, new RectInt(0, 0, 3, 2), _groundNodesCollection);
        AddItemToTable(ENodeContent.Health, new RectInt(3, 0, 2, 2), _groundNodesCollection);

        AddItemToTable(ENodeContent.Rifle, new RectInt(0, 0, 4, 3), _inventoryNodesCollection);
    }

    private void AddItemToTable(ENodeContent content, RectInt rect, ITableNodesCollection collection)
    {
        var node = new Assets.Scripts.Table.Node(content);

        var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;

        collection.AddNode(node, rect, cancellationToken);
    }

    private void OnMouseDownEvent(MouseDownEvent mouseEvent)
    {
        if (!DragAndDrop.IsGrabbing)
        {      
            if (mouseEvent.target?.GetType() == typeof(Image))
            {
                _draggableImage = (Image)mouseEvent.target;
                _draggableNode = (TableNodeView)_draggableImage.parent;
            }
            else
                return;

            DragAndDrop.StartDrag(_draggableNode);
            _startDragPos = _draggableNode.Node.Position;
            _draggableNode.style.visibility = Visibility.Hidden;

            _ghostIcon = new GhostIcon(_draggableImage);
            m_Container.Add(_ghostIcon);            
            return;
        }
    }

    private void OnMouseUpEvent(MouseUpEvent mouseEvent)
    {
        if (DragAndDrop.IsGrabbing)
        {
            _draggableNode.Node.ChangePosition(resultDragPos);

            if (movingToItem && previusDragCollection != null)
                MovingToAnotherTable(previusDragCollection, _startDragPos);

            if (CheckingOverlapOfObjects())
                _draggableNode.Node.ChangePosition(_startDragPos);

            if (CheckingGoingAbroad())
                _draggableNode.Node.ChangePosition(_startDragPos);

            _draggableNode.style.visibility = Visibility.Visible;
            m_Container.Remove(_ghostIcon);
            DragAndDrop.EndDrag();

            movingToItem = true;
            collectionForDrag = null;
            previusDragCollection = null;
            _draggableImage = null;
            _draggableNode = null;
        }
    }

    private void OnMouseMoveEvent(MouseMoveEvent mouseEvent)
    {
        if (!DragAndDrop.IsGrabbing)
            return; 

        Vector2 pos = new Vector2(mouseEvent.mousePosition.x - (_ghostIcon.layout.width / 4) - _ghostIcon.parent.worldBound.position.x, mouseEvent.mousePosition.y -
            (_ghostIcon.layout.height / 4) - _ghostIcon.parent.worldBound.position.y);

        _ghostIcon.style.left = pos.x;
        _ghostIcon.style.top = pos.y;
    }

    private bool CheckingGoingAbroad()
    {
        if (_draggableNode.Node.Position.x + _draggableNode.Node.Size.x > activeTableForDrag.countColumnRows.x ||
        _draggableNode.Node.Position.y + _draggableNode.Node.Size.y > activeTableForDrag.countColumnRows.y)
        {
            if (previusDragCollection != null)
                MovingToAnotherTable(previusDragCollection, _startDragPos);
                
            return true;
        }
        return false;
    }

    private bool CheckingOverlapOfObjects()
    {
        List<RectInt> itemsRect = new List<RectInt>();

        foreach (var item in collectionForDrag.Nodes)
            for (int i = 0; i < collectionForDrag.Nodes.Length; ++i)
                itemsRect.Add(new RectInt(item.Position.x, item.Position.y, item.Size.x, item.Size.y));

        RectInt dragItemRect = new RectInt(_draggableNode.Node.Position.x, _draggableNode.Node.Position.y, _draggableNode.Node.Size.x, _draggableNode.Node.Size.y);

        itemsRect.Remove(dragItemRect);

        var overlapping = itemsRect.Where(x => x.Overlaps(dragItemRect)).ToArray();

        if (overlapping.Length > 2)
        {
            if (previusDragCollection != null)
                MovingToAnotherTable(previusDragCollection, _startDragPos);

            return true;
        }
        return false;
    }

    public void MovingToAnotherTable(ITableNodesCollection collection, int2 position)
    {
        var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;
        _draggableNode.Node.Move(collection, position, cancellationToken);
    }
}