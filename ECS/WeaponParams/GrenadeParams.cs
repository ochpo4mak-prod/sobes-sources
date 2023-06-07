using System;
using Unity.Entities;

namespace Assets.Scripts.ECS
{
    [Serializable]
    [GenerateAuthoringComponent]
    public struct GrenadeParams : IComponentData
    {
        public int Damage;
        public int Radius;
    }
}
