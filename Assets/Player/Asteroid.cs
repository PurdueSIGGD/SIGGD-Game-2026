using UnityEngine;

public class Asteroid : MonoBehaviour
{
    // FOR TESTING PURPOSES ONLY
    [SerializeField] private float speed = 2f;

    private Rigidbody2D rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        rb.linearVelocity = Random.insideUnitCircle.normalized * speed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.otherCollider.CompareTag("Player"))
        {
            collision.otherCollider.GetComponent<IDamageable>().TakeDamage(new DamageContext
            {
                DamageAmount = 1f,
                Attacker = gameObject,
                Victim = collision.otherCollider.gameObject
            });
        }
    }
}
