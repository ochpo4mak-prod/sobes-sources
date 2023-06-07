using System;
using Unity.Entities;

namespace Assets.Scripts.ECS
{
    [Serializable]
    [GenerateAuthoringComponent]
    public struct DestroyableParams : IComponentData
    {
        public bool isExplosive;  // Question about bool in ECS
        public int Strength;
    }
}
