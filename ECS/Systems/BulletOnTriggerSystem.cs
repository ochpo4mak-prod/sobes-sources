using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Rendering;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(EndFramePhysicsSystem))]
public class BulletOnTriggerSystem : SystemBase
{
    private BuildPhysicsWorld buildPhysicsWorld;
    private StepPhysicsWorld stepPhysicsWorld;

    private EndFixedStepSimulationEntityCommandBufferSystem commandBufferSystem;

    protected override void OnCreate()
    {
        base.OnCreate();
        buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
        commandBufferSystem = World.GetOrCreateSystem<EndFixedStepSimulationEntityCommandBufferSystem>();
    }

    [BurstCompile]
    struct BulletOnTriggerSystemJob : ITriggerEventsJob
    {
        [ReadOnly] public ComponentDataFromEntity<PickupTag> allPickups;
        [ReadOnly] public ComponentDataFromEntity<BulletTag> allBullets;

        public EntityCommandBuffer entityCommandBuffer;

        public void Execute(TriggerEvent triggerEvent)
        {
            Entity entityA = triggerEvent.EntityA;
            Entity entityB = triggerEvent.EntityB;

            if (allBullets.HasComponent(entityA) && allPickups.HasComponent(entityB))
            {
                return;
            }
            else if (allBullets.HasComponent(entityB) && allPickups.HasComponent(entityA))
            {
                return;
            }

            if (allBullets.HasComponent(entityB))
            {
                entityCommandBuffer.DestroyEntity(entityB);
            }

            else if (allBullets.HasComponent(entityA))
            {
                entityCommandBuffer.DestroyEntity(entityA);
            }
        }
    }

    protected override void OnUpdate()
    {
        var ecb = commandBufferSystem.CreateCommandBuffer();

        Dependency = new BulletOnTriggerSystemJob
        {
            allPickups = GetComponentDataFromEntity<PickupTag>(true),
            allBullets = GetComponentDataFromEntity<BulletTag>(true),
            entityCommandBuffer = ecb,
        }.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, Dependency);

        commandBufferSystem.AddJobHandleForProducer(Dependency);
    }
}
