using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

[System.Serializable]
[CreateAssetMenu(fileName = "BossAttack", menuName = "Data/BossAttack")]
public class BossAttack : ScriptableObject
{
    public static int Count = 0;
    private int num;
    [SerializeField] public ProjectileScriptableObject pso;
    private ProjectileScriptableObject pso1;
    [field: SerializeField] private int[] Origin {get; set;} = new int[2] {0, 0};
    public int ProjectileCount = 1;
    public int Delay = 0;
    public float InitAngle = 0f;
    public float AngleOffset = 0f;
    public float HOffset = 0f;

    public BossAttack ()
    {
        Count++;
        num = Count;
    }

    public async UniTaskVoid Test(GameObject bossObject)
    {
        GameObject gameObject = new GameObject();
        gameObject.transform.position = new Vector3(Origin[0], Origin[1], 0);
        Vector2 angle = new Vector2(1, 0);
        float newAngle = InitAngle;
        for (int i = 0; i < ProjectileCount; i++)
        {
            ProjectileManager.Instance.ShootProjectileTowardsDirection(pso, gameObject.transform, angle);
            newAngle = newAngle + AngleOffset * (float) Math.PI / 180;
            angle = new Vector2((float) Math.Sin(newAngle), (float) Math.Cos(newAngle));
            gameObject.transform.position = new Vector3(gameObject.transform.position.x + HOffset, Origin[1], 0);
            await UniTask.Delay(Delay);
        }
    }
}
