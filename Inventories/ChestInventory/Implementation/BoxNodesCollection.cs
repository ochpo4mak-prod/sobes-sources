using System;
using System.Threading;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;

public class BoxNodesCollection : IBoxNodesCollection
{
    private List<IBox> _boxNodes = new List<IBox>();

    public IBox[] boxes => _boxNodes.ToArray();

    public int Count => _boxNodes.Count;

    public event Action<IBox> AddedEvent;
    public event Action<ListNode> RemovedEvent;

    public ValueTask AddNode(IBox node, CancellationToken cancellationToken)
    {
        _boxNodes.Add(node);
        AddedEvent?.Invoke(node);
        return new ValueTask(Task.CompletedTask);
    }

    public ValueTask RemoveNode(ListNode node)
    {
        _boxNodes.Remove(node.Node);
        RemovedEvent?.Invoke(node);
        return new ValueTask(Task.CompletedTask);
    }

    public IEnumerator<IBox> GetEnumerator()
    {
        return _boxNodes.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}