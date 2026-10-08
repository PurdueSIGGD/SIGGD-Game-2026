using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileScriptableObject", menuName = "Scriptable Objects/ProjectileScriptableObject")]
public class ProjectileScriptableObject : ScriptableObject
{
    public bool IsExplosive;
    public int Damage;
    public float Lifetime;
    public float Velocity;
    public Sprite Sprite; // if we can assign sprite directly in editor than this should work otherwise an image should be good
    public float BulletAngleOffset; // offsets the bullet probably by a multiple of 90 to make sure the bullet rotates to face the mouse that its firing towards properly
}
