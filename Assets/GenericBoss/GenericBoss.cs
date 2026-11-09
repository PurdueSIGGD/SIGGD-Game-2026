using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;


public class GenericBoss : MonoBehaviour, IDamageable
{
    [SerializeField] private BossData bossData;
    public UnityEvent<DamageContext> OnBossTakeDamage;
    public UnityEvent<DamageContext> OnBossDeath;

    /// <summary>
    /// Stores the current phase (index into PhaseHealthBoundries).
    /// </summary>
    public int CurrentPhase = 0;
    private BossAttack lastAttack = null;

    [ShowInInspector, ReadOnly] public float CurrentHealth { get; private set; }
    [ShowInInspector, ReadOnly] public float MaxHealth { get; private set; }
    [ShowInInspector, ReadOnly] public bool IsAlive { get; private set; } = true;

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
            IsAlive = false;
            OnBossDeath?.Invoke(context);
        }
    }

    private BossAttack CalculateRandomBossAttack()
    {
        uint totalPercentage = 0;
        foreach(AttackPercents c in bossData.AttackPercentList){
            totalPercentage += c.ChanceWeights[CurrentPhase];
        }
        int attackNumber = Random.Range(1, (int)totalPercentage + 1);
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
        MaxHealth = bossData.PhaseHealthBoundaries[0];
        CurrentHealth = MaxHealth;
        AttackLoop(this.GetCancellationTokenOnDestroy()).Forget();
    }

    /// <summary>
    /// Continuously picks a random attack, waits for it to finish firing, then waits
    /// that attack's cooldown before the next one. Stops when the boss dies or is destroyed.
    /// </summary>
    private async UniTaskVoid AttackLoop(CancellationToken token)
    {
        while (IsAlive && !token.IsCancellationRequested)
        {
            BossAttack attack = CalculateRandomBossAttack();
            await attack.Shoot(gameObject).AttachExternalCancellation(token);
            await UniTask.Delay(Mathf.FloorToInt(attack.Cooldown * 1000), cancellationToken: token);
        }
    }

    public void ResetHealth()
    {
        throw new System.NotImplementedException();
    }
}



