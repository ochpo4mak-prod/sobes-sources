using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct PlayerMoveComponentData : IComponentData
{
    [HideInInspector] public float3 playerCoordinates;
    [HideInInspector] public bool leftMouseButton;
    public float moveSpeed;
}