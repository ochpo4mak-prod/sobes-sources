using System;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Assets.Scripts.Table
{
    public interface ITableNodesCollection : INode, IReadOnlyCollection<ITableNode>
    {
        ITableNode[] Nodes { get; }

        event Action<ITableNode> AddedEvent;
        event Action<ITableNode> RemovedEvent;

        ValueTask AddNode(INode node, RectInt rect, CancellationToken cancellationToken);
    }
}
