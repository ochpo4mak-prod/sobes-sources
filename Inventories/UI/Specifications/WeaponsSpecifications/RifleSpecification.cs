using Assets.Scripts.ECS;
using UnityEngine;

namespace Assets.Scripts.Weapon
{
    [CreateAssetMenu(menuName="Weapon/RifleParams")]
    public sealed class RifleSpecification : WeaponSpecification
    {
        public RifleParams RifleParams;
    }
}
