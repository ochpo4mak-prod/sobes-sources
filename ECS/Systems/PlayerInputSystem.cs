using UnityEngine;
using Unity.Entities;

public sealed class PlayerInputSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var vertical = Input.GetAxisRaw("Vertical");
        var horizontal = Input.GetAxisRaw("Horizontal");
        var leftMouseButton = Input.GetMouseButtonDown(0);

        Entities.ForEach((ref PlayerMoveComponentData playerMoveComponentData) => 
        {
            playerMoveComponentData.playerCoordinates.z = vertical;
            playerMoveComponentData.playerCoordinates.x = horizontal;
            playerMoveComponentData.leftMouseButton = leftMouseButton;
        }).Run();
    }
}
