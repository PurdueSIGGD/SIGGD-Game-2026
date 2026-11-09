using UnityEngine;

/// <summary>
/// Stores an attack and its chance weight for each boss phase.
/// </summary>
[System.Serializable]
[CreateAssetMenu(fileName = "AttackPercents", menuName = "Data/AttackPercents")]
public class AttackPercents : ScriptableObject
{
    /// <summary>
    /// An array of the weights for this attack for each boss phase.
    /// </summary>
    [field: SerializeField] public uint[] ChanceWeights {get; private set;}
    [field: SerializeField] public BossAttack Attack {get; private set;}

    public AttackPercents(uint[] chanceWeights, BossAttack attack){
        ChanceWeights = chanceWeights;
        Attack = attack;
    }
}