using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

// this script should be attatched to the player object! - ryan :P

public class PrimaryGun : MonoBehaviour
{
    public Weapon weapon;
    public Transform firePoint;
    public Rigidbody2D playerBody;
    public int currentAmmo;
    public bool isReloading;
    private float nextFireTime;
    private float reloadAccumulator;
    private Coroutine reloadRoutine;

    private void Awake()
    {
        if (firePoint == null) firePoint = transform;
        if (playerBody == null) playerBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (weapon != null)
        {
            currentAmmo = weapon.ammoCapacity;
        }
        else
        {
            Debug.LogWarning("dude put a gun in here");
        }
    }

    private void Update()
    {
        if (weapon == null || Mouse.current == null) return;

        bool wantsToFire = Mouse.current.leftButton.isPressed;
        if (wantsToFire && currentAmmo > 0 && Time.time >= nextFireTime)
        {
            Fire();
        }
    }

    private void Fire()
    {
        nextFireTime = Time.time + 1f / Mathf.Max(weapon.fireSpeed, 0.01f);
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mouseWorld.z = 0f;
        Vector2 aimDir = ((Vector2)(mouseWorld - firePoint.position)).normalized;
        if (aimDir == Vector2.zero) aimDir = Vector2.right;

        int count = Mathf.Max(1, weapon.numBullets);
        for (int i = 0; i < count; i++)
        {
            Vector2 shotDir = GetShotDirection(aimDir, i, count);
            ProjectileManager.Instance.ShootProjectileTowardsDirection(weapon.projectile, firePoint, shotDir);
        }
        currentAmmo--;
        ApplyRecoil(aimDir);

        StartReload();
    }

    private Vector2 GetShotDirection(Vector2 aimDir, int index, int count)
    {
        float half = weapon.spread * 0.5f;
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

        float offsetAngle = UnityEngine.Random.Range(-weapon.offset, weapon.offset);

        return Quaternion.Euler(0f, 0f, spreadAngle + offsetAngle) * aimDir;
    }

    private void ApplyRecoil(Vector2 aimDir)
    {
        if (playerBody == null || weapon.recoil <= 0f) return;
        playerBody.AddForce(-aimDir * weapon.recoil, ForceMode2D.Impulse);
    }

    public void StartReload()
    {
        if (isReloading || weapon == null || currentAmmo >= weapon.ammoCapacity) return;
        reloadRoutine = StartCoroutine(ReloadRoutine());
    }

    private IEnumerator ReloadRoutine()
    {
        isReloading = true;
        reloadAccumulator = 0f;

        while (currentAmmo < weapon.ammoCapacity)
        {
            float ammoPerSecond = weapon.ammoCapacity / Mathf.Max(weapon.reloadTime, 0.01f);
            reloadAccumulator += ammoPerSecond * Time.deltaTime;

            int wholeAmmo = Mathf.FloorToInt(reloadAccumulator);
            if (wholeAmmo > 0)
            {
                currentAmmo = Mathf.Min(currentAmmo + wholeAmmo, weapon.ammoCapacity);
                reloadAccumulator -= wholeAmmo;
            }

            yield return null;
        }

        reloadAccumulator = 0f;
        isReloading = false;
        reloadRoutine = null;
    }

    // Call this script every time the player's weapon is changed! (please!!! ty) - ryan <3
    public void EquipWeapon(Weapon newWeapon)
    {
        if (newWeapon == null) return;

        if (reloadRoutine != null)
        {
            StopCoroutine(reloadRoutine);
            reloadRoutine = null;
        }
        isReloading = false;
        reloadAccumulator = 0f;

        weapon = newWeapon;
        currentAmmo = weapon.ammoCapacity;
    }

    private void OnDisable()
    {
        isReloading = false;
        reloadAccumulator = 0f;
        reloadRoutine = null;
    }
}