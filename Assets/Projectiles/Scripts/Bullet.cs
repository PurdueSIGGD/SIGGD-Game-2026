using UnityEngine;

public class Bullet : MonoBehaviour
{
    private bool isExplosive;
    private int damage;
    private float lifetime;
    private float velocity;

    private void Update()
    {
        if (lifetime > 0)
        {
            lifetime -= 1f * Time.deltaTime;
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
