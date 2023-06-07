using System;
using Unity.Entities;

namespace Assets.Scripts.ECS
{
    [Serializable]
    [GenerateAuthoringComponent]
    public struct HealthKitParams : IComponentData
    {
        public int HealValue;
    }
}
