using UnityEngine;
using System.Linq;
using System.Threading;
using Unity.Mathematics;
using Assets.Scripts.Table;
using UnityEngine.UIElements;
using Assets.Scripts.Table.UI;
using System.Collections.Generic;

public class ChestInvController : MonoBehaviour
{
    public TableView activeTableForDrag;
    public List<ContentToIcon> ContentsBox;
    public List<ContentToIconPair> ContentsNode;
    public ITableNodesCollection collectionForDrag;
    public ITableNodesCollection previusDragCollection;

    [HideInInspector] public int2 resultDragPos;
    [HideInInspector] public bool movingToItem = true;

    private BoxNodesCollection _horizontalListCollection;
    private VisualElement _chestContainerList;
    private TableNodeView _draggableNode;
    private float _upPaddingForDrag;
    private ScrollView m_Container;
    private SearchView _searchView;
    private Image _draggableImage;
    private GhostIcon _ghostIcon;
    private VisualElement m_Root;
    private int2 _startDragPos;

    private void Start()
    {
        m_Root = GetComponentInChildren<UIDocument>().rootVisualElement;
        m_Container = m_Root.Q<ScrollView>("Container");

        m_Container.RegisterCallback<MouseUpEvent>(OnMouseUpEvent);
        m_Container.RegisterCallback<MouseDownEvent>(OnMouseDownEvent);
        m_Container.RegisterCallback<MouseMoveEvent>(OnMouseMoveEvent);

        _horizontalListCollection = new BoxNodesCollection();
        _horizontalListCollection.RemovedEvent += FromHorizontalListRemoved;

        var listView = new HorizontalListView(_horizontalListCollection, ContentsBox);
        m_Container.Add(listView);

        _searchView = new SearchView(ContentsNode);
        m_Container.Add(_searchView);

        _chestContainerList = new VisualElement() { name = "Chest Container List" };
        m_Container.Add(_chestContainerList);

        AddItemToHorizontalList(EChestType.Gold, 18);
        AddItemToHorizontalList(EChestType.Iron, 15);
        AddItemToHorizontalList(EChestType.Wood, 10);
        AddItemToHorizontalList(EChestType.Wood, 11);
        AddItemToHorizontalList(EChestType.Iron, 15);
        AddItemToHorizontalList(EChestType.Gold, 18);
        AddItemToHorizontalList(EChestType.Iron, 15);
        AddItemToHorizontalList(EChestType.Wood, 10);
        AddItemToHorizontalList(EChestType.Wood, 11);
        AddItemToHorizontalList(EChestType.Iron, 15);
    }

    private ChestContent[] GenerateRandomContents(int contentsCount, EChestType chestType)
    {
        var chestSize = new int2();
        foreach (var box in ContentsBox)
        {
            if (chestType == box.Type)
                chestSize = new int2(box.Size);
        }

        var contentInChest = new List<ChestContent>();

        var x = 0;
        var y = 0;
        var maxY = 0;

        for (int i = 0; i < contentsCount; i++)
        {
            var randomContent = new System.Random().Next(0, 5);
            var contentSize = new int2(ContentsNode[randomContent].Size);

            if (x + contentSize.x > chestSize.x)
            {
                x = 0;
                y += maxY;
                maxY = 0;
            }

            var chestContent = new ChestContent(ContentsNode[randomContent].Content, new RectInt(x, y, contentSize.x, contentSize.y));

            if (y + contentSize.y <= chestSize.y)
                contentInChest.Add(chestContent);

            x += contentSize.x;
            
            if (contentSize.y > maxY)
                maxY = contentSize.y;
        }

        return contentInChest.ToArray();
    }

    private void AddItemToHorizontalList(EChestType chestType, int contentsCount)
    {
        var box = new Box(chestType, GenerateRandomContents(contentsCount, chestType));

        var cancelationTokenSource = new CancellationTokenSource();
        var cancelationToken = cancelationTokenSource.Token;

        _horizontalListCollection.AddNode(box, cancelationToken);
    }
    
    private void AddItemToChestTable(ENodeContent content, RectInt rect, ITableNodesCollection collection)
    {
        var node = new Assets.Scripts.Table.Node(content);

        var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;

        collection.AddNode(node, rect, cancellationToken);
    }

    private void FromHorizontalListRemoved(ListNode listNode)
    {
        if (_upPaddingForDrag != _chestContainerList.layout.y)
            _upPaddingForDrag = _chestContainerList.layout.y;

        var chestContainer = new ChestContainer();
        _chestContainerList.Add(chestContainer);

        var previewNode = new PreviewNode(listNode, _horizontalListCollection, chestContainer, _chestContainerList);
        chestContainer.Add(previewNode);

        var chestContentCollection = new TableNodesCollection(ENodeContent.Default);
        chestContainer.Add(new TableView(chestContentCollection, ContentsNode, 70, listNode.Size, this, _searchView, previewNode));

        foreach (var chestContent in listNode.Node.Contents)
            AddItemToChestTable(chestContent.Content, chestContent.Rect, chestContentCollection);
    }

    private void OnMouseDownEvent(MouseDownEvent mouseEvent)
    {
        if (!DragAndDrop.IsGrabbing)
        {
            try
            {
                _draggableImage = (Image)mouseEvent.target;
                _draggableNode = (TableNodeView)_draggableImage.parent;

                foreach (var element in _searchView.SearchActivity.Elements)
                    element.RemoveFromClassList("searched-item");
            }
            catch
            {
                return;
            }

            DragAndDrop.StartDrag(_draggableNode);
            _startDragPos = _draggableNode.Node.Position;
            _draggableNode.style.visibility = Visibility.Hidden;

            _ghostIcon = new GhostIcon(_draggableImage);
            m_Container.Add(_ghostIcon);

            var mousePos = _chestContainerList.WorldToLocal(mouseEvent.mousePosition);
            _ghostIcon.style.left = mousePos.x - (_ghostIcon.style.width.value.value / 4);
            _ghostIcon.style.top = mousePos.y - (_ghostIcon.style.height.value.value / 4) + _upPaddingForDrag;

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

        var mousePos = _chestContainerList.WorldToLocal(mouseEvent.mousePosition);

        var pos = new Vector2(mousePos.x - (_ghostIcon.layout.width / 4) - _ghostIcon.parent.worldBound.position.x,
                                  mousePos.y - (_ghostIcon.layout.height / 4) - _ghostIcon.parent.worldBound.position.y + _upPaddingForDrag);

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
        var itemsRect = new List<RectInt>();

        foreach (var item in collectionForDrag.Nodes)
            itemsRect.Add(new RectInt(item.Position.x, item.Position.y, item.Size.x, item.Size.y));

        var dragItemRect = new RectInt(_draggableNode.Node.Position.x, _draggableNode.Node.Position.y, _draggableNode.Node.Size.x, _draggableNode.Node.Size.y);

        itemsRect.Remove(dragItemRect);

        var overlapping = itemsRect.Where(x => x.Overlaps(dragItemRect)).ToArray();

        if (overlapping.Length > 0)
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