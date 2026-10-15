using UnityEngine;
using System.Collections.Generic;

public class Bullet : MonoBehaviour
{
    private bool isExplosive;
    private int damage;
    private float lifetime;

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
        if (lifetime > 0)
        {
            lifetime -= 1f * Time.deltaTime;
        }
        else
        {
            if (isExplosive)
            {
                // call some sort of blow up function eventually when that gets made
            }

            ProjectileManager.Instance.ProjectilePool.Release(gameObject);
        }
    }

    /// <summary>
    /// Set the sprite, position, rotation and velocity of the bullet as well as the basic bullet values gotten from the ProjectilScriptableObject
    /// </summary>
    public void PopulateBulletValues(ProjectileScriptableObject projectileScriptableObject, Transform firePoint, Vector2 shootDirection)
    {
        bulletRB.angularVelocity = 0f; // reset angular velocity just incase its not 0 before firing

        isExplosive = projectileScriptableObject.IsExplosive;
        damage = projectileScriptableObject.Damage;
        lifetime = projectileScriptableObject.Lifetime;

        gameObject.transform.position = firePoint.position;

        // set bullet sprite
        bulletSR.sprite = projectileScriptableObject.Sprite;

        SetBulletCollider(projectileScriptableObject);

        // rotate bullet
        Vector2 finalDirection = Quaternion.Euler(0, 0, projectileScriptableObject.BulletAngleOffset) * shootDirection;
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

    public bool GetIsExplosive()
    {
        return isExplosive;
    }

    public int GetDamage()
    {
        return damage;
    }

    public float GetLifetime()
    {
        return lifetime;
    }

    public void SetIsExplosive(bool inIsExplosive)
    {
        isExplosive = inIsExplosive;
    }

    public void SetDamage(int inDamage)
    {
        damage = inDamage;
    }
    public void SetLifetime(float inLifetime)
    {
        lifetime = inLifetime;
    }
}
