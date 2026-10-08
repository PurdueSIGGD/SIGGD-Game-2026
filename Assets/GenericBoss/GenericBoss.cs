using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

public class GenericBoss : MonoBehaviour, IDamagable
{
    public float CurrentHealth => currentHealth;
    [SerializeField] private float currentHealth;
    public float MaxHealth;
    public UnityEvent<GameObject, float> OnBossTakeDamage;
    public UnityEvent<GameObject> OnBossDeath;
    public int[] PhaseHealthBoundaries;
    public int CurrentPhase = 0;

    public void TakeDamage(DamageContext context)
    {
        if (context.Victim == GameObject.gameObject)
        {
            currentHealth -= context.DamageAmount;
            OnBossTakeDamage?.Invoke(context);

            for (; CurrentPhase < PhaseBoundaries.Length-1 && currentHealth < phaseBoundaries[currentPhase+1]; CurrentPhase++) {}

            if (currentHealth <= 0)
            {
                OnBossDeath?.Invoke(context);
            }
        }
        
    }

    void Start()
    {
        if (phaseBoundaries.Length == 0)
        {
            Debug.Log("Missing Full Health Phase");
            throw;
        }
        MaxHealth => PhaseHealthBoundaries[0];
    }

    void Update()
    {
        
    }
}
