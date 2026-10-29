using System.Runtime.InteropServices;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public static int Count = 0;
    private int num;
    [SerializeField] public ProjectileScriptableObject pso;
    private ProjectileScriptableObject pso1;
    [field: SerializeField] private int[] Origin {get; set;} = new int[2] {0, 0};
    private int projectileCount = 1;
    private float delay = 0;
    private float angleOffset = 0;
    private float hOffset = 0;

    private void Update()
    {
        Debug.Log("testing1" + (pso == null));    
        pso1 = new ProjectileScriptableObject();
        Debug.Log("testing2" + (pso1 == null));    
    }
    

    public BossAttack ()
    {
        Count++;
        num = Count;
    }
    public void Test(GameObject bossObject)
    {
        Debug.Log("It works: " + num);
        GameObject gameObject = new GameObject();
        gameObject.transform.position = new Vector3(Origin[0], Origin[1], 0);
        Debug.Log("testing" + (pso1 == null));
        bool a = pso1.IsExplosive;
        ProjectileManager.Instance.ShootProjectileTowardsDirection(pso1, gameObject.transform, new Vector2(0, 1));
    }
}
