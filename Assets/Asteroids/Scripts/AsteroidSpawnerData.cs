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
    private const float MinFlyInTime = 0.1f;

    // Below this decay rate (per second) damping is treated as zero
    private const float NegligibleDecayRate = 1e-5f;

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

    [field: SerializeField, Tooltip("Seconds an asteroid takes to reach its target point.")]
    public float TimeToFlyIn { get; private set; } = 2.0f;

    [field: SerializeField, Tooltip("Rigidbody2D linear damping applied to every spawned asteroid for its whole life.")]
    public float LinearDamping { get; private set; } = 1.0f;

    /// <summary>
    /// Returns the launch speed that makes a damped asteroid cover <paramref name="distance"/> in exactly
    /// <see cref="TimeToFlyIn"/> seconds.
    /// </summary>
    /// <param name="distance">Distance from spawn point to target point, in world units.</param>
    /// <param name="fixedDeltaTime">Physics step length, in seconds.</param>
    public float ComputeLaunchSpeed(float distance, float fixedDeltaTime)
    {
        // Linear damping gives exponential decay: distance(t) = (v0 / k) * (1 - e^(-k * t)). Solving for v0 gives
        // the line below. Unity applies damping as v /= (1 + damping * dt) each physics step, which equals
        // e^(-k * dt) with k = ln(1 + damping * dt) / dt, so that k is used to match the engine.
        float decayRate = Mathf.Log(1.0f + LinearDamping * fixedDeltaTime) / fixedDeltaTime;
        if (decayRate < NegligibleDecayRate)
        {
            return distance / TimeToFlyIn;
        }

        return distance * decayRate / (1.0f - Mathf.Exp(-decayRate * TimeToFlyIn));
    }

    private void OnValidate()
    {
        Period = Mathf.Max(MinPeriod, Period);
        TimeToFlyIn = Mathf.Max(MinFlyInTime, TimeToFlyIn);
        MinTravelDistance = Mathf.Max(0.0f, MinTravelDistance);
        OffscreenMargin = Mathf.Max(0.0f, OffscreenMargin);
        LinearDamping = Mathf.Max(0.0f, LinearDamping);

        if (AsteroidPrefab == null)
        {
            Debug.LogWarning($"{name}: AsteroidPrefab is empty, so the spawner has nothing to spawn.", this);
        }
    }
}
