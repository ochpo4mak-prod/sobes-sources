using System;
using Unity.Entities;

[GenerateAuthoringComponent]
public struct PickUpComponentData : IComponentData
{
    public float RadiansPerSecond;
}
