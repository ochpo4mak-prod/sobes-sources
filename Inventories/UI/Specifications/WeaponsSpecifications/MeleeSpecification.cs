using Assets.Scripts.ECS;
using UnityEngine;

namespace Assets.Scripts.Weapon
{
    [CreateAssetMenu(menuName="Weapon/MeleeParams")]
    public sealed class MeleeSpecification : WeaponSpecification
    {
        public MeleeParams MeleeParams;
    }
}
