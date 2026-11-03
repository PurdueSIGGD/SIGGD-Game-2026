using UnityEngine;

/// <summary>
/// Settings for an <see cref="AsteroidSpawner"/>: what to spawn, how often, and how it flies in.
/// </summary>
[CreateAssetMenu(fileName = "AsteroidSpawnerData", menuName = "Scriptable Objects/AsteroidSpawnerData")]
public class AsteroidSpawnerData : ScriptableObject
{
    /// <summary>Which area asteroids are aimed at.</summary>
    public enum SpawnBoundaryType
    {
        /// <summary>The camera's visible area.</summary>
        Camera,

        /// <summary>The bounds of the spawner's BoxCollider2D.</summary>
        Collider
    }

    private const float MinPeriod = 0.05f;

    [field: SerializeField, Tooltip("Asteroid prefab to spawn.")]
    public AsteroidController AsteroidPrefab { get; private set; }

    [field: SerializeField, Tooltip("Seconds between spawns.")]
    public float Period { get; private set; } = 2.0f;

    [field: SerializeField, Tooltip("Area that asteroid target points are picked from.")]
    public SpawnBoundaryType BoundaryType { get; private set; } = SpawnBoundaryType.Camera;

    [field: SerializeField, Tooltip("Direction of travel in degrees. 0 is right, 90 is up, 270 is down.")]
    public float CenterAngle { get; private set; } = 270.0f;

    [field: SerializeField, Range(0.0f, 180.0f), Tooltip("Random +/- degrees added to the direction of travel.")]
    public float Spread { get; private set; } = 0.0f;

    [field: SerializeField, Tooltip("Shortest allowed distance, in world units, from spawn point to target point.")]
    public float MinTravelDistance { get; private set; } = 2.0f;

    [field: SerializeField, Tooltip("Padding past the screen edge, in world units. Use at least the asteroid radius.")]
    public float OffscreenMargin { get; private set; } = 1.0f;

    [field: SerializeField, Tooltip("Shrinks the target area by this many world units on every side, "
    + "so asteroids don't arrive at the screen edge.")]
    public float TargetInset { get; private set; } = 0.0f;

    [field: SerializeField, Tooltip("Rigidbody2D linear damping applied to every spawned asteroid for its whole life. Controls how quickly asteroids settle.")]
    public float LinearDamping { get; private set; } = 1.0f;

    /// <summary>
    /// Returns the launch speed that makes a damped asteroid cover <paramref name="distance"/> in exactly
    /// <see cref="TimeToFlyIn"/> seconds.
    /// </summary>
    /// <param name="distance">Distance from spawn point to target point, in world units.</param>
    /// <param name="fixedDeltaTime">Physics step length, in seconds.</param>
    public float ComputeLaunchSpeed(float distance, float fixedDeltaTime)
    {
        float decayRate = Mathf.Log(1.0f + LinearDamping * fixedDeltaTime) / fixedDeltaTime;
        return distance * decayRate;
    }

    private void OnValidate()
    {
        Period = Mathf.Max(MinPeriod, Period);
        MinTravelDistance = Mathf.Max(0.0f, MinTravelDistance);
        OffscreenMargin = Mathf.Max(0.0f, OffscreenMargin);
        TargetInset = Mathf.Max(0.0f, TargetInset);
        LinearDamping = Mathf.Max(0.05f, LinearDamping);

        if (AsteroidPrefab == null)
        {
            Debug.LogWarning($"{name}: AsteroidPrefab is empty, so the spawner has nothing to spawn.", this);
        }
    }
}
