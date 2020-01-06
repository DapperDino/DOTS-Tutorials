using Unity.Entities;

namespace DOTSTutorial.Spawning
{
    public struct Spawner : IComponentData
    {
        public Entity prefab;
        public float maxDistanceFromSpawner;
        public float secondsBetweenSpawns;
        public float secondsToNextSpawn;
    }
}
