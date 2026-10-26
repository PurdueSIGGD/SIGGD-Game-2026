using Player;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

// this script should be attatched to the player object! - ryan :P

public class PrimaryGun : MonoBehaviour
{
    public PrimaryWeapon Weapon;
    public Transform FirePoint;
    public Transform WeaponHolderTransfrom;

    private GameObject curWeaponVisual;

    public Rigidbody2D PlayerBody;

    public int CurrentAmmo;
    public bool IsReloading;

    private float nextFireTime;
    private float reloadAccumulator;
    private Coroutine reloadRoutine;

    private void Awake()
    {
        if (FirePoint == null) FirePoint = transform;
        if (PlayerBody == null) PlayerBody = GetComponent<Rigidbody2D>();
        if (curWeaponVisual == null) curWeaponVisual = Weapon.WeaponVisualPrefab;

        InputManager.Instance.OnPlayerLeftClick.AddListener(Fire);
    }

    private void Start()
    {
        if (Weapon != null)
        {
            CurrentAmmo = Weapon.ammoCapacity;
        }
        else
        {
            Debug.LogWarning("dude put a gun in here");
        }
    }

    private void Update()
    {
        if (Weapon == null || Mouse.current == null) return;
    }

    private void Fire(Vector2 targetPos)
    {
        if (CurrentAmmo <= 0 || Time.time < nextFireTime) return;

        Vector2 mouseScreenPosition = InputManager.Instance.PointerScreenPosition;
        float distanceToCamera = FirePoint.position.z - Camera.main.transform.position.z;
        targetPos = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, distanceToCamera));

        nextFireTime = Time.time + 1f / Mathf.Max(Weapon.fireSpeed, 0.01f);
        Vector2 aimDir = ((Vector2)((Vector3)targetPos - FirePoint.position)).normalized;
        if (aimDir == Vector2.zero) aimDir = Vector2.right;

        int count = Mathf.Max(1, Weapon.numBullets);
        for (int i = 0; i < count; i++)
        {
            Vector2 shotDir = GetShotDirection(aimDir, i, count);
            ProjectileManager.Instance.ShootProjectileTowardsDirection(Weapon.projectile, FirePoint, shotDir);
        }
        CurrentAmmo--;
        ApplyRecoil(aimDir);

        StartReload();
    }

    private Vector2 GetShotDirection(Vector2 aimDir, int index, int count)
    {
        float half = Weapon.spread * 0.5f;
        float spreadAngle;

        if (count == 1)
        {
            spreadAngle = UnityEngine.Random.Range(-half, half);
        }
        else
        {
            float t = (float)index / (count - 1);
            spreadAngle = Mathf.Lerp(-half, half, t);
        }

        float offsetAngle = UnityEngine.Random.Range(-Weapon.offset, Weapon.offset);

        return Quaternion.Euler(0f, 0f, spreadAngle + offsetAngle) * aimDir;
    }

    private void ApplyRecoil(Vector2 aimDir)
    {
        if (PlayerBody == null || Weapon.recoil <= 0f) return;
        PlayerBody.AddForce(-aimDir * Weapon.recoil, ForceMode2D.Impulse);
    }

    public void StartReload()
    {
        if (IsReloading || Weapon == null || CurrentAmmo >= Weapon.ammoCapacity) return;
        reloadRoutine = StartCoroutine(ReloadRoutine());
    }

    private IEnumerator ReloadRoutine()
    {
        IsReloading = true;
        reloadAccumulator = 0f;

        while (CurrentAmmo < Weapon.ammoCapacity)
        {
            float ammoPerSecond = Weapon.ammoCapacity / Mathf.Max(Weapon.reloadTime, 0.01f);
            reloadAccumulator += ammoPerSecond * Time.deltaTime;

            int wholeAmmo = Mathf.FloorToInt(reloadAccumulator);
            if (wholeAmmo > 0)
            {
                CurrentAmmo = Mathf.Min(CurrentAmmo + wholeAmmo, Weapon.ammoCapacity);
                reloadAccumulator -= wholeAmmo;
            }

            yield return null;
        }

        reloadAccumulator = 0f;
        IsReloading = false;
        reloadRoutine = null;
    }

    // Call this script every time the player's weapon is changed! (please!!! ty) - ryan <3
    public void EquipWeapon(PrimaryWeapon newWeapon)
    {
        if (newWeapon == null) return;

        if (reloadRoutine != null)
        {
            StopCoroutine(reloadRoutine);
            reloadRoutine = null;
        }
        IsReloading = false;
        reloadAccumulator = 0f;

        Weapon = newWeapon;
        CurrentAmmo = Weapon.ammoCapacity;

        // spawn and update new weapon visual
        if(curWeaponVisual != null)
        {
            Destroy(curWeaponVisual);
        }

        if (Weapon.WeaponVisualPrefab != null && WeaponHolderTransfrom != null)
        {
            curWeaponVisual = Instantiate(Weapon.WeaponVisualPrefab, WeaponHolderTransfrom);
            curWeaponVisual.transform.localPosition = Vector3.zero; // set position at zero of the gun holder
            curWeaponVisual.transform.localRotation = Quaternion.Euler(0, 0, Weapon.WeaponVisualRotationOffset);

            // quick lil thing to make sure if firepoint is deleted bullets still work
            if (FirePoint == null)
            {
                FirePoint = WeaponHolderTransfrom;
            }
        }
    }

    private void OnDisable()
    {
        IsReloading = false;
        reloadAccumulator = 0f;
        reloadRoutine = null;
    }
}