using Unity.Entities;

public class DestroyBulletSystem : SystemBase
{
    private EndSimulationEntityCommandBufferSystem _endSimECommandBufferSystem;

    protected override void OnCreate()
    {
        _endSimECommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        var commandBuffer = _endSimECommandBufferSystem.CreateCommandBuffer().AsParallelWriter();
        var deltaTime = Time.DeltaTime;

        Entities
            .ForEach((Entity e, ref BulletComponentData bullet) =>
            {
                bullet.DestroyTime -= deltaTime;
                if(bullet.DestroyTime <= 0)
                    commandBuffer.DestroyEntity(0, e);
            }).Schedule();

        _endSimECommandBufferSystem.AddJobHandleForProducer(Dependency);
    }
}