using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(fileName = "BossAttackLinearFan", menuName = "Data/BossAttackLinearFan")]
public class BossAttackLinearFan : BossAttack
{
    [field: SerializeField] private ProjectileScriptableObject Pso {get; set;}
    [field: SerializeField] private int[] Origin {get; set;} = new int[2] {0, 0};
    [field: SerializeField] private int ProjectileCount {get; set;} = 1;
    [field: SerializeField] private int Delay {get; set;} = 0;
    [field: SerializeField] private float InitAngle {get; set;} = 0f;
    [field: SerializeField] private float AngleOffset {get; set;} = 0f;
    [field: SerializeField] private float HOffset {get; set;} = 0f;
    override public async UniTaskVoid Shoot(GameObject bossObject)
    {
        GameObject gameObject = new GameObject();
        gameObject.transform.position = new Vector3(Origin[0], Origin[1], 0);
        float newAngle = InitAngle * (float) Math.PI / 180;
        for (int i = 0; i < ProjectileCount; i++)
        {
            Vector2 angle = new Vector2((float) Math.Sin(newAngle), (float) Math.Cos(newAngle));
            ProjectileManager.Instance.ShootProjectileTowardsDirection(Pso, gameObject.transform, angle);
            newAngle = newAngle + AngleOffset * (float) Math.PI / 180;
            gameObject.transform.position = new Vector3(gameObject.transform.position.x + HOffset, Origin[1], 0);
            await UniTask.Delay(Delay);
        }
    }
}
