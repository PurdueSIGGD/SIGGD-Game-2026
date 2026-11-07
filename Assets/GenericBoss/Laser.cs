using System;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private DamageContext _damageContext;

    private List<IDamageable> _damageablesInRange = new List<IDamageable>();

    private float timer = 0.0f;

    private float lifetime = 0.0f;
    private bool init = false;

    public void InitLaser(float length, float angle, Vector2 origin, float damage, float lifetime)
    {
        transform.position = new Vector3(origin.x + length * 0.5f * Mathf.Cos(angle * Mathf.PI/180f), origin.y + length * 0.5f * Mathf.Sin(angle * Mathf.PI/180f), 0);
        transform.rotation = Quaternion.Euler(0, 0, angle);
        transform.localScale = new Vector3(length, 1, 1);
        _damageContext.DamageAmount = damage;
        _damageContext.Attacker = this.gameObject;
        this.lifetime = lifetime;
        init = true;
    }

    private void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0.0f && init)
        {
            Destroy(gameObject);
        }

        timer += Time.deltaTime;
        if (timer > 1)
        {
            timer = 0.0f;
            DamageAll();
        }
    }

    private void DamageAll()
    {
        for (int i = 0; i < _damageablesInRange.Count; i++)
        {
            _damageablesInRange[i].TakeDamage(_damageContext);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
        _damageablesInRange.Add(damageable);
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
        _damageablesInRange.Remove(damageable);
    }
}
