using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RoomNodeGraphEditor : EditorWindow
{
    private GUIStyle roomNodeStyle;

    // Node layout values
    private const float nodeWidth = 160f;
    private const float nodeHeight = 75f;
    private const int nodePadding = 25; // padding - inside GUI element
    private const int nodeBorder = 12; // border - outside GUI element

    [MenuItem("Room Node Graph Editor", menuItem = "Window/Dungeon Editor/Room Node Graph Editor")]
    private static void OpenWindow()
    {
        GetWindow<RoomNodeGraphEditor>("Room Node Graph Editor");
    }

    private void OnEnable()
    {
        // Define node layout style
        roomNodeStyle = new GUIStyle(); // creating a new GUI Style for the Editor
        roomNodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D; // loading predefined unity asset bundled in the Unity Editor as a Texture
        roomNodeStyle.normal.textColor = Color.white; // setting text color to white
        roomNodeStyle.padding = new RectOffset(nodePadding, nodePadding, nodePadding, nodePadding); // padding
        roomNodeStyle.border = new RectOffset(nodeBorder, nodeBorder, nodeBorder, nodeBorder); // border
    }

    /// <summary>
    /// Draw Editor GUI
    /// </summary>
    private void OnGUI()
    {
        // Begin a GUILayout block of GUI controls in a fixed screen area
        GUILayout.BeginArea(new Rect(new Vector2(100f, 100f), new Vector2(nodeWidth, nodeHeight)), roomNodeStyle); // GUILayout.BeginArea to define a new area for the automatic layouting system to use
        EditorGUILayout.LabelField("Node 1"); // Label
        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(new Vector2(300f, 300f), new Vector2(nodeWidth, nodeHeight)), roomNodeStyle); // GUILayout.BeginArea to define a new area for the automatic layouting system to use
        EditorGUILayout.LabelField("Node 2"); // Label
        GUILayout.EndArea();
    }
}
