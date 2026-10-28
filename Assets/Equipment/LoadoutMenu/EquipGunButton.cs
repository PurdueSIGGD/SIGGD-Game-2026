using UnityEngine;
using UnityEngine.UI;

// One button equips one gun. Duplicate the button and swap the Weapon asset for each choice.
// Leave Target empty when a Loadout Target is on a parent. See README.md in this folder.
// Awake hooks the Button, so leave the Button's On Click list empty.
// Adding Equip to On Click as well would equip the gun twice per click.
[DisallowMultipleComponent]
public class EquipGunButton : MonoBehaviour
{
    [Tooltip("The gun this button equips. Duplicate this button and drag a different Primary Weapon asset here for each choice.")]
    public PrimaryWeapon weapon;

    [Tooltip("Leave empty to use the Loadout Target on a parent of this button.")]
    public LoadoutTarget target;

    [Tooltip("Optional. A child object, such as a checkmark, that turns on while this gun is equipped. Do not drag this button itself here.")]
    public GameObject equippedMarker;

    LoadoutTarget bound;
    Button button;

    void Awake()
    {
        // A Button on this same object is enough. No On Click setup is required.
        button = GetComponent<Button>();
        if (button != null)
            button.onClick.AddListener(Equip);
    }

    void OnEnable()
    {
        Bind();
        Refresh();
    }

    void OnDisable()
    {
        // Drop the listener while this button is off so a hidden menu does not keep updating.
        if (bound != null)
            bound.Changed -= Refresh;
        bound = null;
    }

    void OnDestroy()
    {
        if (button != null)
            button.onClick.RemoveListener(Equip);
    }

    public void Equip()
    {
        Bind();
        if (bound == null)
        {
            Debug.LogWarning("Equip Gun Button could not find a Loadout Target. Put one on a parent, or drag it into Target.", this);
            return;
        }

        bound.EquipGun(weapon);
    }

    void Bind()
    {
        // Target wins when it is filled in. Otherwise use the Loadout Target above this button.
        LoadoutTarget next = target != null ? target : GetComponentInParent<LoadoutTarget>(true);
        if (bound == next)
            return;

        if (bound != null)
            bound.Changed -= Refresh;

        bound = next;
        if (bound != null)
            bound.Changed += Refresh;
    }

    void Refresh()
    {
        if (equippedMarker == null)
            return;

        // The marker must be a child, such as a checkmark. Turning this button off would make it unclickable.
        equippedMarker.SetActive(bound != null && bound.IsCurrentGun(weapon));
    }
}
