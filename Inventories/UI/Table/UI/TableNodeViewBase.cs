using System;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.UIElements;

namespace Assets.Scripts.Table.UI
{
    public abstract class TableNodeViewBase : VisualElement, IDisposable
    {
        private string _previousStatusClass;
        private int _pixelSize;

        public TableNodeViewBase(ITableNode tableNode, int pixelSize)
        {
            _pixelSize = pixelSize;
            Node = tableNode;

            Position = tableNode.Position * pixelSize;
            Size = tableNode.Size * pixelSize;
            // Rect = new RectInt(tableNode.Rect.position * pixelSize, 
            //     tableNode.Rect.size * pixelSize);

            AddToClassList("table-node");

            //TODO : Changes uss properties from c# (it's bad)
            style.left = Position.x;
            style.top = Position.y;
            style.width = Size.x;
            style.height = Size.y;

            Node.StatusChangedEvent += OnStatusChanged;
            Node.PositionChangedEvent += OnPositionChanged;

            RegisterCallback<MouseEnterEvent>(OnMouseEnterEvent);
        }

        private void OnMouseEnterEvent(MouseEnterEvent mouseEvent)
        {
            if (DragAndDrop.IsGrabbing)
                Debug.Log($"RESULT: {DragAndDrop.IsValidDataType(this)}");
        }

        public void Dispose()
        {
            Node.StatusChangedEvent -= OnStatusChanged;
            Node.PositionChangedEvent -= OnPositionChanged;
        }

        public ITableNode Node { get; }
        // public RectInt Rect { get; }
        public int2 Position { get; private set; }
        public int2 Size { get; }

        private void OnPositionChanged(int2 position)
        {
            //TODO : Changes uss properties from c# (it's bad)
            style.left = position.x * _pixelSize;
            style.top = position.y* _pixelSize;
        }

        private void OnStatusChanged(ENodeStatus status)
        {
            if (!string.IsNullOrEmpty(_previousStatusClass))
                RemoveFromClassList(_previousStatusClass);

            var statusClassName = status.ToString().ToLower();
            _previousStatusClass = statusClassName;

            AddToClassList(statusClassName);
        }
    }
}

