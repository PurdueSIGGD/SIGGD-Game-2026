using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public static int Count = 0;
    private int num;
    [SerializeField] private ProjectileManager pm;
    private ProjectileScriptableObject pso;
    private int[] origin = new int[2] {0, 0};
    private int projectileCount;
    private float delay = 0;
    private float angleOffset = 0;
    private float hOffset = 0;

    public BossAttack ()
    {
        Count++;
        num = Count;
    }
    public void Test(GameObject bossObject)
    {
        Debug.Log("It works: " + num);
        GameObject gameObject = new GameObject();
        gameObject.transform.position = new Vector3(origin[0], origin[1], 0);
        pm.ShootProjectileTowardsDirection(pso, gameObject.transform, new Vector2(0, 1));
    }
}
