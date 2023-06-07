using System;
using Unity.Entities;

namespace Assets.Scripts.ECS
{
    [Serializable]
    [GenerateAuthoringComponent]
    public struct ExplosiveTrapParams : IComponentData
    {
        public int Damage;
        public int Radius;
    }
}
