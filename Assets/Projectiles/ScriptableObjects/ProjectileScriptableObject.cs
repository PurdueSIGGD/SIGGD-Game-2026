using UnityEngine;
using System.Collections.Generic;

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