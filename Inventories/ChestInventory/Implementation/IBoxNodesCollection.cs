using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

public interface IBoxNodesCollection : IReadOnlyCollection<IBox>
{
    IBox[] boxes { get; }

    event Action<IBox> AddedEvent;
    event Action<ListNode> RemovedEvent;

    ValueTask AddNode(IBox node, CancellationToken cancellationToken);
    ValueTask RemoveNode(ListNode node);
}