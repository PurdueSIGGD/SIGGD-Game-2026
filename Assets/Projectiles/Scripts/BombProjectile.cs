using UnityEngine;

public class BombProjectile : MiscellaneousScriptableObject
{
    public float Timer;
    public float Radius;

    public virtual void Detonate()
    {
        Debug.Log("Kaboom!");
    }
}