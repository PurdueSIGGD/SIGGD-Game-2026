using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(fileName = "BossAttackLaser", menuName = "Data/BossAttackLaser")]
public class BossAttackLaser : BossAttack
{
    [field: SerializeField] private Vector2 Origin {get; set;} = Vector2.zero;
    [field: SerializeField] private float Length {get; set;}
    [field: SerializeField] private float LeftAngle {get; set;}
    [field: SerializeField] private float RightAngle {get; set;}
    [field: SerializeField] private float Damage {get; set;}
    [field: SerializeField] private float Lifetime {get; set;}
    [field: SerializeField] private GameObject Laser {get; set;}


    override public async UniTaskVoid Shoot(GameObject bossObject)
    {
        GameObject laserL = Instantiate(Laser, Origin, Quaternion.identity);
        laserL.GetComponent<Laser>().InitLaser(Length, LeftAngle, Origin, Damage, Lifetime);
        GameObject laserR = Instantiate(Laser, Origin, Quaternion.identity);
        laserR.GetComponent<Laser>().InitLaser(Length, RightAngle, Origin, Damage, Lifetime);
    }
}
