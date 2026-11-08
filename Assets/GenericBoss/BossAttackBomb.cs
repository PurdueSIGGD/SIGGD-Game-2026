using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using PlayerSystem;

[CreateAssetMenu(fileName = "BossAttackBomb", menuName = "Data/BossAttackBomb")]
public class BossAttackBomb : BossAttack
{
    [field: SerializeField] private ProjectileScriptableObject Pso {get; set;}
    [field: SerializeField] private int[] Origin {get; set;} = new int[2] {0, 0};
    [field: SerializeField] private int ProjectileCount {get; set;} = 1;
    [field: SerializeField] private int Delay {get; set;} = 0;
    [field: SerializeField] private LayerMask TargetLayerMask {get; set;} = 0;
    [field: SerializeField] private LayerMask IgnorePlayerMask { get; set; } = 0;

    private System.Random rng = new System.Random();
    override public async UniTask Shoot(GameObject bossObject)
    {
        
        Player[] players = PlayerRegistry.Players;
        Player target = GameObject.FindAnyObjectByType<Player>();
        Debug.Log("test" + target);

        GameObject targetObject = new GameObject();
        targetObject.transform.position = new Vector3(Origin[0], Origin[1], 0);

        Rigidbody2D target_rb = target.gameObject.GetComponent<Rigidbody2D>();

        for (int i = 0; i < ProjectileCount; i++)
        {
            GameObject player_go = new GameObject();
            Vector3 player_pos = target_rb.position;
            Vector3 player_vel = target_rb.linearVelocity;
            player_go.transform.position = new Vector3(player_pos.x + player_vel.x, player_pos.y + player_vel.y, 0);

            ProjectileManager.Instance.ShootProjectileTowardsPoint(Pso, targetObject.transform, player_go.transform, TargetLayerMask, IgnorePlayerMask);
            Destroy(player_go);
            await UniTask.Delay(Delay);
        }
        Destroy(targetObject);
        
    }
}
