using UnityEngine;

[CreateAssetMenu(fileName = "RemoteBombMiscWeapon", menuName = "Scriptable Objects/RemoteBombMiscWeapon")]
public class RemoteBombProjectile : BombProjectile
{
    private void Start()
    {
        InputManager.Instance.OnPlayerRightClick.AddListener(Detonate);
    }

    public void Detonate(Vector2 _)
    {
        Debug.Log("Kaboom! Explodes and extends " + Radius + " after " + Timer + " seconds, or after a remote activation.");
    }
}
