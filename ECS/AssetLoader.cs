using System;
using System.Linq;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;

public class AssetLoader : MonoBehaviour
{
    private BlobAssetStore _blobAssetStore;
    private EntityManager _entityManager;
    private List<Entity> _assetEntities;

    [SerializeField] private List<SpawnInfo> _spawnList;
    [SerializeField] private AssetReference _bulletAsset;
    public static Entity bulletEntity;

    private async Task Start()
    {
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        _blobAssetStore = new BlobAssetStore();
        _assetEntities = new List<Entity>();

        var bulletPrefab = await Addressables.LoadAssetAsync<GameObject>(_bulletAsset).Task;
        var bulletEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(bulletPrefab,
            GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, _blobAssetStore));

        bulletEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(bulletPrefab,
            GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, _blobAssetStore));

        var tasks = _spawnList.Select(l => l.asset).Select(asset => Addressables.LoadAssetAsync<GameObject>(asset).Task).ToArray();
        var assetPrefabs = (await Task.WhenAll(tasks)).ToList();

        for (var i = 0; i < _spawnList.Count; ++i)
        {
            var assetPrefab = assetPrefabs[i];

            var assetEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(assetPrefab,
                GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, _blobAssetStore));

            _entityManager.SetComponentData(assetEntity, new Translation
            {
                Value = new float3(_spawnList[i].position)
            });

            _entityManager.SetName(assetEntity, assetPrefab.name);

            _assetEntities.Add(assetEntity);
        }

        foreach (var entityPrefab in _assetEntities)
            _entityManager.Instantiate(entityPrefab);
    }

    private void OnDestroy()
    {
        _blobAssetStore.Dispose();
    }
}

[Serializable]
struct SpawnInfo
{
    public AssetReference asset;
    public float3 position;
    public quaternion rotation;
}