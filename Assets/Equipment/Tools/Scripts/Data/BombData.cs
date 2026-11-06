using UnityEngine;

[CreateAssetMenu(fileName = "BombData", menuName = "Scriptable Objects/BombData")]
public class BombData : ToolProjectileData
{
    [field: SerializeField] public float fuseTime { get; private set; } = 2f;
    [field: SerializeField] public float blastRadius { get; private set; } = 3f;
    [field: SerializeField] public float blastDamage { get; private set; } = 0f;
    [field: SerializeField] public float blastImpulse { get; private set; } = 20f;
    [field: SerializeField] public float blastMinDistance { get; private set; } = 0.5f;
}
