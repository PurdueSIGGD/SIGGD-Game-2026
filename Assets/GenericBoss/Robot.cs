using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using PlayerSystem;

public class Robot : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 10f;
    public UnityEvent<DamageContext> OnRobotTakeDamage;
    public UnityEvent<DamageContext> OnRobotDeath;

    public float CurrentHealth { get; private set; }
    public float MaxHealth => maxHealth;
    public bool IsAlive { get; private set; } = true;

    private System.Random rng = new System.Random();
    private int damage;
    private float lifetime;
    private Rigidbody2D bulletRB;
    private SpriteRenderer bulletSR;
    private PolygonCollider2D bulletCollider;
    public UnityEvent<GameObject> OnBulletHit;
    private ProjectileScriptableObject pso;
    private float dashTime;
    private float timeTillDash;
    private void Update()
    {
        if (lifetime > 0)
        {
            lifetime -= 1f * Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }

        if (timeTillDash > 0)
        {
            timeTillDash -= 1f * Time.deltaTime;
        }
        else
        {
            DashTowardsPlayer();
            timeTillDash = dashTime;
        }
    }
    public void InitRobot(ProjectileScriptableObject projectileScriptableObject, Transform firePoint, Vector2 shootDirection, float dashtime)
    {
        bulletRB = gameObject.GetComponent<Rigidbody2D>();
        bulletSR = gameObject.GetComponent<SpriteRenderer>();
        bulletCollider = gameObject.GetComponent<PolygonCollider2D>();

        pso = projectileScriptableObject;
        ResetHealth();

        dashTime = dashtime;
        timeTillDash = dashtime;

        bulletRB.angularVelocity = 0f; // reset angular velocity just incase its not 0 before firing

        damage = projectileScriptableObject.Damage;
        lifetime = projectileScriptableObject.Lifetime;

        gameObject.transform.position = firePoint.position;

        // set bullet sprite
        bulletSR.sprite = projectileScriptableObject.ProjectileSprite;
        bulletSR.color = projectileScriptableObject.ProjectileColor;

        gameObject.transform.localScale = projectileScriptableObject.ProjectileScale;

        SetBulletCollider(projectileScriptableObject);

        // rotate bullet
        Vector2 finalDirection = Quaternion.Euler(0, 0, projectileScriptableObject.SpriteAngleOffset) * shootDirection;
        bulletRB.rotation = Mathf.Atan2(finalDirection.y, finalDirection.x) * Mathf.Rad2Deg;

        // give bullet its speed and in the direction
        bulletRB.linearVelocity = finalDirection * projectileScriptableObject.Velocity;
    }

    private void SetBulletCollider(ProjectileScriptableObject projectileScriptableObject)
    {
        List<Vector2[]> paths = projectileScriptableObject.PhysicsPaths;

        if (paths != null && paths.Count > 0)
        {
            bulletCollider.pathCount = paths.Count;

            for (int i = 0; i < paths.Count; i++)
            {
                bulletCollider.SetPath(i, paths[i]);
            }
        }
        else
        {
            bulletCollider.pathCount = 0;
        }
    }

    public int GetDamage()
    {
        return damage;
    }

    public float GetLifetime()
    {
        return lifetime;
    }

    public void SetDamage(int inDamage)
    {
        damage = inDamage;
    }
    public void SetLifetime(float inLifetime)
    {
        lifetime = inLifetime;
    }

    public void TakeDamage(DamageContext context)
    {
        if (!IsAlive) return;

        CurrentHealth -= context.DamageAmount;
        OnRobotTakeDamage?.Invoke(context);

        if (CurrentHealth <= 0)
        {
            IsAlive = false;
            OnRobotDeath?.Invoke(context);
            Destroy(gameObject);
        }
    }

    public void ResetHealth()
    {
        CurrentHealth = maxHealth;
        IsAlive = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnBulletHit?.Invoke(collision.gameObject);
    }
    private void DashTowardsPlayer()
    {
        Player target = null;
        float bestDistSq = float.MaxValue;
        foreach (Player p in PlayerRegistry.Players)
        {
            if (p == null) continue;
            float d = ((Vector2)p.transform.position - bulletRB.position).sqrMagnitude;
            if (d < bestDistSq) { bestDistSq = d; target = p; }
        }
        if (target == null) return; // no player alive/registered: keep drifting

        Vector2 direction = (Vector2)target.transform.position - bulletRB.position;
        bulletRB.rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bulletRB.linearVelocity = direction.normalized * pso.Velocity;
    }
}