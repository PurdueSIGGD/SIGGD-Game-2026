using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "MiscellaneousScriptableObject", menuName = "Scriptable Objects/MiscellaneousScriptableObject")]
public class MiscellaneousScriptableObject : ScriptableObject
{
    public float Lifetime;
    public float Velocity;
    public float SpriteAngleOffset;

    public Sprite MiscProjectileSprite;

    public Color32 MiscProjectileColor = new Color32(255, 255, 255, 255); // in case we wanna change the color
    public Vector3 MiscProjectileScale = new Vector3(1, 1, 1); // so that we can scale up and scale down bullets for different attack and have it be done in scriptable object instead of in game

    public Sprite UseSprite; // sprite for when you use the misc you just threw like the explosion for the bomb or suck sprite for the magnet throwable
    public GameObject RopeObject; // really only for grappling hook but if we have a gameobject that needs to follow the projectile

    // everything from here downward used the sprite assigned to generate a polygoncollider so that only one collider has to be made and then the sprite and collider are just applied 
    public List<Vector2[]> PhysicsPaths = new List<Vector2[]>();
    private static readonly List<Vector2> pathBuffer = new List<Vector2>();

    private void OnValidate()
    {
        // clean list in between updates in the scriptable object
        PhysicsPaths.Clear();

        if (MiscProjectileSprite == null)
        {
            return;
        }

        // get total count of shapes in the sprite
        int shapeCount = MiscProjectileSprite.GetPhysicsShapeCount();

        // for each shape get its shape and add it to the list
        for (int i = 0; i < shapeCount; i++)
        {
            pathBuffer.Clear();
            MiscProjectileSprite.GetPhysicsShape(i, pathBuffer);
            PhysicsPaths.Add(pathBuffer.ToArray());
        }
    }
}
