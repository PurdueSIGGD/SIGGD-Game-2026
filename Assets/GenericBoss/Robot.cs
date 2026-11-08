using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using Unity.VisualScripting;
using PlayerSystem;

public class Robot : MonoBehaviour
{
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
        Vector2 finalDirection = Quaternion.Euler(0, 0, projectileScriptableObject.ProjectileAngleOffset) * shootDirection;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnBulletHit?.Invoke(collision.gameObject);
    }
    private void DashTowardsPlayer()
    {
        Player[] players = PlayerRegistry.Players;
        Player target = GameObject.FindAnyObjectByType<Player>();
        print(target == null);
        Rigidbody2D target_rb = target.gameObject.GetComponent<Rigidbody2D>();
        Vector2 direction = new Vector2(target_rb.position.x - bulletRB.position.x, target_rb.position.y - bulletRB.position.y);
        bulletRB.rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bulletRB.linearVelocity = direction.normalized * pso.Velocity;
    }
}