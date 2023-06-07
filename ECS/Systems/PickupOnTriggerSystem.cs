using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Rendering;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(EndFramePhysicsSystem))]
public class PickupOnTriggerSystem : SystemBase
{
    private BuildPhysicsWorld buildPhysicsWorld;
    private StepPhysicsWorld stepPhysicsWorld;

    private EndSimulationEntityCommandBufferSystem commandBufferSystem;

    protected override void OnCreate()
    {
        base.OnCreate();
        buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
        commandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    [BurstCompile]
    struct PickupOnTriggerSystemJob : ITriggerEventsJob
    {
        [ReadOnly] public ComponentDataFromEntity<PickupTag> allPickups;
        [ReadOnly] public ComponentDataFromEntity<PlayerTag> allPlayers;

        public EntityCommandBuffer entityCommandBuffer;

        public void Execute(TriggerEvent triggerEvent)
        {
            Entity entityA = triggerEvent.EntityA;
            Entity entityB = triggerEvent.EntityB;

            if (allPickups.HasComponent(entityA) && allPickups.HasComponent(entityB))
            {
                return;
            }

            if (allPickups.HasComponent(entityA) && allPlayers.HasComponent(entityB))
            {
                entityCommandBuffer.DestroyEntity(entityA);
            }
            else if (allPlayers.HasComponent(entityA) && allPickups.HasComponent(entityB))
            {
                entityCommandBuffer.DestroyEntity(entityB);
            }
        }
    }

    protected override void OnUpdate()
    {
        Dependency = new PickupOnTriggerSystemJob
        {
            allPickups = GetComponentDataFromEntity<PickupTag>(true),
            allPlayers = GetComponentDataFromEntity<PlayerTag>(true),
            entityCommandBuffer = commandBufferSystem.CreateCommandBuffer(),
        }.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, Dependency);

        Dependency.Complete();
    }
}
