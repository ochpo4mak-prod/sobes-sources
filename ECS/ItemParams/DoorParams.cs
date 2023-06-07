using System;
using Unity.Entities;

namespace Assets.Scripts.ECS
{
    [Serializable]
    [GenerateAuthoringComponent]
    public struct DoorParams : IComponentData
    {
        public bool isClose;  // Question about bool in ECS
        public int Strength;
    }
}
