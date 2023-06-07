using System;
using Assets.Scripts.Weapon;
using Unity.Entities;

namespace Assets.Scripts.ECS
{
    [Serializable]
    [GenerateAuthoringComponent]
    public sealed class WeaponPickupInfo : IComponentData
    {
        public WeaponSpecification WeaponSpecification;
        public Entity WeaponModel;
    }
}
