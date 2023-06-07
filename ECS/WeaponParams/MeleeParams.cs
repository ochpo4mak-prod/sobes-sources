using System;
using Unity.Entities;

namespace Assets.Scripts.ECS
{
    [Serializable]
    [GenerateAuthoringComponent]
    public struct MeleeParams : IComponentData
    {
        public int Damage;
        public int ImpactSpeed;
    }
}
