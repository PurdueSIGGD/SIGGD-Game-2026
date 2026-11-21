using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.Splines;
using Unity.Mathematics;


public class Bullet : MonoBehaviour
{
    public static UnityEvent<Bullet, GameObject> OnBulletHit;
    private bool isExplosive;
    private bool hasSpline;
    private int damage;
    private float lifetime;
    private Rigidbody2D bulletRB;
    private SpriteRenderer bulletSR;
    private PolygonCollider2D bulletCollider;
    private float velocity;
    private SplineContainer splineContainer;
    private float bulletRotation;

    private bool hasHit = false; // track if bullet has hit something yet

    // TODO after spline refactor fix the uncoming bug with values not getting reset when the bullet is pooled. Stuff like layers and this spline stuff will need to get reset to default when bullet re pools itself and also move the velocity cancel stuff from ProjectileManager to here

    private int activeSplineIndex;
    private float splineProgress;
    private Vector3 curAnchorPosition;
    private Quaternion rotationQuat;

    public bool DestroyOnHit { get; set; } = true;

    private void Awake()
    {
        bulletRB = gameObject.GetComponent<Rigidbody2D>();
        bulletSR = gameObject.GetComponent<SpriteRenderer>();
        bulletCollider = gameObject.GetComponent<PolygonCollider2D>();
    }
    

    private void MoveAlongSpline()
    {
        Spline activeSpline = splineContainer.Splines[activeSplineIndex];
        float splineLength = activeSpline.GetLength();

        // if the spline has no length then dont move the bullet
        if (splineLength <= 0)
        {
            transform.position = curAnchorPosition;
            return;
        }
        // might have to add check for length being 0 but for not now having it so that in theory a full line splie could chain into one that moves between 0,0,0 and 0,0,0 so that it stays stationary

        // advance progress
        splineProgress += (velocity * Time.deltaTime) / splineLength;

        // if spline is complete
        if (splineProgress >= 1)
        {
            float overflowProgress = splineProgress - 1;

            Vector3 localEndPos = activeSpline.EvaluatePosition(1);
            Vector3 rotatedEndPos = rotationQuat * localEndPos;

            // if theres 1 spline
            if (splineContainer.Splines.Count == 1)
            {
                // just move offset and kwwp moving along spline
                curAnchorPosition += rotatedEndPos;
                splineProgress = 0;
            }
            // if theres 2 splines
            else if (splineContainer.Splines.Count == 2)
            {
                // if we are on first spline of 2 add offset position and set index to 1
                if (activeSplineIndex == 0)
                {
                    curAnchorPosition += rotatedEndPos;
                    splineProgress = 0;
                    activeSplineIndex = 1;
                }
                // if we are on the second spline it should just loop so reset progress
                else
                {
                    splineProgress = 0;
                }
            }

            // incrase active spline index was changed reset active spline
            activeSpline = splineContainer.Splines[activeSplineIndex];
        }

        // moving object along spline
        // get local position
        Vector3 localPos = activeSpline.EvaluatePosition(splineProgress);
        Vector3 localTan = activeSpline.EvaluateTangent(splineProgress);

        // rotate it in the correct direction
        Vector3 worldOffset = rotationQuat * localPos;
        Vector3 worldTan = rotationQuat * localTan;

        transform.position = curAnchorPosition + worldOffset;
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(worldTan.y, worldTan.x) * Mathf.Rad2Deg);
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
        hasHit = false;

        gameObject.transform.position = firePoint.position;

        // set bullet sprite
        bulletSR.sprite = projectileScriptableObject.ProjectileSprite;
        bulletSR.color = projectileScriptableObject.ProjectileColor;

        gameObject.transform.localScale = projectileScriptableObject.ProjectileScale;

        gameObject.layer = projectileScriptableObject.BulletLayer;
        SetBulletCollider(projectileScriptableObject);

        // rotate bullet
        Vector2 finalDirection = Quaternion.Euler(0, 0, projectileScriptableObject.SpriteAngleOffset) * shootDirection;
        bulletRotation = (Mathf.Atan2(finalDirection.y, finalDirection.x) * Mathf.Rad2Deg); // may need a -90 degree offset cuz im assuming people want to design down to up since thats how the projectiles are gonna be moving in game
        bulletRB.rotation = bulletRotation;

        curAnchorPosition = transform.position;

        rotationQuat = Quaternion.Euler(0f, 0f, bulletRotation);
        activeSplineIndex = 0;
        splineProgress = 0f;

        // if have a spline thatll determine velocity and shooting stuff so set rigidbody velocity to 0 this will totally not cause unexpected issues later what are you talking about
        if (projectileScriptableObject.HasSpline)
        {
            splineContainer = projectileScriptableObject.SplineContainer;
        }
        else
        {
            // give bullet its speed and in the direction
            bulletRB.linearVelocity = finalDirection * projectileScriptableObject.Velocity;
        }
    }

    private void SetBulletCollider(ProjectileScriptableObject projectileScriptableObject)
    {
        List<Vector2[]> paths = projectileScriptableObject.PhysicsPaths;

        bulletCollider.excludeLayers = projectileScriptableObject.CollisionExcludes;

        if (paths != null && paths.Count > 0)
        {
            bulletCollider.pathCount = paths.Count;

            for (int i = 0; i < paths.Count; i++) { bulletCollider.SetPath(i, paths[i]); }
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

    public bool GetIsExplosive() { return isExplosive; }

    public int GetDamage() { return damage; }

    public float GetLifetime() { return lifetime; }

    public void SetIsExplosive(bool inIsExplosive) { isExplosive = inIsExplosive; }

    public void SetDamage(int inDamage) { damage = inDamage; }

    public void SetLifetime(float inLifetime) { lifetime = inLifetime; }
}
