using System;
using UnityEngine;
using System.Threading;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Assets.Scripts.Table
{
    public partial class TableNodesCollection : Node, ITableNodesCollection
    {
        private List<ITableNode> _tableNodes = new List<ITableNode>();

        public TableNodesCollection(ENodeContent content)
            : base(content)
        {
        }

        public int Count => _tableNodes.Count;
        public ITableNode[] Nodes => _tableNodes.ToArray();

        public event Action<ITableNode> AddedEvent;
        public event Action<ITableNode> RemovedEvent;

        public ValueTask AddNode(INode node, RectInt rect, CancellationToken cancellationToken)
        {
            Handle.Create(this, rect, node);
            return new ValueTask(Task.CompletedTask);
        }

        public IEnumerator<ITableNode> GetEnumerator()
        {
            return _tableNodes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
