using UnityEngine;

/// <summary>
/// Returns asteroids to the pool when they leave this trigger area.
/// </summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(Collider2D))]
public class AsteroidDespawnZone : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.TryGetComponent(out AsteroidController asteroid))
        {
            return;
        }

        if (asteroid.gameObject.activeInHierarchy)
        {
            PrefabPool.Release(asteroid.gameObject);
        }
    }

    private void OnValidate()
    {
        Collider2D zone = GetComponent<Collider2D>();
        if (zone != null && !zone.isTrigger)
        {
            Debug.LogWarning("The collider must have Is Trigger turned on, or asteroids will bounce off it.", this);
        }
    }
}
