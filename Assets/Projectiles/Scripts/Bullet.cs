using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class Bullet : MonoBehaviour
{
    public static UnityEvent<Bullet, GameObject> OnBulletHit;

    public bool IsExplosive { get; set; }
    public float Damage { get; set; }
    public float Lifetime { get; set; }
    public bool DestroyOnHit { get; set; } = true;


    private Rigidbody2D bulletRB;
    private SpriteRenderer bulletSR;
    private PolygonCollider2D bulletCollider;

    private void Awake()
    {
        bulletRB = gameObject.GetComponent<Rigidbody2D>();
        bulletSR = gameObject.GetComponent<SpriteRenderer>();
        bulletCollider = gameObject.GetComponent<PolygonCollider2D>();
    }

    private void Update()
    {
        if (Lifetime > 0)
        {
            Lifetime -= 1f * Time.deltaTime;
        }
        else
        {
            if (IsExplosive)
            {
                // call some sort of blow up function eventually when that gets made
            }

            ProjectileManager.Instance.ProjectilePool.Release(gameObject);
        }
    }

    /// <summary>
    /// Set the sprite, position, rotation and velocity of the bullet as well as the basic bullet values gotten from the ProjectilScriptableObject
    /// </summary>
    public void PopulateBulletValues(ProjectileScriptableObject projectileScriptableObject, Transform firePoint, Vector2 shootDirection, LayerMask layerToHit, LayerMask layerToIgnore)
    {
        bulletRB.angularVelocity = 0f; // reset angular velocity just incase its not 0 before firing

        IsExplosive = projectileScriptableObject.IsExplosive;
        Damage = projectileScriptableObject.Damage;
        Lifetime = projectileScriptableObject.Lifetime;

        gameObject.transform.position = firePoint.position;

        // set bullet sprite
        bulletSR.sprite = projectileScriptableObject.ProjectileSprite;
        bulletSR.color = projectileScriptableObject.ProjectileColor;

        gameObject.transform.localScale = projectileScriptableObject.ProjectileScale;

        SetBulletCollider(projectileScriptableObject, layerToHit, layerToIgnore);

        // rotate bullet
        Vector2 finalDirection = Quaternion.Euler(0, 0, projectileScriptableObject.ProjectileAngleOffset) * shootDirection;
        bulletRB.rotation = Mathf.Atan2(finalDirection.y, finalDirection.x) * Mathf.Rad2Deg;

        // give bullet its speed and in the direction
        bulletRB.linearVelocity = finalDirection * projectileScriptableObject.Velocity;
    }

    private void SetBulletCollider(ProjectileScriptableObject projectileScriptableObject, LayerMask layerToHit, LayerMask layerToIgnore)
    {
        List<Vector2[]> paths = projectileScriptableObject.PhysicsPaths;

        bulletCollider.includeLayers = layerToHit;
        bulletCollider.excludeLayers = layerToIgnore;

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        var damageable = other.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(new DamageContext(Damage, gameObject, other.gameObject));
        }

        OnBulletHit?.Invoke(this, other.gameObject);

        if (DestroyOnHit)
        {
            ProjectileManager.Instance.ProjectilePool.Release(gameObject);
        }
    }
}
