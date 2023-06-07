using System;
using UnityEngine;
using System.Threading;
using Unity.Mathematics;
using System.Threading.Tasks;

namespace Assets.Scripts.Table
{
    public partial class TableNodesCollection
    {
        private class Handle : ITableNode
        {
            private TableNodesCollection _nodesCollection;

            public static void Create(TableNodesCollection collection, RectInt rect, INode node = null)
            {
                var handle = new Handle(collection, rect, node);
                collection._tableNodes.Add(handle);
                collection.AddedEvent?.Invoke(handle);
            }

            private Handle(TableNodesCollection collection, RectInt rect, INode node)
            {
                Node = node;
                Position = new int2(rect.x, rect.y);
                Size = new int2(rect.width, rect.height);
                Id = Guid.NewGuid().ToString();
                _nodesCollection = collection;
            }

            public string Id { get; }
            public int2 Position { get; private set; }
            public int2 Size { get; }
            public ENodeStatus Status { get; private set; }
            public INode Node { get; }

            public event Action<ENodeStatus> StatusChangedEvent;
            public event Action<int2> PositionChangedEvent;

            public void ChangePosition(int2 position)
            {
                Position = position;
                PositionChangedEvent?.Invoke(Position);
            }

            public void ChangeStatus(ENodeStatus status)
            {
                Status = status;
                StatusChangedEvent?.Invoke(status);
            }

            public ValueTask Destroy(CancellationToken cancellationToken)
            {
                _nodesCollection._tableNodes.Remove(this);
                _nodesCollection.RemovedEvent?.Invoke(this);
                return new ValueTask(Task.CompletedTask);
            }

            public ValueTask Move(ITableNodesCollection to, int2 position, CancellationToken cancellationToken)
            {
                if (to is TableNodesCollection tableNodesCollection)
                {
                    this.Destroy(cancellationToken);

                    _nodesCollection = tableNodesCollection;
                    tableNodesCollection._tableNodes.Add(this);
                    tableNodesCollection.AddedEvent?.Invoke(this);

                    this.ChangePosition(position);
                }

                return new ValueTask(Task.CompletedTask);
            }
        }
    }
}
