using UnityEngine;
using UnityEngine.Events;

public interface I_Damagable 
{
    int Health {get; set;}
    int Damage(DamageContext damageContext);


}
public struct DamageContext{
    public bool criticalDamage;
    public UnityEvent onPersonHit;
}