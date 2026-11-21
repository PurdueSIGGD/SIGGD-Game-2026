using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Splines;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ProjectileScriptableObject", menuName = "Scriptable Objects/ProjectileScriptableObject")]
public class ProjectileScriptableObject : ScriptableObject 
{
    public bool IsExplosive;
    public int Damage;
    public float Lifetime;
    public float Velocity;
    [FormerlySerializedAs("ProjectileAngleOffset")]
    public float SpriteAngleOffset;
    // offsets the bullet used when attack are being fired to a point but need to be a little offset think like a shotgun or a boss shooting waves of attacks at different angles
    // just tweak this value from the gun script to change how the bullet is rotated

    public Sprite ProjectileSprite; // sprite for projectile
    public Color32 ProjectileColor = new Color32(255, 255, 255, 255); // in case we wanna change the color
    public Vector3 ProjectileScale = new Vector3(1, 1, 1); // so that we can scale up and scale down bullets for different attack and have it be done in scriptable object instead of in game

    public LayerMask CollisionExcludes; // what this bullet should not collide with when shot
    [SerializeField] private LayerMask BulletLayerSelector; // what layer the bullet should be on
    [HideInInspector] public int BulletLayer; // the actual layer the bullet is on which other things access

    // Spline stuff
    public bool HasSpline;
    public SplineContainer SplineContainer;  // The holder of the spline that bullet script will use to trace the bullet pattern

    // everything from here downward used the sprite assigned to generate a polygoncollider so that only one collider has to be made and then the sprite and collider are just applied 
    public List<Vector2[]> PhysicsPaths = new List<Vector2[]>();
    private static readonly List<Vector2> pathBuffer = new List<Vector2>();

    private void OnValidate()
    {
        // clean list in between updates in the scriptable object
        PhysicsPaths.Clear();

        if (ProjectileSprite == null)
        {
            return;
        }

        // get total count of shapes in the sprite
        int shapeCount = ProjectileSprite.GetPhysicsShapeCount();

        // for each shape get its shape and add it to the list
        for (int i = 0; i < shapeCount; i++)
        {
            pathBuffer.Clear();
            ProjectileSprite.GetPhysicsShape(i, pathBuffer);
            PhysicsPaths.Add(pathBuffer.ToArray());
        }

        // use the stored spline object to get the spline container
        HasSpline = false;

        if (SplineContainer != null)
        {
            // check to make sure the spline container has at least one spline
            if (SplineContainer[0] != null)
            {
                HasSpline = true;
            }
            else
            {
                Debug.LogError("Given SplineContainer doesn't contain any splines");
            }
        }

        if ((BulletLayerSelector.value & (BulletLayerSelector.value - 1)) != 0)
        {
            Debug.LogError("Multiple layers selected for a projectiles layer. Ensure only on layer is selected in the BulletLayerSelector.");
            BulletLayer = 0; // set to default so that it does hold a value it just wont work
        } 
        else
        {
            BulletLayer = Mathf.RoundToInt(Mathf.Log(BulletLayerSelector.value, 2));
        }
    }
}