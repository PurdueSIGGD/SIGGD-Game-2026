using UnityEngine;
using System;

public class BombProjectile : MiscellaneousScriptableObject
{
    public float Timer;
    public float Radius;

    private void OnEnable()
    {
        function = Detonate;
    }

    public virtual void Detonate(MiscBullet bomb)
    {
        Debug.Log("Kaboom!");

        bomb.DisplayUseSprite(UseSprite);
    }
}