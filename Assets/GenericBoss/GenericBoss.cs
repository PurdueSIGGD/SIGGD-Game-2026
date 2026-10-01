using UnityEngine;
using UnityEngine.Events;

public class GenericBoss : MonoBehaviour
{
    public float Health => health;
    [SerializeField] private float health;
    public UnityEvent<GameObject, float> OnBossTakeDamage;

    public void DamageBoss(GameObject source, float damage)
    {
        health -= damage;
        OnBossTakeDamage?.Invoke(source, damage);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
