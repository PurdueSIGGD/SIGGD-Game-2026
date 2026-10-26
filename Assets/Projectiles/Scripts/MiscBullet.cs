using System.Collections.Generic;
using UnityEngine;

public class MiscBullet : MonoBehaviour
{
    private float lifetime;

    private Rigidbody2D miscRB;
    private SpriteRenderer miscSR;
    private PolygonCollider2D miscCollider;

    private void Awake()
    {
        miscRB = gameObject.GetComponent<Rigidbody2D>();
        miscSR = gameObject.GetComponent<SpriteRenderer>();
        miscCollider = gameObject.GetComponent<PolygonCollider2D>();
    }

    private void Update()
    {
        if (lifetime > 0)
        {
            lifetime -= 1f * Time.deltaTime;
        }
        else
        {
            ProjectileManager.Instance.MiscProjectilePool.Release(gameObject);
        }
    }
    public void PopulateMiscValues(MiscellaneousScriptableObject miscScriptableObject, Transform firePoint, Vector2 direction)
    {
        miscRB.angularVelocity = 0f;

        lifetime = miscScriptableObject.Lifetime;

        gameObject.transform.position = firePoint.position;

        // set sprite and collider
        miscSR.sprite = miscScriptableObject.MiscProjectileSprite;

        SetMiscCollider(miscScriptableObject);

        // set throw direction
        Vector2 finalDirection = Quaternion.Euler(0, 0, miscScriptableObject.SpriteAngleOffset) * direction;
        miscRB.rotation = Mathf.Atan2(finalDirection.y, finalDirection.x) * Mathf.Rad2Deg;

        // give speed
        miscRB.linearVelocity = finalDirection * miscScriptableObject.Velocity;
    }

    private void SetMiscCollider(MiscellaneousScriptableObject miscScriptableObject)
    {
        List<Vector2[]> paths = miscScriptableObject.PhysicsPaths;

        if (paths != null && paths.Count > 0)
        {
            miscCollider.pathCount = paths.Count;

            for (int i = 0; i < paths.Count; i++)
            {
                miscCollider.SetPath(i, paths[i]);
            }
        }
        else
        {
            miscCollider.pathCount = 0;
        }
    }
}
