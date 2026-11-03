using System;
using UnityEngine;

// One of these sits on the menu panel. See README.md in this folder for setup.
// Buttons under this object call EquipGun, EquipMisc, and EquipPassive.
// Those methods talk to the live player. They do not use EquipmentManager.
// For a second player, duplicate the whole menu and drag in that player's guns.
public class LoadoutTarget : MonoBehaviour
{
    [Header("Player")]
    [Tooltip("Primary Gun on the player's Weapon Holder.")]
    public PrimaryGun gun;

    [Tooltip("Misc Gun on the player's Weapon Holder. It sits on the same object as Primary Gun.")]
    public MiscGun misc;

    [Tooltip("Passive Holder on the player. Add that component to the player if it is missing, then drag it here. Choosing a passive stores it. It does not change the player's stats yet.")]
    public PassiveHolder passiveHolder;

    // Markers and the readout listen to this so they update after a click.
    public event Action Changed;

    public void EquipGun(PrimaryWeapon weapon)
    {
        if (gun == null)
        {
            Debug.LogWarning("Loadout Target has no Primary Gun. Drag the player's Weapon Holder into the Gun field.", this);
            return;
        }

        if (weapon == null)
        {
            Debug.LogWarning("This gun button has no weapon. Drag a Primary Weapon asset onto it.", this);
            return;
        }

        // EquipWeapon swaps the asset, refills ammo, and replaces the held visual.
        gun.EquipWeapon(weapon);
        Changed?.Invoke();
    }

    public void EquipMisc(MiscWeapon weapon)
    {
        if (misc == null)
        {
            Debug.LogWarning("Loadout Target has no Misc Gun. Drag the player's Weapon Holder into the Misc field.", this);
            return;
        }

        if (weapon == null)
        {
            Debug.LogWarning("This misc button has no misc. Drag a Misc Weapon asset onto it.", this);
            return;
        }

        // EquipMisc swaps the asset, clears the cooldown, and replaces the misc visual.
        misc.EquipMisc(weapon);
        Changed?.Invoke();
    }

    public void EquipPassive(Passive passive)
    {
        if (passiveHolder == null)
        {
            Debug.LogWarning("Loadout Target has no Passive Holder. Add Passive Holder to the player, then drag it into the Passive Holder field.", this);
            return;
        }

        if (passive == null)
        {
            Debug.LogWarning("This passive button has no passive. Drag a Passive asset onto it.", this);
            return;
        }

        // The capital P is required. setpassive does not exist on PassiveHolder.
        // This only stores the asset. Nothing applies buff, debuff, or mode yet.
        passiveHolder.setPassive(passive);
        Changed?.Invoke();
    }

    public bool IsCurrentGun(PrimaryWeapon weapon)
    {
        return gun != null && weapon != null && gun.Weapon == weapon;
    }

    public bool IsCurrentMisc(MiscWeapon weapon)
    {
        return misc != null && weapon != null && misc.MiscWeapon == weapon;
    }

    public bool IsCurrentPassive(Passive passive)
    {
        // getPassive matches PassiveHolder. getpassive will not compile.
        return passiveHolder != null && passive != null && passiveHolder.getPassive() == passive;
    }

    public string CurrentGunName()
    {
        if (gun == null || gun.Weapon == null)
            return "None";

        if (string.IsNullOrEmpty(gun.Weapon.WeaponName))
            return gun.Weapon.name;

        return gun.Weapon.WeaponName;
    }

    public string CurrentMiscName()
    {
        if (misc == null || misc.MiscWeapon == null)
            return "None";

        if (string.IsNullOrEmpty(misc.MiscWeapon.Name))
            return misc.MiscWeapon.name;

        return misc.MiscWeapon.Name;
    }

    public string CurrentPassiveName()
    {
        if (passiveHolder == null || passiveHolder.getPassive() == null)
            return "None";

        // Passive has no public display name, so the label uses the asset name.
        return passiveHolder.getPassive().name;
    }
}
