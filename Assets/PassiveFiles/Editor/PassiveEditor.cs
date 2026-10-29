using UnityEngine;

using UnityEditor;

[CustomEditor(typeof(Passive))]
public class PassiveEditor : Editor
{
    public override void OnInspectorGUI()
    {

        EditorGUILayout.HelpBox(
            "PASSIVE SYSTEM GUIDE\n\n" +
            "Each Passive ScriptableObject stores the passive's name, " +
            "a buff value, a debuff value, and a stat modification mode.\n\n" +

            "STAT MODIFIERS:\n" +
            "Some Passives buff and debuff player stats. For example, " +
            "an Increased HP passive could have buff = 15 and debuff = 0. " +
            "Another script should read these values and apply them to " +
            "the player's data. Use buff for positive stat changes and " +
            "debuff for negative stat changes.\n\n" +

            "FLAT AND PERCENT MODES:\n" +
            "Each passive has a mode that determines how its buff and " +
            "debuff values are applied.\n\n" +
            "Flat: Adds or subtracts a fixed amount. For example, if " +
            "health starts at 100 and buff = 15, the final health is 115.\n\n" +
            "Percent: Increases or decreases a stat by a percentage. " +
            "For example, if health starts at 50 and buff = 25, health " +
            "increases by 25%% to 62.5.\n\n" +

            "BOOLEAN PASSIVES:\n" +
            "Some passives activate an ability or effect without " +
            "modifying a stat by a specific amount. For these passives, " +
            "keep both buff and debuff at 0. Other scripts should check " +
            "whether the passive is equipped to determine whether its " +
            "effect should be active.",
            MessageType.Info
        );



        EditorGUILayout.Space(10);

        // Display the ScriptableObject's normal fields.
        DrawDefaultInspector();
    }
}