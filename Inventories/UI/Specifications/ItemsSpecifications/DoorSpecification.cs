using Assets.Scripts.ECS;
using UnityEngine;

namespace Assets.Scripts.Item
{
    [CreateAssetMenu(menuName="Item/DoorParams")]
    public sealed class DoorSpecification : ItemSpecification
    {
        public DoorParams DoorParams;
    }
}
