using UnityEngine;

public class Asteroid : MonoBehaviour
{
    // FOR TESTING PURPOSES ONLY
    [SerializeField] private float speed = 2f;

    private Rigidbody2D rb;
    private Vector2 startPos;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        startPos = rb.position;
        rb.linearVelocity = Random.insideUnitCircle.normalized * speed;
    }

    void FixedUpdate()
    {
        if ((startPos - rb.position).magnitude > 20f)
        {
            Destroy(gameObject);
        }
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
            Destroy(gameObject);
        }
    }
}
