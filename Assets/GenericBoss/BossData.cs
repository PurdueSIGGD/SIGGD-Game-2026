using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "BossData", menuName = "Data/BossData")]
public class BossData : ScriptableObject
{
    // public UnityEvent<DamageContext> OnBossTakeDamage;
    // public UnityEvent<DamageContext> OnBossDeath;
    /// <summary>
    /// Stores health level upper boundries (in decending order) for each phase.
    /// </summary>
    [field: SerializeField] public int[] PhaseHealthBoundaries {get; private set; }
    [field: SerializeField] public float SpeedDamageThreshold {get; private set; } = 5.0f;
    [field: SerializeField] public float DamageMultiplier {get; private set; } = 10.0f;
    [field: SerializeField] public AttackPercents[] AttackPercentList {get; private set; }
}
