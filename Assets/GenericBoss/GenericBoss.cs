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

    public AttackPercent[] AttackPercentList = {new AttackPercent(0,0)};
    public Random rng = new Random()

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

    public int bossCalculator()
    {
        int TotalPercentage = 0;
        foreach(AttackPercent c in AttackPercentList){
            TotalPercentage += c.Chance;
        }
        int AttackNumber = rng.Next(1, TotalPercentage);
        int index = 0;
        while (true)
        {
            if (AttackPercentList[index].Chance >= AttackNumber)
            {
                return AttackPercentList[index].Attacks;
            }
            AttackNumber -= AttackPercentList[index].Chance;
            index++;
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

struct AttackPercent
{
    public int Chance {get;}
    public int Attacks {get;} //TODO: make this an attack class

    public AttackPercent(int chance, int attacks){
        Chance = chance;
        Attacks = attacks;
    }
}