using System;
using Unity.Entities;

[GenerateAuthoringComponent]
public struct BulletComponentData : IComponentData
{
    public float DestroyTime;
}