using System;
using System.Threading;
using Unity.Mathematics;
using System.Threading.Tasks;

namespace Assets.Scripts.Table
{
    public interface ITableNode : INode
    {
        event Action<ENodeStatus> StatusChangedEvent;
        event Action<int2> PositionChangedEvent;

        int2 Position { get; }
        int2 Size { get; }
        ENodeStatus Status { get; }
        INode Node { get; }

        public void ChangePosition(int2 position);
        public void ChangeStatus(ENodeStatus status);
        public ValueTask Destroy(CancellationToken cancellationToken);
        public ValueTask Move(ITableNodesCollection to, int2 position, CancellationToken cancellationToken);
    }

    public enum ENodeStatus
    {
        None,
        Locked
    }
}
