using UnityEngine;
using UnityEngine.Events;

public class GenericBoss : MonoBehaviour
{
    public float Health => health;
    [SerializeField] private float health;
    public UnityEvent<GameObject, float> OnBossTakeDamage;
    public UnityEvent<GameObject> OnBossDeath;

    public void DamageBoss(GameObject source, float damage)
    {
        health -= damage;
        OnBossTakeDamage?.Invoke(source, damage);

        if (health <= 0)
        {
            OnBossDeath?.Invoke(source);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
