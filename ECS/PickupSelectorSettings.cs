using System;
using Unity.Entities;
using Unity.Mathematics;

namespace Assets.Scripts.ECS
{
    [Serializable]
    [GenerateAuthoringComponent]
    public struct PickupSelectorSettings : IComponentData
    {
        public Entity PickupSelector;
        public float Speed;
        public float3 Direction;
    }
}
