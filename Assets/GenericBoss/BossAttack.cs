using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

public abstract class BossAttack : ScriptableObject
{
    ProjectileScriptableObject Pso;
    int[] Origin;
    int ProjectileCount;
    int Delay;
    public abstract UniTaskVoid Shoot(GameObject bossObject);
}