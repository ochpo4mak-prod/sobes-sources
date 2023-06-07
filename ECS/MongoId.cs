using System;
using MongoDB.Bson;
using Sirenix.OdinInspector;
using Unity.Entities;

namespace Assets.Scripts.ECS
{
    [Serializable]
    [GenerateAuthoringComponent]
    public struct MongoId : IComponentData
    {
        public ObjectId ObjectId;

        [ShowInInspector]
        public string Value
        {
            get => ObjectId.ToString();
            set => ObjectId = ObjectId.Parse(value);
        }
    }
}