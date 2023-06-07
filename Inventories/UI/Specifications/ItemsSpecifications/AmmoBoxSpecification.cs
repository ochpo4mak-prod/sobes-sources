using Assets.Scripts.ECS;
using UnityEngine;

namespace Assets.Scripts.Item
{
    [CreateAssetMenu(menuName="Item/AmmoBoxParams")]
    public sealed class AmmoBoxSpecification : ItemSpecification
    {
        public AmmoBoxParams AmmoBoxParams;
    }
}
