using System;
using Unity.Entities;

namespace Assets.Scripts.ECS
{
    [Serializable]
    [GenerateAuthoringComponent]
    public struct DamageTrapParams : IComponentData
    {
        public int Damage;
        public int BPM;
    }
}
