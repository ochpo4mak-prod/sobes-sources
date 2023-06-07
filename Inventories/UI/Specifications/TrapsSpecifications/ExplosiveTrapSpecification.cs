using Assets.Scripts.ECS;
using UnityEngine;

namespace Assets.Scripts.Trap
{
    [CreateAssetMenu(menuName="Trap/ExplosiveTrapParams")]
    public sealed class ExplosiveTrapSpecification : TrapSpecification
    {
        public ExplosiveTrapParams ExplosiveTrapParams;
    }
}
