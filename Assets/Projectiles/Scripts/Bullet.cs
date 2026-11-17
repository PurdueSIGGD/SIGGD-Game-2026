using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Splines;
using Unity.Mathematics;
using System;
using Unity.Collections;
using Unity.VisualScripting;

public class Bullet : MonoBehaviour
{
    public static UnityEvent<Bullet, GameObject> OnBulletHit;

    public bool DestroyOnHit { get; set; } = true;

    private bool isExplosive;
    private bool hasSpline;
    private int damage;
    private float lifetime;
    private Rigidbody2D bulletRB;
    private SpriteRenderer bulletSR;
    private PolygonCollider2D bulletCollider;
    private float distanceTravaledAlongSpline;  //maybe delete?
    private float velocity;
    private Transform firePoint;
    private Spline spline;
    private Vector3 shootDirection;
    private int currentKnotIndex = 0;
    private int nextKnotIndex;

    private bool hasHit = false; // track if bullet has hit something yet

    private float bulletRotation;
    private Vector3 curTarget;
    private bool initialzedTarget = false;

    private void Awake()
    {
        bulletRB = gameObject.GetComponent<Rigidbody2D>();
        bulletSR = gameObject.GetComponent<SpriteRenderer>();
        bulletCollider = gameObject.GetComponent<PolygonCollider2D>();
    }

    private void MoveAlongSpline()  //This method may or may not exist in the future, the logic, however, will. DO NOT DELETE
    {
        //  protects against "Index Out of Range" errors
        nextKnotIndex = (currentKnotIndex == spline.Count - 1) ? -69420 : currentKnotIndex + 1;
        if (nextKnotIndex == -69420) lifetime = 0;

        // set current and target knots
        BezierKnot currentKnot = spline[currentKnotIndex];  //add increment thingy to change later // incremnt the incremtnty thingy =)
        BezierKnot targetKnot = spline[nextKnotIndex];

        Vector2 currentKnotVec = new Vector2(currentKnot.Position.x, currentKnot.Position.y);
        Vector2 targetKnotVec = new Vector2(targetKnot.Position.x, targetKnot.Position.y);
        Vector2 velocityVec = new Vector2(velocity, velocity);

        Vector2 step = Vector2.SmoothDamp(currentKnotVec, targetKnotVec, ref velocityVec, 5.0f); // gets smooth directoin along a path

        Quaternion temp_rotation = new Quaternion(0, 0, 0, 0);

        gameObject.transform  //moves the bullet?
            .SetPositionAndRotation(new Vector3(gameObject.transform.position.x + step.x, gameObject.transform.position.y + step.y), temp_rotation);

        // dev notes DO NOT DELETE
        // add logic that incremnts bullet along spline nodes, no while loop, need to have current node and next node and reset once next node is hit.
        // gameObject.transform allows us to get the x and y pos
    }

    // THIS IS WORK IN PROGRESS LIKE THE REST OF THE SPLINE SYSTEM PLZ REFRAIM FROM TOUCHING
    private void MoveAlongSpline2()
    {
        // get knot at index
        BezierKnot curKnot = spline[currentKnotIndex];

        int nextKnotIndex = currentKnotIndex + 1;

        // if we are at end of spline reset to start
        if (nextKnotIndex >= spline.Count)
        {
            currentKnotIndex = 0;
            curKnot = spline[currentKnotIndex];
            nextKnotIndex = currentKnotIndex + 1;
        }

        BezierKnot nextKnot = spline[nextKnotIndex];

        // set the target at the beginning of moving one spline to another since the target wont change at all until the bullet reaches it
        if (initialzedTarget == false)
        {
            float cos = Mathf.Cos(bulletRotation);
            float sin = Mathf.Sin(bulletRotation);

            float xMoveAmount = -((nextKnot.Position.x * cos - nextKnot.Position.y * sin) - (curKnot.Position.x * cos - curKnot.Position.y * sin));
            float yMoveAmount = -((nextKnot.Position.x * sin + nextKnot.Position.y * cos) - (curKnot.Position.x * sin + curKnot.Position.y * cos));

            // move the actual bullet by the move amount
            curTarget = new Vector3(transform.position.x + xMoveAmount, transform.position.y + yMoveAmount, 0);

            initialzedTarget = true;
        }
        // calculate how much it should move based on velocity then move it
        float step = velocity * Time.deltaTime;

        Vector3 moveVector = Vector3.MoveTowards(transform.position, curTarget, step); // cpuld swap this for a smoothdamp/lerp to give a more smooth feeling

        gameObject.transform.position = new Vector3(moveVector.x, moveVector.y, 0);

        // now that the move has happened check if the current position is at the nextKnot position and increment index if so
        if ((gameObject.transform.position - curTarget).sqrMagnitude < 0.0001f)
        {
            currentKnotIndex += 1;
            initialzedTarget = false;
        }
    }

    private void Update()
    {
        if (lifetime > 0)
        {
            lifetime -= 1f * Time.deltaTime;

            if (hasSpline)
            {
                MoveAlongSpline2();
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
    public void PopulateBulletValues(ProjectileScriptableObject projectileScriptableObject, Transform firePoint, Vector2 shootDirection, LayerMask layerToHit, LayerMask layerToIgnore)
    {
        currentKnotIndex = 0;
        initialzedTarget = false;

        bulletRB.angularVelocity = 0f; // reset angular velocity just incase its not 0 before firing

        isExplosive = projectileScriptableObject.IsExplosive;
        hasSpline = projectileScriptableObject.HasSpline;
        damage = projectileScriptableObject.Damage;
        lifetime = projectileScriptableObject.Lifetime;
        velocity = projectileScriptableObject.Velocity;
        hasHit = false;
        this.firePoint = firePoint;
        this.shootDirection = shootDirection;

        gameObject.transform.position = firePoint.position;

        // set bullet sprite
        bulletSR.sprite = projectileScriptableObject.ProjectileSprite;
        bulletSR.color = projectileScriptableObject.ProjectileColor;

        gameObject.transform.localScale = projectileScriptableObject.ProjectileScale;

        SetBulletCollider(projectileScriptableObject, layerToHit, layerToIgnore);

        // rotate bullet
        Vector2 finalDirection = Quaternion.Euler(0, 0, projectileScriptableObject.SpriteAngleOffset) * shootDirection;
        bulletRB.rotation = Mathf.Atan2(finalDirection.y, finalDirection.x) * Mathf.Rad2Deg;
        bulletRotation = firePoint.eulerAngles.z * Mathf.Deg2Rad;

        // if have a spline thatll determine velocity and shooting stuff so set rigidbody velocity to 0 this will totally not cause unexpected issues later what are you talking about
        if (projectileScriptableObject.HasSpline)
        {
            spline = projectileScriptableObject.Spline;
        }
        else
        {
            // give bullet its speed and in the direction
            bulletRB.linearVelocity = finalDirection * projectileScriptableObject.Velocity;
        }
    }

    private void SetBulletCollider(ProjectileScriptableObject projectileScriptableObject, LayerMask layerToHit, LayerMask layerToIgnore)
    {
        List<Vector2[]> paths = projectileScriptableObject.PhysicsPaths;

        bulletCollider.includeLayers = layerToHit;
        bulletCollider.excludeLayers = layerToIgnore;

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit) return;

        IDamageable damageable = other.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(new DamageContext(damage, gameObject, other.gameObject));
        }

        OnBulletHit?.Invoke(this, other.gameObject);

        if (DestroyOnHit)
        {
            hasHit = true;

            ProjectileManager.Instance.ProjectilePool.Release(gameObject);
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
