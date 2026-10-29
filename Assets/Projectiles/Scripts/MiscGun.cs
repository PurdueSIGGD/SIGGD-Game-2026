using UnityEngine;
using System.Collections;

public class MiscGun : MonoBehaviour
{
    // put this script with the PrimaryGun script like on the same object
    public MiscWeapon MiscWeapon;
    public Transform FirePoint;
    private Transform weaponHolderTransform;

    [SerializeField] private GameObject curMiscWeaponVisual;
    private GameObject miscBulletReference;

    private Coroutine miscOnCooldown;
    private Coroutine miscIsThrown;

    private void Awake()
    {
        // man im so excited to rewrite all of this as soon as the input manager is fixed :D
        InputManager.Instance.OnPlayerRightClick.AddListener(DestroyMisc);
        InputManager.Instance.OnPlayerRightClick.AddListener(ShootMisc);

        InputManager.Instance.OnUseMiscItem.AddListener(UseMisc);

        weaponHolderTransform = gameObject.transform;
    }

    private void Start()
    {
        // just for now i this will be replaced later with the player actaully buying a misc
        EquipMisc(MiscWeapon);
    }

    private void ShootMisc(Vector2 shootPos)
    {
        if (miscOnCooldown != null) return;

        // safe the reference to the object returned so that we can repool it later when activated if needed
        miscBulletReference = ProjectileManager.Instance.ShootMiscTowardsMouse(MiscWeapon.MiscProjectile, FirePoint);

        miscOnCooldown = StartCoroutine(MiscOnCooldown());
        miscIsThrown = StartCoroutine(MiscIsThrown());
    }
    
    private void DestroyMisc(Vector2 notUsedInThisFunctionIJustNeedToPassThisToGetRightClickInput)
    {
        if (miscIsThrown == null) return;

        StopCoroutine(miscIsThrown);
        miscIsThrown = null;

        ProjectileManager.Instance.MiscProjectilePool.Release(miscBulletReference);
        miscBulletReference = null;

        // release the fired game object
    }

    // does the activated ability of the misc item like bomb exploding 
    private void UseMisc(Vector2 notUsedInThisFunctionIJustNeedToPassThisToGetEInput)
    {
        if (miscIsThrown == null) return;
        // this will probably have to check whether the secondary is grapple or not and then do a reel in or explosion/magnet aura kinda thing
        MiscWeapon.Use(miscBulletReference);

        // then we will either have to delete it or not depending on which misc object it is perhapsn misc weapon can return a bool for if we should destroy it or not?
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
            //Destroy(curMiscWeaponVisual);
        }

        if (MiscWeapon.MiscWeaponVisualPrefab != null && weaponHolderTransform != null)
        {
            curMiscWeaponVisual = Instantiate(MiscWeapon.MiscWeaponVisualPrefab, weaponHolderTransform);
            curMiscWeaponVisual.transform.localPosition = Vector3.zero;
            curMiscWeaponVisual.transform.localRotation = Quaternion.Euler(0, 0, MiscWeapon.MiscWeaponVisualRotationOffset);

            if (FirePoint == null)
            {
                FirePoint = weaponHolderTransform;
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
