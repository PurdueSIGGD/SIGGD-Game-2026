using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// Reuses prefab instances instead of destroying and re-instantiating them. Call <c>Get</c> where you would
/// call Instantiate, and <c>Release</c> where you would call Destroy.
/// </summary>
public static class PrefabPool
{
    private const int DefaultCapacity = 16;
    private const int MaxPoolSize = 128;

    private static readonly Dictionary<GameObject, ObjectPool<GameObject>> PoolsByPrefab =
    new Dictionary<GameObject, ObjectPool<GameObject>>();

    private static readonly Dictionary<GameObject, ObjectPool<GameObject>> PoolsByInstance =
    new Dictionary<GameObject, ObjectPool<GameObject>>();

    private static Transform root;

    /// <summary>
    /// Returns an active instance of <paramref name="prefab"/> at the given pose, reusing a released one when
    /// available. The instance still holds its state from its previous use.
    /// </summary>
    public static T Get<T>(T prefab, Vector3 position, Quaternion rotation) where T : Component
    {
        EnsureRoot();

        if (!PoolsByPrefab.TryGetValue(prefab.gameObject, out ObjectPool<GameObject> pool))
        {
            pool = CreatePool(prefab.gameObject);
            PoolsByPrefab.Add(prefab.gameObject, pool);
        }

        GameObject instance = pool.Get();
        instance.transform.SetPositionAndRotation(position, rotation);
        instance.SetActive(true);
        return instance.GetComponent<T>();
    }

    /// <summary>
    /// Deactivates the instance and keeps it for reuse. Objects that did not come from <c>Get</c> are destroyed.
    /// Releasing the same instance twice throws in the editor.
    /// </summary>
    public static void Release(GameObject instance)
    {
        if (PoolsByInstance.TryGetValue(instance, out ObjectPool<GameObject> pool))
        {
            pool.Release(instance);
        }
        else
        {
            Object.Destroy(instance);
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatics()
    {
        PoolsByPrefab.Clear();
        PoolsByInstance.Clear();
        root = null;
    }

    private static void EnsureRoot()
    {
        if (root != null)
        {
            return;
        }

        PoolsByPrefab.Clear();
        PoolsByInstance.Clear();
        root = new GameObject("PrefabPoolRoot").transform;
    }

    private static ObjectPool<GameObject> CreatePool(GameObject prefab)
    {
        ObjectPool<GameObject> pool = null;
        pool = new ObjectPool<GameObject>(
            createFunc: () =>
            {
                GameObject instance = Object.Instantiate(prefab, root);
                instance.SetActive(false);
                PoolsByInstance.Add(instance, pool);
                return instance;
            },

            actionOnRelease: instance => instance.SetActive(false),
            actionOnDestroy: instance =>
            {
                PoolsByInstance.Remove(instance);
                Object.Destroy(instance);
            },
            defaultCapacity: DefaultCapacity,
            maxSize: MaxPoolSize);
        return pool;
    }
}
