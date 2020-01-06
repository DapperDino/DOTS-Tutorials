using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

namespace DOTSTutorial.Rotating
{
    public class RotateSystem : JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            float deltaTime = Time.DeltaTime;

            var jobHandle = Entities.ForEach((ref RotationEulerXYZ euler, in Rotate rotate) =>
            {
                euler.Value.y += rotate.radiansPerSecond * deltaTime;
            }).Schedule(inputDeps);

            return jobHandle;
        }
    }
}
