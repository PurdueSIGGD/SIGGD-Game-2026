using UnityEngine;

/// <summary>
/// Periodically spawns asteroids off-screen and aims them at random points so they drift into view.
/// </summary>
[DisallowMultipleComponent]
public class AsteroidSpawner : MonoBehaviour
{
    // Direction components smaller than this are treated as zero (moving parallel to an axis).
    private const float ParallelAxisEpsilon = 1e-6f;

    [Tooltip("Spawn timing, direction and fly-in settings.")]
    [SerializeField] private AsteroidSpawnerData asteroidSpawnerData;

    [Tooltip("Camera whose view defines what counts as on-screen. Required in every boundary mode.")]
    [SerializeField] private Camera mainCamera;

    [Tooltip("Area asteroids are aimed at when the boundary type is Collider.")]
    [SerializeField] private BoxCollider2D spawnBox;

    private Rect targetRect;
    private Rect offscreenRect;
    private float timeSinceSpawn;

    private void Start()
    {
        if (asteroidSpawnerData == null)
        {
            Debug.LogError("AsteroidSpawnerData is not assigned.", this);
            enabled = false;
            return;
        }

        if (asteroidSpawnerData.AsteroidPrefab == null)
        {
            Debug.LogError("AsteroidSpawnerData has no AsteroidPrefab assigned.", this);
            enabled = false;
            return;
        }

        if (mainCamera == null)
        {
            Debug.LogError("mainCamera is not assigned.", this);
            enabled = false;
            return;
        }

        Rect cameraRect = GetCameraRect();
        float margin = asteroidSpawnerData.OffscreenMargin;
        offscreenRect = new Rect(
            cameraRect.xMin - margin,
            cameraRect.yMin - margin,
            cameraRect.width + 2.0f * margin,
            cameraRect.height + 2.0f * margin);
        targetRect = GetTargetRect();
    }

    private void Update()
    {
        timeSinceSpawn += Time.deltaTime;
        if (timeSinceSpawn >= asteroidSpawnerData.Period)
        {
            SpawnAsteroid();
            timeSinceSpawn -= asteroidSpawnerData.Period;
        }
    }

    private void SpawnAsteroid()
    {
        Vector2 target = GetRandomTarget();
        Vector2 direction = GetRandomDirection();

        float distance = Mathf.Max(
            asteroidSpawnerData.MinTravelDistance,
            GetDistanceToLeaveRect(target, direction, offscreenRect));
        Vector2 spawnPosition = target - direction * distance;
        float launchSpeed = asteroidSpawnerData.ComputeLaunchSpeed(distance, Time.fixedDeltaTime);

        AsteroidController asteroid = Instantiate(
            asteroidSpawnerData.AsteroidPrefab,
            spawnPosition,
            Quaternion.identity);
        asteroid.Initialize(direction * launchSpeed, asteroidSpawnerData.LinearDamping);
    }

    /// <summary>
    /// Returns how far to travel from <paramref name="point"/> along the negative of
    /// <paramref name="direction"/> before leaving <paramref name="rect"/> for good, or 0 if that path misses it.
    /// </summary>
    private static float GetDistanceToLeaveRect(Vector2 point, Vector2 direction, Rect rect)
    {
        float dEnter = float.NegativeInfinity;
        float dExit = float.PositiveInfinity;

        for (int axis = 0; axis < 2; axis++)
        {
            float p = point[axis];
            float v = -direction[axis];
            float min = axis == 0 ? rect.xMin : rect.yMin;
            float max = axis == 0 ? rect.xMax : rect.yMax;

            if (Mathf.Abs(v) < ParallelAxisEpsilon)
            {
                if (p < min || p > max)
                {
                    return 0.0f;
                }

                continue;
            }

            float d1 = (min - p) / v;
            float d2 = (max - p) / v;
            dEnter = Mathf.Max(dEnter, Mathf.Min(d1, d2));
            dExit = Mathf.Min(dExit, Mathf.Max(d1, d2));
        }

        if (dEnter > dExit)
        {
            return 0.0f;
        }

        return Mathf.Max(0.0f, dExit);
    }

    private Vector2 GetRandomTarget()
    {
        return new Vector2(
            Mathf.Lerp(targetRect.xMin, targetRect.xMax, Random.value),
                           Mathf.Lerp(targetRect.yMin, targetRect.yMax, Random.value));
    }

    private Vector2 GetRandomDirection()
    {
        float minAngle = asteroidSpawnerData.CenterAngle - asteroidSpawnerData.Spread;
        float maxAngle = asteroidSpawnerData.CenterAngle + asteroidSpawnerData.Spread;
        float radians = Random.Range(minAngle, maxAngle) * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
    }

    private Rect GetCameraRect()
    {
        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, 0.0f));
        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1.0f, 1.0f, 0.0f));
        return new Rect(bottomLeft.x, bottomLeft.y, topRight.x - bottomLeft.x, topRight.y - bottomLeft.y);
    }

    private Rect GetTargetRect()
    {
        switch (asteroidSpawnerData.BoundaryType)
        {
            case AsteroidSpawnerData.SpawnBoundaryType.Camera:
                return GetCameraRect();

            case AsteroidSpawnerData.SpawnBoundaryType.Collider:
                if (spawnBox == null)
                {
                    Debug.LogError("spawnBox is not assigned. Aiming at the camera view instead.", this);
                    return GetCameraRect();
                }

                Bounds bounds = spawnBox.bounds;
                return new Rect(bounds.min.x, bounds.min.y, bounds.size.x, bounds.size.y);

            default:
                Debug.LogError("Unknown spawn boundary type. Aiming at the camera view instead.", this);
                return GetCameraRect();
        }
    }
}
