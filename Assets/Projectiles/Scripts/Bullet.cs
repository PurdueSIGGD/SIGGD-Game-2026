using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Splines;
using Unity.Mathematics;
using System;
using Unity.Collections;

public class Bullet : MonoBehaviour
{
    private bool isExplosive;
    private bool hasSpline;
    private int damage;
    private float lifetime;
    private Rigidbody2D bulletRB;
    private SpriteRenderer bulletSR;
    private PolygonCollider2D bulletCollider;
    private float distanceTravaledAlongSpline;
    private float velocity;
    private Transform firePoint;
    private Spline spline;
    private Vector3 shootDirection;
    private int currentKnotIndex = 0;
    private int nextKnotIndex;

    private void Awake()
    {
        bulletRB = gameObject.GetComponent<Rigidbody2D>();
        bulletSR = gameObject.GetComponent<SpriteRenderer>();
        bulletCollider = gameObject.GetComponent<PolygonCollider2D>();
        
    }

    private void MoveAlongSpline() 
    {
        //  protects against Index Out of Range errors
        nextKnotIndex = (currentKnotIndex == spline.Count - 1) ? -69420 : currentKnotIndex + 1;
        if (nextKnotIndex == -69420) lifetime = 0;

        // set current and target knots
        BezierKnot currentKnot = spline[currentKnotIndex];  //add increment thingy to change later // incremnt the incremtnty thingy =)
        BezierKnot targetKnot = spline[nextKnotIndex];

        


        // dev notes DO NOT DELETE
        // add logic that incremnts bullet along spline nodes, no while loop, need to have current node and next node and reset once next node is hit.
        // gameObject.transform allows us to get the x and y pos
        // Vector3.SmoothDamp(x, y, 0); // gets smooth directoin along a path
    }
    private void Update()
    {
        if (lifetime > 0)
        {
            lifetime -= 1f * Time.deltaTime;

            if (hasSpline)
            {
                MoveAlongSpline();
                
            }
        }
        else
        {
            if (isExplosive)
            {
                // call some sort of blow up function eventually when that gets made
            }

            ProjectileManager.Instance.ProjectilePool.Release(gameObject);
        }
    }

    /// <summary>
    /// Set the sprite, position, rotation and velocity of the bullet as well as the basic bullet values gotten from the ProjectilScriptableObject
    /// </summary>
    public void PopulateBulletValues(ProjectileScriptableObject projectileScriptableObject, Transform firePoint, Vector2 shootDirection)
    {
        bulletRB.angularVelocity = 0f; // reset angular velocity just incase its not 0 before firing

        isExplosive = projectileScriptableObject.IsExplosive;
        hasSpline = projectileScriptableObject.HasSpline; 
        damage = projectileScriptableObject.Damage;
        lifetime = projectileScriptableObject.Lifetime;
        velocity = projectileScriptableObject.Velocity;
        this.firePoint = firePoint;
        this.shootDirection = shootDirection;

        if (projectileScriptableObject.HasSpline)
        {
            spline = projectileScriptableObject.Spline;
        }

        gameObject.transform.position = firePoint.position;

        // set bullet sprite
        bulletSR.sprite = projectileScriptableObject.ProjectileSprite;
        bulletSR.color = projectileScriptableObject.ProjectileColor;

        gameObject.transform.localScale = projectileScriptableObject.ProjectileScale;

        SetBulletCollider(projectileScriptableObject);

        // rotate bullet
        Vector2 finalDirection = Quaternion.Euler(0, 0, projectileScriptableObject.SpriteAngleOffset) * shootDirection;
        bulletRB.rotation = Mathf.Atan2(finalDirection.y, finalDirection.x) * Mathf.Rad2Deg;

        // give bullet its speed and in the direction
        bulletRB.linearVelocity = finalDirection * projectileScriptableObject.Velocity;
    }

    private void SetBulletCollider(ProjectileScriptableObject projectileScriptableObject)
    {
        List<Vector2[]> paths = projectileScriptableObject.PhysicsPaths;

        if (paths != null && paths.Count > 0)
        {
            bulletCollider.pathCount = paths.Count;

            for (int i = 0; i < paths.Count; i++)
            {
                bulletCollider.SetPath(i, paths[i]);
            }
        }
        else
        {
            bulletCollider.pathCount = 0;
        }
    }

    public bool GetIsExplosive()
    {
        return isExplosive;
    }

    public int GetDamage()
    {
        return damage;
    }

    public float GetLifetime()
    {
        return lifetime;
    }

    public void SetIsExplosive(bool inIsExplosive)
    {
        isExplosive = inIsExplosive;
    }

    public void SetDamage(int inDamage)
    {
        damage = inDamage;
    }
    public void SetLifetime(float inLifetime)
    {
        lifetime = inLifetime;
    }
}
