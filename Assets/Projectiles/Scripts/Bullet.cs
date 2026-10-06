using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] bool isExplosive;
    [SerializeField] int damage;
    [SerializeField] float lifetime;
    [SerializeField] float velocity;

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

    public int GetIsDamage()
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
}
