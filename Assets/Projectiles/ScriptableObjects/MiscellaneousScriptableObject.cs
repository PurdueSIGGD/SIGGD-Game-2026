using UnityEngine;

[CreateAssetMenu(fileName = "Miscellaneous", menuName = "Scriptable Objects/Miscellaneous")]
public class MiscellaneousScriptableObject : ScriptableObject
{
    public ProjectileScriptableObject miscProjectileBehavior;
    public Sprite UseSprite; // sprite for when you use the misc you just threw like the explosion for the bomb or suck sprite for the magnet throwable
    public GameObject RopeObject; // really only for grappling hook but if we have a gameobject that needs to follow the projectile
}
