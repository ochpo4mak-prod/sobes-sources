using Assets.Scripts.ECS;
using UnityEngine;

namespace Assets.Scripts.Item
{
    [CreateAssetMenu(menuName="Item/InteractivableParams")]
    public sealed class InteractivableSpecification : ItemSpecification
    {
        public InteractivableParams InteractivableParams;
    }
}
