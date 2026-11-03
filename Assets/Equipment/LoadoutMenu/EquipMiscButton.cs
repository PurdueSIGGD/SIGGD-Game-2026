using UnityEngine;
using UnityEngine.UI;

// Same pattern as EquipGunButton, for one Misc Weapon asset.
// Duplicate the button and swap the Misc asset for each choice.
// Leave Target empty when a Loadout Target is on a parent. Leave the Button On Click list empty.
[DisallowMultipleComponent]
public class EquipMiscButton : MonoBehaviour
{
    [Tooltip("The misc this button equips. Duplicate this button and drag a different Misc Weapon asset here for each choice.")]
    public MiscWeapon misc;

    [Tooltip("Leave empty to use the Loadout Target on a parent of this button.")]
    public LoadoutTarget target;

    [Tooltip("Optional. A child object, such as a checkmark, that turns on while this misc is equipped. Do not drag this button itself here.")]
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
            Debug.LogWarning("Equip Misc Button could not find a Loadout Target. Put one on a parent, or drag it into Target.", this);
            return;
        }

        bound.EquipMisc(misc);
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
        equippedMarker.SetActive(bound != null && bound.IsCurrentMisc(misc));
    }
}
