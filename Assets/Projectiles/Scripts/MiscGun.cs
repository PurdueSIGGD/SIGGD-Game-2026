using UnityEngine;
using System.Collections;

public class MiscGun : MonoBehaviour
{
    // put this script with the PrimaryGun script like on the same object
    public MiscObject MiscObject;
    public Transform FirePoint;
    [SerializeField] private bool onCooldown;
    [SerializeField] private bool miscThrown;

    private void Awake()
    {
        onCooldown = false;
        miscThrown = false;

        InputManager.Instance.OnPlayerRightClick.AddListener(ShootMisc);
        InputManager.Instance.OnPlayerRightClick.AddListener(DestroyMisc);
    }

    private void ShootMisc(Vector2 shootPos)
    {
        if (onCooldown == true) return;

        ProjectileManager.Instance.ShootMiscTowardsMouse(MiscObject.MiscProjectile, FirePoint);
        StartCoroutine(MiscOnCooldown());
        StartCoroutine(MiscIsThrown());
    }

    private void DestroyMisc(Vector2 notUsedInThisFunctionIJustNeedToPassThisToGetRightClickInput)
    {
        if (miscThrown == false) return;

        StopCoroutine(MiscIsThrown());
        miscThrown = false;
    }

    private void ActivateMisc(Vector2 shootPos)
    {
        if (miscThrown == false) return;
        // this will probably have to check whether the secondary is grapple or not and then do a reel in or explosion/magnet aura kinda thing
    }

    private IEnumerator MiscOnCooldown()
    {
        onCooldown = true;
        yield return new WaitForSeconds(MiscObject.Cooldown);
        onCooldown = false;
    }

    private IEnumerator MiscIsThrown()
    {
        miscThrown = true;
        yield return new WaitForSeconds(MiscObject.MiscProjectile.miscProjectileBehavior.Lifetime);
        miscThrown = false;
    }
}
