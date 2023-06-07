using Unity.Physics;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public class DirectionShootSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities
            .ForEach((ref DirectionShootComponentData directionShootComponentData, in PlayerMoveComponentData playerMoveComponentData) => 
            {
                if (directionShootComponentData.lastMovement.Equals(float3.zero))
                {
                    directionShootComponentData.lastMovement = new float3(50, 0, 0);
                }

                if ((playerMoveComponentData.playerCoordinates.x != 0) || (playerMoveComponentData.playerCoordinates.z !=0))
                {
                    directionShootComponentData.lastMovement = playerMoveComponentData.playerCoordinates * 50;
                }
            })
            .Schedule();
    }
}