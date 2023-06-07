using Assets.Scripts.ECS;
using UnityEngine;

namespace Assets.Scripts.Item
{
    public abstract class ItemSpecification : ScriptableObject
    {
        public MongoId MongoId;
    }
}
