using Assets.Scripts.ECS;
using UnityEngine;

namespace Assets.Scripts.Item
{
    [CreateAssetMenu(menuName="Item/DestroyableParams")]
    public sealed class DestroyableSpecification : ItemSpecification
    {
        public DestroyableParams DestroyableParams;
    }
}
