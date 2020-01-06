using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace DOTSTutorial.Spawning
{
    public class SpawnerAuthoring : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private float spawnRate;
        [SerializeField] private float maxDistanceFromSpawner;

        public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
        {
            referencedPrefabs.Add(prefab);
        }

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new Spawner
            {
                prefab = conversionSystem.GetPrimaryEntity(prefab),
                maxDistanceFromSpawner = maxDistanceFromSpawner,
                secondsBetweenSpawns = 1 / spawnRate,
                secondsToNextSpawn = 0f
            });
        }
    }
}
