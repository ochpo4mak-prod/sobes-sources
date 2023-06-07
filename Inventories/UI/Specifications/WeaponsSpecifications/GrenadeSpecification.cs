using Assets.Scripts.ECS;
using UnityEngine;

namespace Assets.Scripts.Weapon
{
    [CreateAssetMenu(menuName="Weapon/GrenadeParams")]
    public sealed class GrenadeSpecification : WeaponSpecification
    {
        public GrenadeParams GrenadeParams;
    }
}
