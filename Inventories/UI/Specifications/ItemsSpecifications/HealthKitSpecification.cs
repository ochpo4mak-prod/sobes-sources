using Assets.Scripts.ECS;
using UnityEngine;

namespace Assets.Scripts.Item
{
    [CreateAssetMenu(menuName="Item/HealthKitParams")]
    public sealed class HealthKitSpecification : ItemSpecification
    {
        public HealthKitParams HealthKitParams;
    }
}
