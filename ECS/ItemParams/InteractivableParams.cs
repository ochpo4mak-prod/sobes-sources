using System;
using Unity.Entities;

namespace Assets.Scripts.ECS
{
    [Serializable]
    [GenerateAuthoringComponent]
    public struct InteractivableParams : IComponentData
    {
        public bool NeedKey;  // Question about bool in ECS
        public int UseTime;
    }
}
