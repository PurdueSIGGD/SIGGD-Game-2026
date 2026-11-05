using UnityEngine;

[CreateAssetMenu(fileName = "BombThrowerMiscWeapon", menuName = "Scriptable Objects/BombThrowerMiscWeapon")]
public class BombThrowerMiscWeapon : MiscWeapon
{
    public override void Use(GameObject miscProjectileObject)
    {
        Debug.Log("Fwhoosh!");
        MiscBullet bomb = miscProjectileObject.GetComponent<MiscBullet>();
        if (bomb.Type == "Remote Bomb")
        {
            bomb.bulletFunction?.Invoke(bomb);
        }
    }
}
