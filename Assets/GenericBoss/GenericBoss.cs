using UnityEngine;
using UnityEngine.Events;


public class GenericBoss : MonoBehaviour
{
    [SerializeField] private BossData bossData;
    public UnityEvent<DamageContext> OnBossTakeDamage;
    public UnityEvent<DamageContext> OnBossDeath;
    public float CurrentHealth;
    private float maxHealth;
    /// <summary>
    /// Stores the current phase (index into PhaseHealthBoundries).
    /// </summary>
    public int CurrentPhase = 0;
    private System.Random rng = new System.Random();
    private BossAttack lastAttack = null;

    /// <summary>
    /// Applies damage done to boss and updates related parts (i.e. health, phase, death)
    /// </summary>
    /// <param name="context">Information about the damage done (i.e. damage amount, attacker, victim)</param>
    public void TakeDamage(DamageContext context)
    {
        // TODO: check who dealt damage and if they're elegible to deal damage
        
        CurrentHealth -= context.DamageAmount;
        OnBossTakeDamage?.Invoke(context);

        for (; CurrentPhase < bossData.PhaseHealthBoundaries.Length-1 && CurrentHealth < bossData.PhaseHealthBoundaries[CurrentPhase+1]; CurrentPhase++) {}

        if (CurrentHealth <= 0)
        {
            OnBossDeath?.Invoke(context);
        }
    }

    private BossAttack CalculateRandomBossAttack()
    {
        uint totalPercentage = 0;
        foreach(AttackPercents c in bossData.AttackPercentList){
            totalPercentage += c.ChanceWeights[CurrentPhase];
        }
        int attackNumber = rng.Next(1, (int)totalPercentage + 1);
        int index = 0;
        while (true)
        {
            if (bossData.AttackPercentList[index].ChanceWeights[CurrentPhase] >= attackNumber)
            {
                if (lastAttack != null && lastAttack == bossData.AttackPercentList[index].Attack)
                {
                    lastAttack = null;
                    return CalculateRandomBossAttack();
                }
                else
                {
                    lastAttack = bossData.AttackPercentList[index].Attack;
                    return bossData.AttackPercentList[index].Attack;
                }
            }
            attackNumber -= (int)bossData.AttackPercentList[index].ChanceWeights[CurrentPhase];
            index++;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        float impactSpeed = collision.relativeVelocity.magnitude;

        if (impactSpeed > bossData.SpeedDamageThreshold) {
            float damageAmount = impactSpeed * bossData.DamageMultiplier;
            TakeDamage(new DamageContext {
                DamageAmount = damageAmount,
                Attacker = collision.gameObject,
                Victim = gameObject
            });
        }
    }
    void Start()
    {
        Debug.Assert(bossData.PhaseHealthBoundaries.Length > 0, "Missing Full Health Phase");
        maxHealth = bossData.PhaseHealthBoundaries[0];
        CurrentHealth = maxHealth;
        // bossData.AttackPercentList = new AttackPercents[] {new AttackPercents(new uint[] {1},new BossAttack()), new AttackPercents(new uint[] {1},new BossAttack()), new AttackPercents(new uint[] {1},new BossAttack())};
        CalculateRandomBossAttack().Test(gameObject);
        CalculateRandomBossAttack().Test(gameObject);
        CalculateRandomBossAttack().Test(gameObject);
        CalculateRandomBossAttack().Test(gameObject);
        CalculateRandomBossAttack().Test(gameObject);
    }

    void Update()
    {
        
    }
}


/// <summary>
/// Stores an attack and its chance weight for each boss phase.
/// </summary>
[System.Serializable]
public struct AttackPercents
{
    /// <summary>
    /// An array of the weights for this attack for each boss phase.
    /// </summary>
    public uint[] ChanceWeights {get;}
    public BossAttack Attack {get;}

    public AttackPercents(uint[] chanceWeights, BossAttack attack){
        ChanceWeights = chanceWeights;
        Attack = attack;
    }
}
