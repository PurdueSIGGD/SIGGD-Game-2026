using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Miscellaneous", menuName = "Scriptable Objects/Miscellaneous")]
public class MiscellaneousScriptableObject : ScriptableObject
{
    public float Lifetime;
    public Sprite Sprite;
    public float BulletAngleOffset;
    public float Velocity;

    public Sprite UseSprite; // sprite for when you use the misc you just threw like the explosion for the bomb or suck sprite for the magnet throwable
    public GameObject RopeObject; // really only for grappling hook but if we have a gameobject that needs to follow the projectile

    // everything from here downward used the sprite assigned to generate a polygoncollider so that only one collider has to be made and then the sprite and collider are just applied 
    public List<Vector2[]> PhysicsPaths = new List<Vector2[]>();
    private static readonly List<Vector2> pathBuffer = new List<Vector2>();

    private void OnValidate()
    {
        // clean list in between updates in the scriptable object
        PhysicsPaths.Clear();

        if (Sprite == null)
        {
            return;
        }

        // get total count of shapes in the sprite
        int shapeCount = Sprite.GetPhysicsShapeCount();

        // for each shape get its shape and add it to the list
        for (int i = 0; i < shapeCount; i++)
        {
            pathBuffer.Clear();
            Sprite.GetPhysicsShape(i, pathBuffer);
            PhysicsPaths.Add(pathBuffer.ToArray());
        }
    }
}
