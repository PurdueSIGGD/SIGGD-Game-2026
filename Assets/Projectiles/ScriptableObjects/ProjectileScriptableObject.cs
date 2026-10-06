using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileScriptableObject", menuName = "Scriptable Objects/ProjectileScriptableObject")]
public class ProjectileScriptableObject : ScriptableObject
{
    public bool IsExplosive;
    public int Damage;
    public float Lifetime;
    public float Velocity;
    public SpriteRenderer Sprite;
}
