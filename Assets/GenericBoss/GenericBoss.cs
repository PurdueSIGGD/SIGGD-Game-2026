using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

public class GenericBoss : MonoBehaviour, IDamagable
{
    public float CurrentHealth => currentHealth;
    [SerializeField] private float currentHealth;
    public float MaxHealth => maxHealth;
    [SerializeField] private float maxHealth;
    public UnityEvent<GameObject, float> OnBossTakeDamage;
    public UnityEvent<GameObject> OnBossDeath;
    public float[] PhaseBoundaries;
    public int CurrentPhase = 0;

    public void TakeDamage(DamageContext context)
    {
        if (context.Victim == GameObject.gameObject)
        {
            health -= context.DamageAmount;
            OnBossTakeDamage?.Invoke(context);

            for (; CurrentPhase < PhaseBoundaries.Length-1 && health < phaseBoundaries[currentPhase+1] * MaxHealth; CurrentPhase++){}

            if (health <= 0)
            {
                OnBossDeath?.Invoke(context);
            }
        }
        
    }

    void Start()
    {
        if (phaseBoundaries.Length > 0 && phaseBoundaries[0] != 1)
        {
            Debug.Log("Missing Full Health Phase");
        }
        MaxHealth = health;
    }

    void Update()
    {
        
    }
}
