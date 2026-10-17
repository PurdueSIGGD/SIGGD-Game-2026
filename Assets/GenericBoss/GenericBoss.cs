using UnityEngine;
using UnityEngine.Events;


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
    [SerializeField] private float speedDamageThreshold = 5.0f;
    [SerializeField] private float damageMultiplier = 10.0f;



    private AttackPercents[] attackPercentList;
    private System.Random rng = new System.Random();

    /// <summary>
    /// Applies damage done to boss and updates related parts (i.e. health, phase, death)
    /// </summary>
    /// <param name="context">Information about the damage done (i.e. damage amount, attacker, victim)</param>
    public void TakeDamage(DamageContext context)
    {
        // TODO: check who dealt damage and if they're elegible to deal damage
        
        currentHealth -= context.DamageAmount;
        OnBossTakeDamage?.Invoke(context);

        for (; CurrentPhase < PhaseHealthBoundaries.Length-1 && currentHealth < PhaseHealthBoundaries[CurrentPhase+1]; CurrentPhase++) {}

        if (currentHealth <= 0)
        {
            OnBossDeath?.Invoke(context);
        }
    }

    private int CalculateRandomBossAttack()
    {
        uint totalPercentage = 0;
        foreach(AttackPercents c in attackPercentList){
            totalPercentage += c.ChanceWeights[CurrentPhase];
        }
        int attackNumber = rng.Next(1, (int)totalPercentage + 1);
        int index = 0;
        while (true)
        {
            if (attackPercentList[index].ChanceWeights[CurrentPhase] >= attackNumber)
            {
                return attackPercentList[index].Attack;
            }
            attackNumber -= (int)attackPercentList[index].ChanceWeights[CurrentPhase];
            index++;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        float impactSpeed = collision.relativeVelocity.magnitude;

        if (impactSpeed > speedDamageThreshold) {
            float damageAmount = impactSpeed * damageMultiplier;
            TakeDamage(new DamageContext {
                DamageAmount = damageAmount,
                Attacker = collision.gameObject,
                Victim = gameObject
            });
        }
    }
    void Start()
    {
        Debug.Assert(PhaseHealthBoundaries.Length > 0, "Missing Full Health Phase");
        MaxHealth = PhaseHealthBoundaries[0];
        currentHealth = MaxHealth;
    }

    void Update()
    {
        
    }
}


/// <summary>
/// Stores an attack and its chance weight for each boss phase.
/// </summary>
public struct AttackPercents
{
    /// <summary>
    /// An array of the weights for this attack for each boss phase.
    /// </summary>
    public uint[] ChanceWeights {get;}
    public int Attack {get;} //TODO: make this an attack class (maybe "BossAttack")

    public AttackPercents(uint[] chanceWeights, int attack){
        ChanceWeights = chanceWeights;
        Attack = attack;
    }
}