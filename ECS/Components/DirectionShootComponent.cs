using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct DirectionShootComponentData : IComponentData
{
    [HideInInspector] public float3 lastMovement;
}