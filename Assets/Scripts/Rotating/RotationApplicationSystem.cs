using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;

namespace DOTSTutorial.Rotating
{
    public class RotationApplicationSystem : JobComponentSystem
    {
        private BuildPhysicsWorld buildPhysicsWorld;
        private StepPhysicsWorld stepPhysicsWorld;

        protected override void OnCreate()
        {
            buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
            stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var applicationJob = new ApplicationJob
            {
                rotateApplierGroup = GetComponentDataFromEntity<RotateApplier>(),
                rotateGroup = GetComponentDataFromEntity<Rotate>()
            };

            return applicationJob.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, inputDeps);
        }

        [BurstCompile]
        private struct ApplicationJob : ITriggerEventsJob
        {
            [ReadOnly] public ComponentDataFromEntity<RotateApplier> rotateApplierGroup;
            public ComponentDataFromEntity<Rotate> rotateGroup;

            public void Execute(TriggerEvent triggerEvent)
            {
                if (rotateApplierGroup.HasComponent(triggerEvent.Entities.EntityA))
                {
                    if (rotateGroup.HasComponent(triggerEvent.Entities.EntityB))
                    {
                        Rotate rotate = rotateGroup[triggerEvent.Entities.EntityB];
                        rotate.radiansPerSecond = math.radians(90f);
                        rotateGroup[triggerEvent.Entities.EntityB] = rotate;
                    }
                }

                if (rotateApplierGroup.HasComponent(triggerEvent.Entities.EntityB))
                {
                    if (rotateGroup.HasComponent(triggerEvent.Entities.EntityA))
                    {
                        Rotate rotate = rotateGroup[triggerEvent.Entities.EntityA];
                        rotate.radiansPerSecond = math.radians(90f);
                        rotateGroup[triggerEvent.Entities.EntityA] = rotate;
                    }
                }
            }
        }
    }
}
