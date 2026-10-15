using UnityEngine;
using UnityEngine.Events;
using Random = System.Random;
using IDamageable;
public class GenericBoss : MonoBehaviour
{
    public float CurrentHealth => currentHealth;
    [SerializeField] private float currentHealth;
    public float MaxHealth;
    public UnityEvent<DamageContext> OnBossTakeDamage;
    public UnityEvent<DamageContext> OnBossDeath;
    /// <summary>
    /// Stores health level upper boundries (in decending order) for each phase.
    /// </summary>
    public int[] PhaseHealthBoundaries;
    /// <summary>
    /// Stores the current phase (index into PhaseHealthBoundries).
    /// </summary>
    public int CurrentPhase = 0;

    private AttackPercent[] AttackPercentList;
    private Random rng = new System.Random();

    /// <summary>
    /// Applies damage done to boss and updates related parts (i.e. health, phase, death)
    /// </summary>
    /// <param name="context">Information about the damage done (i.e. damage amount, attacker, victim)</param>
    public void TakeDamage(DamageContext context)
    {
        currentHealth -= context.DamageAmount;
        OnBossTakeDamage?.Invoke(context);

        for (; CurrentPhase < PhaseHealthBoundaries.Length-1 && currentHealth < PhaseHealthBoundaries[CurrentPhase+1]; CurrentPhase++) {}

        if (currentHealth <= 0)
        {
            OnBossDeath?.Invoke(context);
        }
    }

    public int BossCalculator()
    {
        int TotalPercentage = 0;
        foreach(AttackPercent c in AttackPercentList){
            TotalPercentage += c.Chance[CurrentPhase];
        }
        int AttackNumber = rng.Next(1, TotalPercentage + 1);
        int index = 0;
        while (true)
        {
            if (AttackPercentList[index].Chance[CurrentPhase] >= AttackNumber)
            {
                return AttackPercentList[index].Attacks;
            }
            AttackNumber -= AttackPercentList[index].Chance[CurrentPhase];
            index++;
        }
    }
    void Start()
    {
        Debug.Assert(PhaseHealthBoundaries.Length == 0, "Missing Full Health Phase");
        MaxHealth = PhaseHealthBoundaries[0];
    }

    void Update()
    {
        
    }
}

public struct AttackPercent
{
    public int[] Chance {get;}
    public int Attacks {get;} //TODO: make this an attack class

    public AttackPercent(int[] chance, int attacks){
        Chance = chance;
        Attacks = attacks;
    }
}