using System;
using Unity.Entities;

namespace Assets.Scripts.ECS
{
    [Serializable]
    [GenerateAuthoringComponent]
    public struct TurretParams : IComponentData
    {
        public int Damage;
        public int Strength;
    }
}
