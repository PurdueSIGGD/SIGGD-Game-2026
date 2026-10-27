using UnityEngine;
using System.Collections;

public class MiscEquipment : MonoBehaviour
{
    public MiscWeapon MiscWeapon;

    private Coroutine miscOnCooldown;

    private void OnEnable()
    {
        InputManager.Instance.OnPlayerRightClick.AddListener(UseMisc);
    }

    private void OnDisable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnPlayerRightClick.RemoveListener(UseMisc);
        }

        if (miscOnCooldown != null)
        {
            StopCoroutine(miscOnCooldown);
            miscOnCooldown = null;
        }
    }


    private void UseMisc(Vector2 position)
    {
        if (miscOnCooldown != null) return;
        if (MiscWeapon == null) return;

        MiscWeapon.Use();
        miscOnCooldown = StartCoroutine(MiscOnCooldown());
    }

    private IEnumerator MiscOnCooldown()
    {
        float cooldownRemaining = MiscWeapon.Cooldown;

        while (cooldownRemaining > 0)
        {
            Debug.Log($"Cooldown: {cooldownRemaining:F2}");

            if (cooldownRemaining % 1 == 0)
            {
                yield return new WaitForSeconds(1f);
                cooldownRemaining--;
            }
            else
            {
                yield return new WaitForSeconds(cooldownRemaining % 1);
                cooldownRemaining -= cooldownRemaining % 1;
            }
        }

        Debug.Log("Cooldown over!");

        miscOnCooldown = null;
    }
}
