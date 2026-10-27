using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A destructible asteroid. It takes damage from fast collisions and, when its health runs out, spawns an
/// explosion and fragments that carry on the parent's momentum.
/// </summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class AsteroidController : MonoBehaviour, IDamageable
{
    // Reused so the collision-ignore coroutine doesn't allocate a new object every physics step.
    private static readonly WaitForFixedUpdate WaitForPhysicsStep = new WaitForFixedUpdate();

    [Tooltip("Stats, sprite and destruction settings. Fragments swap in their own data when they are created.")]
    [SerializeField] private AsteroidData asteroidData;

    [Tooltip("Collisions at or below this relative speed (units per second) deal no damage.")]
    [SerializeField] private float speedDamageThreshold = 5.0f;

    [Tooltip("Damage dealt per unit of relative impact speed.")]
    [SerializeField] private float damageMultiplier = 10.0f;

    [Tooltip("Gap, in world units, two new fragments need before they can collide with each other again. "
    + "Too small and fragments flying apart damage each other.")]
    [SerializeField] private float siblingClearance = 0.05f;

    private Rigidbody2D asteroidRigidbody;
    private SpriteRenderer spriteRenderer;
    private Collider2D asteroidCollider;

    private float mass;
    private float size;
    private bool isInitialized;
    private bool isDead;

    private readonly List<Collider2D> ignoredColliders = new List<Collider2D>();
    private AsteroidData defaultAsteroidData;
    private Sprite defaultSprite;

    /// <summary>Remaining health. The asteroid is destroyed when this reaches zero.</summary>
    public float CurrentHealth { get; private set; }

    /// <summary>Starting health from the asteroid's data, or 0 if no data is assigned.</summary>
    public float MaxHealth => asteroidData != null ? asteroidData.BaseMaxHealth : 0.0f;

    /// <summary>
    /// Raised once as this asteroid is destroyed, after its explosion and fragments have been spawned.
    /// Subscribers must unsubscribe (usually in OnDisable).
    /// </summary>
    public event System.Action<AsteroidController> Died;

    private void Awake()
    {
        asteroidRigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        asteroidCollider = GetComponent<Collider2D>();
        defaultAsteroidData = asteroidData;
        defaultSprite = spriteRenderer.sprite;
    }

    private void Start()
    {
        if (!isInitialized)
        {
            Initialize(Vector2.zero, asteroidRigidbody.linearDamping);
        }
    }

    private void Update()
    {
        if (!isInitialized || isDead)
        {
            return;
        }

        if (CurrentHealth <= 0.0f)
        {
            DestroyAsteroid();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float impactSpeed = collision.relativeVelocity.magnitude;

        if (impactSpeed > speedDamageThreshold)
        {
            TakeDamage(new DamageContext
            {
                DamageAmount = impactSpeed * damageMultiplier,
                Attacker = collision.gameObject,
                Victim = gameObject
            });
        }
    }

    private void OnDisable()
    {
        foreach (Collider2D other in ignoredColliders)
        {
            if (asteroidCollider != null && other != null)
            {
                Physics2D.IgnoreCollision(asteroidCollider, other, false);
            }
        }

        ignoredColliders.Clear();
        Died = null;
    }

    private void OnValidate()
    {
        Rigidbody2D body = GetComponent<Rigidbody2D>();
        if (body != null && body.useAutoMass)
        {
            Debug.LogWarning("Use Auto Mass is on, so the mass set by Initialize is ignored and fragments "
            + "will not conserve momentum. Turn it off on the Rigidbody2D.", this);
        }

        if (GetComponent<Collider2D>() == null)
        {
            Debug.LogWarning("No Collider2D found. Asteroids need one to collide and take damage.", this);
        }
    }

    /// <summary>
    /// Sets the asteroid up and gives it its starting motion. Call this right after instantiating
    /// call before Start. Calling this again resets the asteroid.
    /// </summary>
    /// <param name="velocity">Starting linear velocity in world units per second.</param>
    /// <param name="linearDamping">Rigidbody2D linear damping to use for the asteroid's whole life.</param>
    /// <param name="dataOverride">Data to use instead of the prefab's own. Fragments pass this.</param>
    /// <param name="massOverride">Exact mass. Rolled from the data when null.</param>
    /// <param name="sizeOverride">Exact uniform scale. Rolled from the data when null.</param>
    /// <param name="angularVelocity">Starting spin in degrees per second. Rolled from the data when null.</param>
    public void Initialize(
        Vector2 velocity,
        float linearDamping,
        AsteroidData dataOverride = null,
        float? massOverride = null,
        float? sizeOverride = null,
        float? angularVelocity = null)
    {
        asteroidData = dataOverride != null ? dataOverride : defaultAsteroidData;

        if (asteroidData == null)
        {
            Debug.LogError("No AsteroidData assigned, so the asteroid cannot be initialized.", this);
            return;
        }

        Sprite overrideSprite = dataOverride != null ? dataOverride.AsteroidSprite : null;
        spriteRenderer.sprite = overrideSprite != null ? overrideSprite : defaultSprite;

        CurrentHealth = asteroidData.BaseMaxHealth;
        mass = massOverride ?? RollVariation(asteroidData.BaseMass, asteroidData.MassVariationPercentage);
        size = sizeOverride ?? RollVariation(asteroidData.BaseSize, asteroidData.SizeVariationPercentage);

        asteroidRigidbody.mass = mass;
        asteroidRigidbody.linearDamping = linearDamping;
        asteroidRigidbody.linearVelocity = velocity;
        asteroidRigidbody.angularVelocity = angularVelocity
        ?? Random.Range(-asteroidData.MaxInitialSpin, asteroidData.MaxInitialSpin);
        transform.localScale = new Vector3(size, size, 1.0f);

        isDead = false;
        isInitialized = true;
    }

    /// <summary>Reduces health by the damage in the context. The asteroid is destroyed on the next Update.</summary>
    public void TakeDamage(DamageContext context)
    {
        CurrentHealth -= context.DamageAmount;
    }

    /// <summary>
    /// Ignores collisions between this asteroid and <paramref name="other"/> until the two no longer overlap.
    /// </summary>
    public void IgnoreCollisionUntilClear(Collider2D other)
    {
        StartCoroutine(IgnoreCollisionUntilClearRoutine(other));
    }

    private static float RollVariation(float baseValue, float variationFraction)
    {
        return baseValue * Random.Range(1.0f - variationFraction, 1.0f + variationFraction);
    }

    private IEnumerator IgnoreCollisionUntilClearRoutine(Collider2D other)
    {
        if (asteroidCollider == null || other == null)
        {
            yield break;
        }

        Physics2D.IgnoreCollision(asteroidCollider, other, true);
        ignoredColliders.Add(other);

        while (other != null && other.gameObject.activeInHierarchy)
        {
            ColliderDistance2D separation = asteroidCollider.Distance(other);
            if (!separation.isOverlapped && separation.distance > siblingClearance)
            {
                break;
            }

            yield return WaitForPhysicsStep;
        }

        if (other != null)
        {
            Physics2D.IgnoreCollision(asteroidCollider, other, false);
        }

        ignoredColliders.Remove(other);
    }

    private void DestroyAsteroid()
    {
        isDead = true;
        Vector2 parentVelocity = asteroidRigidbody.linearVelocity;

        SpawnExplosion(parentVelocity);
        SpawnFragments(parentVelocity);

        Died?.Invoke(this);
        PrefabPool.Release(gameObject);
    }

    private void SpawnExplosion(Vector2 parentVelocity)
    {
        if (asteroidData.ExplosionPrefab == null)
        {
            return;
        }

        ExplosionEffect explosion = PrefabPool.Get(
            asteroidData.ExplosionPrefab,
            transform.position,
            Quaternion.identity);
        explosion.Play(size * asteroidData.ExplosionScale, parentVelocity);
    }

    /// <summary>
    /// Splits the asteroid into fragments. In the parent's frame the fragments start at rest and each receives an
    /// impulse J, with the impulses summing to zero. Adding the parent's velocity back gives
    /// v = parentVelocity + J / fragmentMass, and because the fragment masses add up to the parent's mass,
    /// total momentum is conserved.
    /// </summary>
    private void SpawnFragments(Vector2 parentVelocity)
    {
        int count = asteroidData.DebrisCount;
        if (count <= 0 || asteroidData.FragmentPrefab == null || asteroidData.FragmentData == null)
        {
            return;
        }

        float fragmentMass = mass / count;
        float fragmentSize = size / Mathf.Sqrt(count);
        float parentDamping = asteroidRigidbody.linearDamping;
        float parentSpin = asteroidRigidbody.angularVelocity;

        RollFragmentImpulses(count, out Vector2[] directions, out Vector2[] impulses);

        AsteroidController[] fragments = new AsteroidController[count];
        for (int i = 0; i < count; i++)
        {
            Vector2 position = (Vector2)transform.position
            + directions[i] * size * asteroidData.FragmentSpawnRadius;

            AsteroidController fragment = PrefabPool.Get(asteroidData.FragmentPrefab, position, transform.rotation);
            fragment.Initialize(
                parentVelocity + impulses[i] / fragmentMass,
                parentDamping,
                dataOverride: asteroidData.FragmentData,
                massOverride: fragmentMass,
                sizeOverride: fragmentSize,
                angularVelocity: parentSpin);
            fragments[i] = fragment;
        }

        IgnoreSiblingCollisions(fragments);
    }

    /// <summary>
    /// Rolls evenly spaced outward directions and an impulse along each. The average impulse is
    /// subtracted from all of them so the total is exactly zero.
    /// </summary>
    private void RollFragmentImpulses(int count, out Vector2[] directions, out Vector2[] impulses)
    {
        directions = new Vector2[count];
        impulses = new Vector2[count];
        Vector2 impulseSum = Vector2.zero;

        float startAngle = Random.Range(0.0f, 2.0f * Mathf.PI);
        float variation = asteroidData.ImpulseVariationPercentage;

        for (int i = 0; i < count; i++)
        {
            float angle = startAngle + 2.0f * Mathf.PI * i / count;
            directions[i] = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            float magnitude = asteroidData.ExplosionImpulse * Random.Range(1.0f - variation, 1.0f + variation);
            impulses[i] = directions[i] * magnitude;
            impulseSum += impulses[i];
        }

        Vector2 meanImpulse = impulseSum / count;
        for (int i = 0; i < count; i++)
        {
            impulses[i] -= meanImpulse;
        }
    }

    // Fresh fragments overlap, and that overlap would otherwise register as damaging collisions.
    private static void IgnoreSiblingCollisions(AsteroidController[] fragments)
    {
        for (int i = 0; i < fragments.Length; i++)
        {
            for (int j = i + 1; j < fragments.Length; j++)
            {
                fragments[i].IgnoreCollisionUntilClear(fragments[j].asteroidCollider);
            }
        }
    }
}
