using System;
using Assets.Scripts.Item;
using Unity.Entities;

namespace Assets.Scripts.ECS
{
    [Serializable]
    [GenerateAuthoringComponent]
    public sealed class ItemInfo : IComponentData
    {
        public ItemSpecification ItemSpecification;
    }
}
