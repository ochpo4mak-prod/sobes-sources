using System;
using Unity.Mathematics;
using UnityEngine.UIElements;
using System.Collections.Generic;

namespace Assets.Scripts.Table.UI
{
    public class TableView : ScrollView, IDisposable
    {
        private Dictionary<string, TableNodeViewBase> _nodeId2NodeView = new Dictionary<string, TableNodeViewBase>();
        private List<TableNodeViewBase> _tableNodeViews = new List<TableNodeViewBase>();
        private ITableNodeViewFactory _nodeViewFactory;

        public TableView() { }
        
        public TableView(ITableNodesCollection collection, IEnumerable<ContentToIconPair> contents, int pixelSize,
            int2 countColumnRows, ChestInvController chestInvController, SearchView searchView, PreviewNode previewNode)
        {
            SearchView = searchView;
            ChestInvController = chestInvController;
            PreviewNode = previewNode;
            InitVisual(pixelSize, countColumnRows);
            InitDatas(collection, contents);
        }

        public void Dispose()
        {
            Collection.AddedEvent -= OnNodeAdded;
            Collection.RemovedEvent -= OnNodeRemoved;
        }

        public new class UxmlFactory : UxmlFactory<TableView, UxmlTraits> { }
        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            UxmlIntAttributeDescription m_pixelSizeAttr = new UxmlIntAttributeDescription { name = "pixel-size" };
            UxmlIntAttributeDescription m_countColumnRowsAttr = new UxmlIntAttributeDescription { name = "count-column-rows" };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                var tableView = ve as TableView;
                var pixelSize = m_pixelSizeAttr.GetValueFromBag(bag, cc);
                var countColumnRows = m_countColumnRowsAttr.GetValueFromBag(bag, cc);

                tableView.InitVisual(pixelSize, countColumnRows);
            }
        }

        public ITableNodesCollection Collection { get; private set; }
        public int2 countColumnRows { get; private set; }
        public int pixelSize { get; private set; }
        public ChestInvController ChestInvController { get; }
        public PreviewNode PreviewNode { get; }
        public SearchView SearchView { get; }

        public void InitVisual(int pixelSize, int2 countColumnRows)
        {
            name = "TableView";
            AddToClassList("table-view");
            this.pixelSize = pixelSize;
            this.countColumnRows = countColumnRows;

            mode = ScrollViewMode.Vertical;
            horizontalScrollerVisibility = ScrollerVisibility.Hidden;

            var nodeContainer = new NodeContainer();
            Add(nodeContainer);

            nodeContainer.style.width = pixelSize * countColumnRows.x;
            nodeContainer.style.height = pixelSize * countColumnRows.y;

            for(var x = 0; x < countColumnRows.y; ++x)
            {
                for(var y = 0; y < countColumnRows.x; ++y)
                {
                    nodeContainer.Add(new TableCellView(pixelSize, new int2(y, x), ChestInvController));
                }
            }

            RegisterCallback<MouseUpEvent>(OnMouseUpEvent);
            RegisterCallback<MouseDownEvent>(OnMouseDownEvent);
        }

        private void OnMouseDownEvent(MouseDownEvent mouseEvent)
        {
            if (!DragAndDrop.IsGrabbing)
                ChestInvController.collectionForDrag = Collection;
        }

        private void OnMouseUpEvent(MouseUpEvent mouseEvent)
        {
            if (DragAndDrop.IsGrabbing)
            {
                ChestInvController.activeTableForDrag = this;

                if (ChestInvController.collectionForDrag != Collection)
                {
                    ChestInvController.previusDragCollection = ChestInvController.collectionForDrag;
                    ChestInvController.collectionForDrag = Collection;
                    ChestInvController.MovingToAnotherTable(ChestInvController.collectionForDrag, ChestInvController.resultDragPos);
                }
            }
        }

        public void InitDatas(ITableNodesCollection collection, IEnumerable<ContentToIconPair> contents)
        {
            Collection = collection;
            _nodeViewFactory = new TableNodeViewFactory(this, contents);

            Collection.AddedEvent += OnNodeAdded;
            Collection.RemovedEvent += OnNodeRemoved;
        }

        private void OnNodeAdded(ITableNode tableNode)
        {
            if (tableNode.Node is IContent content)
            {
                Add(tableNode.Id, _nodeViewFactory.CreateTableNodeView(tableNode, content, SearchView, this, PreviewNode));
                return;
            }

            Add(tableNode.Id, _nodeViewFactory.CreateUndefinedTableNodeView(tableNode));
        }

        private void OnNodeRemoved(ITableNode tableNode)
        {
            if (!_nodeId2NodeView.TryGetValue(tableNode.Id, out var nodeView))
                throw new ArgumentException($"TableNodeViewBase with id {tableNode.Id} not contains in TableView");

            Remove(tableNode.Id, nodeView);
        }

        private void Add(string nodeId, TableNodeViewBase nodeView)
        {
            _nodeId2NodeView.Add(nodeId, nodeView);
            _tableNodeViews.Add(nodeView);
            Add(nodeView);
        }

        private void Remove(string nodeId, TableNodeViewBase nodeView)
        {
            _nodeId2NodeView.Remove(nodeId);
            _tableNodeViews.Remove(nodeView);
            Remove(nodeView);
        }
    }
}

