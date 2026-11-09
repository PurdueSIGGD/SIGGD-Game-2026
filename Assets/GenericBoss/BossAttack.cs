using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

public abstract class BossAttack : ScriptableObject
{
    ProjectileScriptableObject Pso;
    int[] Origin;
    int ProjectileCount;
    int Delay;

    /// <summary>
    /// Seconds the boss waits after this attack finishes before starting its next attack.
    /// </summary>
    [field: SerializeField] public float Cooldown {get; private set;} = 2f;

    /// <summary>
    /// Performs the attack. The returned task completes once the attack has finished firing.
    /// </summary>
    public abstract UniTask Shoot(GameObject bossObject);
}