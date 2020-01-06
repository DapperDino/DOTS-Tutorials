using Unity.Entities;

namespace DOTSTutorial.Rotating
{
    public struct Rotate : IComponentData
    {
        public float radiansPerSecond;
    }
}
