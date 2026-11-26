using UnityEngine;
using UnityEditor;
using UnityEngine.Splines;
using Unity.Mathematics;

public class SplineConnector : EditorWindow
{
    private GameObject mainSplineObject;
    private GameObject splineToAddObject;

    private float xScale;
    private float yScale;

    private SplineContainer mainContainer;
    private SplineContainer addContainer;

    // Adds a menu item at the top of Unity to open this window
    [MenuItem("Tools/Spline Connector")]
    public static void ShowWindow()
    {
        GetWindow<SplineConnector>("Spline Connector");
    }

    private void OnGUI()
    {
        GUILayout.Label("Assign Two Gameobjects with SplineContainers", EditorStyles.boldLabel);

        // Object fields where you can drag and drop GameObjects
        mainSplineObject = (GameObject)EditorGUILayout.ObjectField("Main Spline", mainSplineObject, typeof(GameObject), true);
        splineToAddObject = (GameObject)EditorGUILayout.ObjectField("Spline To Add", splineToAddObject, typeof(GameObject), true);

        xScale = EditorGUILayout.FloatField("X Scale", xScale);
        yScale = EditorGUILayout.FloatField("Y Scale", yScale);

        GUILayout.Space(10);

        // Disable the button if either slot is empty
        EditorGUI.BeginDisabledGroup(mainSplineObject == null || splineToAddObject == null);
        {
            if (GUILayout.Button("Add Spline To Main Splines"))
            {
                CombineSplines();
            }
        }
        EditorGUI.EndDisabledGroup();

        GUILayout.Space(10);

        GUILayout.Label("Assign the X scale and Y scale", EditorStyles.boldLabel);

        EditorGUI.BeginDisabledGroup(mainSplineObject == null || xScale == 0 || yScale == 0);
        {
            if (GUILayout.Button("Scale the main spline"))
            {
                ScaleSpline();
            }
        }
        EditorGUI.EndDisabledGroup();
    }

    private void CombineSplines()
    {
        mainContainer = mainSplineObject.GetComponent<SplineContainer>();
        addContainer = splineToAddObject.GetComponent<SplineContainer>();

        if (mainContainer == null || addContainer == null)
        {
            Debug.LogError("One Input Object Is Missing A Spline");
            return;
        }

        Undo.RecordObject(mainContainer, "Connect Two Splines");

        // get the spline from add spline
        Spline newSpline = new Spline(addContainer.Splines[0]);
        Spline mainSpline = mainContainer.Splines[0];

        BezierKnot finalKnotOfMain = mainSpline[mainSpline.Count - 1];
        float3 offsetAmount = new float3(finalKnotOfMain.Position.x, finalKnotOfMain.Position.y, 0);

        // loop through every knot and add the x and y of the final knot of main container to it
        for (int i = 0; i < newSpline.Count; i++)
        {
            bool linearTan = false;

            BezierKnot knot = newSpline[i];

            TangentMode originalTangent = addContainer.Splines[0].GetTangentMode(i);

            if (originalTangent == TangentMode.Linear)
            {
                linearTan = true;
            }

            knot.Position += offsetAmount;

            newSpline[i] = knot;

            // if tangent isnt linear force it to be autosmooth otherwise it converts them from auto to bezier which messes up the path between splines
            if (linearTan)
            {
                newSpline.SetTangentMode(i, TangentMode.Linear);
            }
            else
            {
                newSpline.SetTangentMode(i, TangentMode.AutoSmooth);
            }
        }

        SplineUtility.AddSpline(mainContainer, newSpline);

        EditorUtility.SetDirty(mainContainer);
    }

    private void ScaleSpline()
    {
        mainContainer = mainSplineObject.GetComponent<SplineContainer>();

        if (mainContainer == null)
        {
            Debug.LogError("Main Spline Object Is Missing A Spline");
        }

        Undo.RecordObject(mainContainer, "Scale Main Splines");

        if (mainContainer.Splines.Count > 0)
        {
            for (int i = 0; i < mainContainer.Splines.Count; i++)
            {
                Spline curSpline = mainContainer.Splines[i];

                for (int j = 0; j < curSpline.Count; j++)
                {
                    BezierKnot knot = curSpline[j];

                    knot.Position.x *= xScale;
                    knot.Position.y *= yScale;

                    curSpline[j] = knot;
                }
            }
        }
        else
        {
            Debug.LogError("Inputted main spline has no splines");
        }

        EditorUtility.SetDirty(mainContainer);
    }
}
