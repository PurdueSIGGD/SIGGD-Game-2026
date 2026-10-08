using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileScriptableObject", menuName = "Scriptable Objects/ProjectileScriptableObject")]
public class ProjectileScriptableObject : ScriptableObject
{
    public bool IsExplosive;
    public int Damage;
    public float Lifetime;
    public float Velocity;
    public Sprite Sprite; // if we can assign sprite directly in editor than this should work otherwise an image should be good
    public float BulletAngleOffset; // offsets the bullet used when attack are being fired to a point but need to be a little offset think like a shotgun or a boss shooting waves of attacks at different angles
                                    // just tweak this value from the gun script to change how the bullet is rotated
}