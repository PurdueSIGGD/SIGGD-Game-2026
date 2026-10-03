using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] bool isExplosive;
    [SerializeField] int damage;
    [SerializeField] float lifetime;

    private void Update()
    {
        if (lifetime > 0)
        {
            lifetime -= 1f * Time.deltaTime;
        }
    }
}
