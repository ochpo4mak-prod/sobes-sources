using Assets.Scripts.ECS;
using UnityEngine;

namespace Assets.Scripts.Weapon
{
    [CreateAssetMenu(menuName="Weapon/PistolParams")]
    public sealed class PistolSpecification : WeaponSpecification
    {
        public PistolParams PistolParams;
    }
}
