using UnityEngine;
using System.Collections;

public class MiscGun : MonoBehaviour
{
    // put this script with the PrimaryGun script like on the same object
    public MiscObject MiscObject;
    public Transform FirePoint;
    [SerializeField] private bool onCooldown;

    private void Awake()
    {
        onCooldown = false;
    }

    private void Update()
    {
        // i have no idea how the input system works so im just gonna
        if (Input.GetMouseButtonDown(0) && onCooldown == false)
        {
            ProjectileManager.Instance.ShootMiscTowardsMouse(MiscObject.MiscProjectile, FirePoint);
            //StartCoroutine()
        }
    }

    //private IEnumerator MiscOnCooldown()
    //{
     //   onCooldown
    //}
}
