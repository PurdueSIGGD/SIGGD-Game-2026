using UnityEngine;

[RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(SpriteRenderer))]
public class AsteroidController : MonoBehaviour, IDamageable
{
    [SerializeField] private AsteroidData asteroidData;
    Rigidbody2D asteroidRigidbody;
    SpriteRenderer spriteRenderer;

    float currentHealth;
    float mass;
    float size;

    [SerializeField] private float speedDamageThreshold = 5.0f;
    [SerializeField] private float damageMultiplier = 10.0f;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => asteroidData != null ? asteroidData.baseMaxHealth : 0.0f;

    void Awake() {
        asteroidRigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitializeAsteroid();
    }

    // Update is called once per frame
    void Update()
    {
        if (asteroidData == null) return;

        if (currentHealth <= 0) {
            DestroyAsteroid();
        }
    }

    private void InitializeAsteroid() {
        Debug.Log("Initializing Asteroid");
        if (asteroidData == null) return;

        currentHealth = asteroidData.baseMaxHealth;
        mass = asteroidData.baseMass * Random.Range(1.0f - asteroidData.massVariationPercentage, 1.0f + asteroidData.massVariationPercentage);
        size = asteroidData.baseSize * Random.Range(1.0f - asteroidData.sizeVariationPercentage, 1.0f + asteroidData.sizeVariationPercentage);

        asteroidRigidbody.mass = mass;
        transform.localScale = new Vector3(size, size, 1.0f);

        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        asteroidRigidbody.AddForce(randomDirection * Random.Range(1.0f, 5.0f), ForceMode2D.Impulse);
        asteroidRigidbody.AddTorque(Random.Range(-1.0f, 1.0f), ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        float impactSpeed = collision.relativeVelocity.magnitude;

        if (impactSpeed > speedDamageThreshold) {
            float damageAmount = impactSpeed * damageMultiplier;
            TakeDamage(new DamageContext {
                DamageAmount = damageAmount,
                Attacker = collision.gameObject,
                Victim = gameObject
            });
        }
    }

    public void TakeDamage(DamageContext context) {
        Debug.Log($"Asteroid took {context.DamageAmount} damage from {context.Attacker.name}");
        currentHealth -= context.DamageAmount;
    }

    private void DestroyAsteroid() {
        Debug.Log("Destroying Asteroid");
        Destroy(gameObject);
    }
}
