using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.Splines;


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
    private Spline spline;
    private float bulletRotation;
    private Vector3 curTarget;
    private int currentKnotIndex = 0;
    private bool initialzedTarget = false;
    public bool DestroyOnHit { get; set; } = true;

    private void Awake()
    {
        bulletRB = gameObject.GetComponent<Rigidbody2D>();
        bulletSR = gameObject.GetComponent<SpriteRenderer>();
        bulletCollider = gameObject.GetComponent<PolygonCollider2D>();
    }

    // THIS IS WORK IN PROGRESS LIKE THE REST OF THE SPLINE SYSTEM PLZ REFRAIM FROM TOUCHING
    private void MoveAlongSpline()
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

            if (hasSpline) { MoveAlongSpline(); }
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
        if (projectileScriptableObject.HasSpline) { spline = projectileScriptableObject.Spline; }
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

            for (int i = 0; i < paths.Count; i++) { bulletCollider.SetPath(i, paths[i]); }
        }
        else
        {
            bulletCollider.pathCount = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var damageable = other.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(new DamageContext(damage, gameObject, other.gameObject));
        }

        OnBulletHit?.Invoke(this, other.gameObject);

        if (DestroyOnHit)
        {
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
