using UnityEngine;
using System.Collections;

public class MiscGun : MonoBehaviour
{
    // put this script with the PrimaryGun script like on the same object
    public MiscWeapon MiscWeapon;
    public Transform FirePoint;
    public Transform WeaponHolderTransfrom;

    private GameObject curMiscWeaponVisual;

    private Coroutine miscOnCooldown;
    private Coroutine miscIsThrown;

    private void Awake()
    {
        InputManager.Instance.OnPlayerRightClick.AddListener(ShootMisc);
        InputManager.Instance.OnPlayerRightClick.AddListener(DestroyMisc);
    }

    private void ShootMisc(Vector2 shootPos)
    {
        if (miscOnCooldown != null) return;

        ProjectileManager.Instance.ShootMiscTowardsMouse(MiscWeapon.MiscProjectile, FirePoint);
        miscOnCooldown = StartCoroutine(MiscOnCooldown());
        miscIsThrown = StartCoroutine(MiscIsThrown());
    }

    private void DestroyMisc(Vector2 notUsedInThisFunctionIJustNeedToPassThisToGetRightClickInput)
    {
        if (miscIsThrown == null) return;

        StopCoroutine(MiscIsThrown());

        // release the fired game object
    }

    private void ActivateMisc(Vector2 shootPos)
    {
        if (miscIsThrown != null) return;
        // this will probably have to check whether the secondary is grapple or not and then do a reel in or explosion/magnet aura kinda thing
    }

    public void EquipMisc(MiscWeapon newMisc)
    {
        if (newMisc == null) return;

        MiscWeapon = newMisc;

        if (miscOnCooldown != null)
        {
            StopCoroutine(miscOnCooldown);
            miscOnCooldown = null;
        }

        if (miscIsThrown != null)
        {
            StopCoroutine(miscIsThrown);
            miscIsThrown = null;
        }

        if (curMiscWeaponVisual != null)
        {
            Destroy(curMiscWeaponVisual);
        }

        if (MiscWeapon.MiscWeaponVisualPrefab != null && WeaponHolderTransfrom != null)
        {
            curMiscWeaponVisual = Instantiate(MiscWeapon.MiscWeaponVisualPrefab, WeaponHolderTransfrom);
            curMiscWeaponVisual.transform.localPosition = Vector3.zero;
            curMiscWeaponVisual.transform.localRotation = Quaternion.Euler(0, 0, MiscWeapon.MiscWeaponVisualRotationOffset);

            if (FirePoint == null)
            {
                FirePoint = WeaponHolderTransfrom;
            }
        }
    }

    private IEnumerator MiscOnCooldown()
    {
        yield return new WaitForSeconds(MiscWeapon.Cooldown);

        miscOnCooldown = null;
    }

    private IEnumerator MiscIsThrown()
    {
        yield return new WaitForSeconds(MiscWeapon.MiscProjectile.Lifetime);

        miscIsThrown = null;
    }
}
