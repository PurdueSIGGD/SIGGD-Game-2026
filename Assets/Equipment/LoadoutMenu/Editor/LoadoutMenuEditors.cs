using UnityEditor;
using UnityEngine;

// Inspector-only help boxes. This file is not part of the build.
// The text here is what teammates see on the component, so they can set the menu up without opening the scripts.

[CustomEditor(typeof(LoadoutTarget))]
public class LoadoutTargetEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox(
            "Put this on the menu panel once.\n" +
            "Drag in the player's Primary Gun, Misc Gun, and Passive Holder.\n" +
            "Equip buttons under this object use it automatically.\n" +
            "For a second player, duplicate the whole menu and point this copy at player 2.",
            MessageType.Info);
        DrawDefaultInspector();
    }
}

[CustomEditor(typeof(EquipGunButton))]
public class EquipGunButtonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox(
            "Put this on a Button. Drag the gun asset into Weapon.\n" +
            "Duplicate the button and swap the asset for each gun.\n" +
            "Leave Target empty when a Loadout Target is on a parent.\n" +
            "The button equips on click. Leave the Button On Click list empty.",
            MessageType.Info);
        DrawDefaultInspector();
    }
}

[CustomEditor(typeof(EquipMiscButton))]
public class EquipMiscButtonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox(
            "Put this on a Button. Drag the misc asset into Misc.\n" +
            "Duplicate the button and swap the asset for each misc.\n" +
            "Leave Target empty when a Loadout Target is on a parent.\n" +
            "The button equips on click. Leave the Button On Click list empty.",
            MessageType.Info);
        DrawDefaultInspector();
    }
}

[CustomEditor(typeof(EquipPassiveButton))]
public class EquipPassiveButtonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox(
            "Put this on a Button. Drag the passive asset into Passive.\n" +
            "Duplicate the button and swap the asset for each passive.\n" +
            "Leave Target empty when a Loadout Target is on a parent.\n" +
            "The button equips on click. Leave the Button On Click list empty.",
            MessageType.Info);
        DrawDefaultInspector();
    }
}

[CustomEditor(typeof(LoadoutReadout))]
public class LoadoutReadoutEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox(
            "Optional labels for the current gun, misc, and passive.\n" +
            "Drag a Text or TextMeshPro component into each label field.\n" +
            "Leave Target empty when a Loadout Target is on this object or a parent.",
            MessageType.Info);
        DrawDefaultInspector();
    }
}
