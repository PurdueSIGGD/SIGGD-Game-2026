using UnityEngine;

public class Bullet : MonoBehaviour
{
    private bool isExplosive;
    private int damage;
    private float lifetime;

    private void Update()
    {
        if (lifetime > 0)
        {
            lifetime -= 1f * Time.deltaTime;
        }
        else
        {
            ProjectileManager.Instance.ProjectilePool.Release(gameObject);
        }
    }

    public void PopulateBulletValues(ProjectileScriptableObject projectileScriptableObject, float bulletRotationTargetAngle, Vector2 shootDirection)
    {
        isExplosive = projectileScriptableObject.IsExplosive;
        damage = projectileScriptableObject.Damage;
        lifetime = projectileScriptableObject.Lifetime;

        // getting needed components
        Rigidbody2D bulletRB = gameObject.GetComponent<Rigidbody2D>();
        SpriteRenderer bulletSR = gameObject.GetComponent<SpriteRenderer>();

        // set bullet sprite
        bulletSR.sprite = projectileScriptableObject.Sprite;

        // rotate bullet to face player
        bulletRB.rotation = bulletRotationTargetAngle + projectileScriptableObject.BulletAngleOffset;

        // give bullet its speed and direction
        bulletRB.AddForce(shootDirection * projectileScriptableObject.Velocity, ForceMode2D.Impulse);
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

    public float GetVelocity()
    {
        return velocity;
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

    public void SetVelocity(float inVelocity)
    {
        velocity = inVelocity;
    }
}
