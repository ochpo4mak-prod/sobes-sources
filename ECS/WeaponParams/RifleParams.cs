using System;
using Unity.Entities;

namespace Assets.Scripts.ECS
{
    [Serializable]
    [GenerateAuthoringComponent]
    public struct RifleParams : IComponentData
    {
        public int Damage;
        public int ReloadTime;
    }
}
