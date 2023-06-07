using Assets.Scripts.ECS;
using UnityEngine;

namespace Assets.Scripts.Trap
{
    [CreateAssetMenu(menuName="Trap/DamageTrapParams")]
    public sealed class DamageTrapSpecification : TrapSpecification
    {
        public DamageTrapParams DamageTrapParams;
    }
}
