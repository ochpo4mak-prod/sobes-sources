using Unity.Physics;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public class SpawnBulletSystem : SystemBase
{
    private BeginSimulationEntityCommandBufferSystem _beginSimECommandBufferSystem;

    protected override void OnCreate()
    {
        _beginSimECommandBufferSystem = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        var commandBuffer = _beginSimECommandBufferSystem.CreateCommandBuffer().AsParallelWriter();

        Entities
            .ForEach((ref Translation pos, in PlayerMoveComponentData playerMoveComponentData, in DirectionShootComponentData directionShootComponentData) => 
            {
                if (playerMoveComponentData.leftMouseButton)
                {
                    var bullet = commandBuffer.Instantiate(0, AssetLoader.bulletEntity);

                    commandBuffer.SetComponent(0, bullet, new Translation
                    {
                        Value = pos.Value + new float3(0, 1f, 0)
                    });

                    var velocity = new PhysicsVelocity { Linear = directionShootComponentData.lastMovement };
                    commandBuffer.SetComponent(0, bullet, velocity);
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Shoot");
                }
            })
            .Schedule();
    }
}