using System;
using Unity.Entities;

namespace Assets.Scripts.ECS
{
    [Serializable]
    [GenerateAuthoringComponent]
    public struct AmmoBoxParams : IComponentData
    {
        public int BulletsCount;
    }
}
