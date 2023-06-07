using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public class PlayerMovementSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var deltaTime = Time.DeltaTime;

        Entities
            .ForEach((ref Translation pos, in PlayerMoveComponentData playerMoveComponentData) => 
            {
                pos.Value += playerMoveComponentData.playerCoordinates * playerMoveComponentData.moveSpeed * deltaTime;
            })
            .Schedule();
    }
}