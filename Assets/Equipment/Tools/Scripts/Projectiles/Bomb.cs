using UnityEngine;
using System.Collections.Generic;

public class Bomb : ToolProjectile
{
    BombData bombData;

    public override void Init(ToolProjectileData data, Vector2 direction)
    {
        base.Init(data, direction);

        bombData = data as BombData;
        if (bombData == null)
        {
            Debug.LogError("Bomb needs a BombData asset, got " + data.name, this);
            Destroy(gameObject);
            return;
        }

        GetComponent<Rigidbody2D>().linearVelocity = direction * bombData.speed;
        Invoke(nameof(Explode), bombData.fuseTime);
    }

    // There needs to be way more nuance, like who the bomb damages, whether it destroys boss projectiles, etc.
    // So far this is just a simple bomb that damages and pushes objects away.
    void Explode()
    {
        Vector2 origin = transform.position;
        Collider2D[] hits = Physics2D.OverlapCircleAll(origin, bombData.blastRadius);
        HashSet<Rigidbody2D> pushed = new HashSet<Rigidbody2D>();
        HashSet<IDamageable> damaged = new HashSet<IDamageable>();

        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject == gameObject) continue;

            // Damage
            IDamageable target = hit.GetComponentInParent<IDamageable>();
            if (target != null && target.IsAlive && damaged.Add(target))
            {
                target.TakeDamage(new DamageContext
                {
                    DamageAmount = bombData.blastDamage,
                    Attacker = gameObject,
                    Victim = hit.GetComponentInParent<Component>().gameObject
                });
            }

            // Impulse
            Rigidbody2D body = hit.attachedRigidbody;
            if (body == null || body.bodyType != RigidbodyType2D.Dynamic) continue;
            if (!pushed.Add(body)) continue;

            float distance = Vector2.Distance(origin, hit.ClosestPoint(origin));
            float ratio = bombData.blastMinDistance / Mathf.Max(distance, bombData.blastMinDistance);
            float falloff = ratio * ratio;

            Vector2 direction = body.worldCenterOfMass - origin;
            direction = direction.sqrMagnitude > 0.0001f ? direction.normalized : Vector2.up;

            body.AddForce(direction * bombData.blastImpulse * falloff, ForceMode2D.Impulse);
        }


        // TODO: explosion effects
        Finish();
    }
}
