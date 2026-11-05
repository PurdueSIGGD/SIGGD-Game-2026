using UnityEngine;
using System;

public class BombProjectile : MiscellaneousScriptableObject
{
    public float Timer;
    public float Width;

    private void OnEnable()
    {
        function = Detonate;
    }

    public virtual void Detonate(MiscBullet bomb)
    {
        Debug.Log("Kaboom! Explodes and extends " + Width + " after " + Timer + " seconds.");

        bomb.DisplayUseSprite(UseSprite);
    }
}