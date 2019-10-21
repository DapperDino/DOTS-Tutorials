using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Time = UnityEngine.Time;

namespace DOTSTutorial.Spawning
{
    public class SpawnerSystem : JobComponentSystem
    {
        private EndSimulationEntityCommandBufferSystem endSimulationEntityCommandBufferSystem;

        protected override void OnCreate()
        {
            endSimulationEntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        private struct SpawnerJob : IJobForEachWithEntity<Spawner, LocalToWorld>
        {
            private EntityCommandBuffer.Concurrent entityCommandBuffer;
            private Random random;
            private readonly float deltaTime;

            public SpawnerJob(EntityCommandBuffer.Concurrent entityCommandBuffer, Random random, float deltaTime)
            {
                this.entityCommandBuffer = entityCommandBuffer;
                this.random = random;
                this.deltaTime = deltaTime;
            }

            public void Execute(Entity entity, int index, ref Spawner spawner, [ReadOnly] ref LocalToWorld localToWorld)
            {
                spawner.secondsToNextSpawn -= deltaTime;

                if (spawner.secondsToNextSpawn >= 0) { return; }

                spawner.secondsToNextSpawn += spawner.secondsBetweenSpawns;

                Entity instance = entityCommandBuffer.Instantiate(index, spawner.prefab);
                entityCommandBuffer.SetComponent(index, instance, new Translation
                {
                    Value = localToWorld.Position + random.NextFloat3Direction() * random.NextFloat() * spawner.maxDistanceFromSpawner
                });
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var spawnerJob = new SpawnerJob(
                endSimulationEntityCommandBufferSystem.CreateCommandBuffer().ToConcurrent(),
                new Random((uint)UnityEngine.Random.Range(0, int.MaxValue)),
                Time.deltaTime
            );

            JobHandle jobHandle = spawnerJob.Schedule(this, inputDeps);

            endSimulationEntityCommandBufferSystem.AddJobHandleForProducer(jobHandle);

            return jobHandle;
        }
    }
}
