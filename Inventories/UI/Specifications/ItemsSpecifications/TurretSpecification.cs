using Assets.Scripts.ECS;
using UnityEngine;

namespace Assets.Scripts.Item
{
    [CreateAssetMenu(menuName="Item/TurretParams")]
    public sealed class TurretSpecification : ItemSpecification
    {
        public TurretParams TurretParams;
    }
}
