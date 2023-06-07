using System;
using Assets.Scripts.Trap;
using Unity.Entities;

namespace Assets.Scripts.ECS
{
    [Serializable]
    [GenerateAuthoringComponent]
    public sealed class TrapInfo : IComponentData
    {
        public TrapSpecification TrapSpecification;
    }
}
