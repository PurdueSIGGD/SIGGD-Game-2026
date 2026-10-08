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
    /// <summary>
    /// Stores health level upper boundries (in decending order) for each phase.
    /// </summary>
    public int[] PhaseHealthBoundaries;
    /// <summary>
    /// Stores the current phase (index into PhaseHealthBoundries).
    /// </summary>
    public int CurrentPhase = 0;

    /// <summary>
    /// Applies damage done to boss and updates related parts (i.e. health, phase, death)
    /// </summary>
    /// <param name="context">Information about the damage done (i.e. damage amount, attacker, victim)</param>
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
        Debug.Assert(phaseBoundaries.Length == 0, "Missing Full Health Phase");
        MaxHealth => PhaseHealthBoundaries[0];
    }

    void Update()
    {
        
    }
}
