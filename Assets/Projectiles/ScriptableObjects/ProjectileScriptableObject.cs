using UnityEngine;
using UnityEngine.Splines;
using System.Collections.Generic;
using TreeEditor;

[CreateAssetMenu(fileName = "ProjectileScriptableObject", menuName = "Scriptable Objects/ProjectileScriptableObject")]
public class ProjectileScriptableObject : ScriptableObject 
{
    public bool IsExplosive;
    public bool HasSpline;
    public int Damage;
    public float Lifetime;
    public float Velocity;
    public float SpriteAngleOffset;
    // offsets the bullet used when attack are being fired to a point but need to be a little offset think like a shotgun or a boss shooting waves of attacks at different angles
    // just tweak this value from the gun script to change how the bullet is rotated

    public Sprite ProjectileSprite; // sprite for projectile
    public Color32 ProjectileColor = new Color32(255, 255, 255, 255); // in case we wanna change the color
    public Vector3 ProjectileScale = new Vector3(1, 1, 1); // so that we can scale up and scale down bullets for different attack and have it be done in scriptable object instead of in game

    //Spline stuff
    public GameObject SplineGameObject;  // This gets the spline that was drawn in the engine.

    public SplineContainer SplineContainer;  // This is the ultiment destination of the Spline

    public List<Spline> SplineNodes;

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

        /*
        SplineContainer = SplineGameObject.GetComponent<SplineContainer>();
        SplineNodes.Clear();

        // adds the spline game object componets to the spline nodes list
        for (int i = 0; i < SplineContainer.Splines.Count; i++)
        {
            SplineNodes.Add(SplineContainer.Splines[i]);
        }
        */
    }
}