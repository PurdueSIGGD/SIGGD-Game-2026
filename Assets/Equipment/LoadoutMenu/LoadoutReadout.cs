using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Optional labels for the equipped gun, misc, and passive.
// Drag a Text or TextMeshPro component into each field. See README.md in this folder.
// This listens to LoadoutTarget.Changed, so the words update on the same click that equips.
public class LoadoutReadout : MonoBehaviour
{
    [Tooltip("Leave empty to use the Loadout Target on this object or a parent.")]
    public LoadoutTarget target;

    [Tooltip("Drag a Text or TextMeshPro component here. Shows the equipped gun.")]
    public Component gunLabel;

    [Tooltip("Drag a Text or TextMeshPro component here. Shows the equipped misc.")]
    public Component miscLabel;

    [Tooltip("Drag a Text or TextMeshPro component here. Shows the equipped passive.")]
    public Component passiveLabel;

    LoadoutTarget bound;

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

    void Bind()
    {
        // Target wins when it is filled in. Otherwise use the Loadout Target on this object or a parent.
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
        if (bound == null)
        {
            SetLabel(gunLabel, "None");
            SetLabel(miscLabel, "None");
            SetLabel(passiveLabel, "None");
            return;
        }

        SetLabel(gunLabel, bound.CurrentGunName());
        SetLabel(miscLabel, bound.CurrentMiscName());
        SetLabel(passiveLabel, bound.CurrentPassiveName());
    }

    static void SetLabel(Component label, string value)
    {
        if (label == null)
            return;

        // Either UI Text or TextMeshPro can be dropped in the Inspector.

        if (label is Text uiText)
        {
            uiText.text = value;
            return;
        }

        if (label is TMP_Text tmp)
        {
            tmp.text = value;
            return;
        }

        Debug.LogWarning("Loadout Readout needs a Text or TextMeshPro component in this field.", label);
    }
}
