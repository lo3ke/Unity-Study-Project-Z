using UnityEngine;
using UnityEditor.Callbacks;
using UnityEditor;

public class RoomNodeGraphEditor : EditorWindow
{
    private GUIStyle roomNodeStyle;
    private static RoomNodeGraphSO currentRoomNodeGraph;
    private RoomNodeTypeListSO roomNodeTypeList;

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

        // Load Room node types
        roomNodeTypeList = GameResources.Instance.roomNodeTypeList;
    }

    /// <summary>
    /// Open the room node graph editor window if a room node graph scriptable object asset is double clicked in the inspector
    /// </summary>
    [OnOpenAsset(0)] // Need the namespace UnityEditor.Callbacks
    public static bool OnDoubleClickAsset(int instanceID, int line)
    {
        // if the thing that is clicked on is actually a RoomNodeGraphSO, then roomNodeGraph will be populated with a value
        // if it isn't a RoomNodeGraphSO, then roomNodeGraph is going to be null.
        RoomNodeGraphSO roomNodeGraph = EditorUtility.InstanceIDToObject(instanceID) as RoomNodeGraphSO;

        if(roomNodeGraph != null)
        {
            OpenWindow();

            currentRoomNodeGraph = roomNodeGraph;

            return true;
        }
        return false;
    }

    /// <summary>
    /// Draw Editor GUI
    /// </summary>
    private void OnGUI()
    {
        // If a scriptable object of type RoomNodeGraphSO has been selected then process
        if(currentRoomNodeGraph != null)
        {
            // Process Events
            ProcessEvents(Event.current);

            // Draw Room Nodes
            DrawRoomNodes();
        }

        if (GUI.changed)
            Repaint();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="currentEvent">Generated event from the system (mouse click, etc..)</param>
    private void ProcessEvents(Event currentEvent)
    {
        ProcessRoomNodeGraphEvents(currentEvent);
    }

    /// <summary>
    /// Process Room Node Graph Events
    /// </summary>
    private void ProcessRoomNodeGraphEvents(Event currentEvent)
    {
        switch (currentEvent.type)
        {
            // Process Mouse Down Events, 'EventType' is a system enum that has different event types we can check
            case EventType.MouseDown:
                ProcessMouseDownEvent(currentEvent);
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// Process mouse down events on the room node graph (not over a node)
    /// </summary>
    private void ProcessMouseDownEvent(Event currentEvent)
    {
        // Process right click mouse down on graph event (show context menu)
        // 0 - Left click, 1 - Right click
        if(currentEvent.button == 1)
        {
            ShowContextMenu(currentEvent.mousePosition);
        }
    }

    /// <summary>
    /// Show the context menu
    /// </summary>
    private void ShowContextMenu(Vector2 mousePosition)
    {
        // GenericMenu lets you create custom context menus and dropdown menus.
        GenericMenu menu = new GenericMenu();

        // Adding item, menu description, false boolean to not show currently activated item tick, function to call when the menu item is selected (CreateRoomNode), the data to pass to the function called when the item is selected (mouse position)
        menu.AddItem(new GUIContent("Create Room Node"), false, CreateRoomNode, mousePosition);

        // Show the menu under the mouse when right-clicked.
        menu.ShowAsContext();
    }

    /// <summary>
    /// Create a room node at the mouse position
    /// </summary>
    /// <param name="mousePositionObject">Mouse position as an object</param>
    private void CreateRoomNode(object mousePositionObject)
    {
        // initial version gets called that passes mousePositionObject
        // then we pass in another parameter, roomNodeType to find an "x" from the list, where the "x" is isNone boolean = true
        CreateRoomNode(mousePositionObject, roomNodeTypeList.list.Find(x => x.isNone));
    }

    /// <summary>
    /// Create a room node at the mouse position - overloaded to also pass in RoomNodeType
    /// </summary>
    private void CreateRoomNode(object mousePositionObject, RoomNodeTypeSO roomNodeType)
    {
        Vector2 mousePosition = (Vector2)mousePositionObject;

        // create room node scriptable object asset
        RoomNodeSO roomNode = ScriptableObject.CreateInstance<RoomNodeSO>();

        // add room node to current room node graph room node list
        currentRoomNodeGraph.roomNodeList.Add(roomNode);

        // set room node values
        roomNode.Initialise(new Rect(mousePosition, new Vector2(nodeWidth, nodeHeight)), currentRoomNodeGraph, roomNodeType);

        // add room node to room node graph scriptable object asset database
        AssetDatabase.AddObjectToAsset(roomNode, currentRoomNodeGraph);

        // writes all unsaved asset changes to disk.
        AssetDatabase.SaveAssets();
    }

    /// <summary>
    /// Draw room nodes in the graph window
    /// </summary>
    private void DrawRoomNodes()
    {
        // Loop through all room nodes and draw them
        foreach(RoomNodeSO roomNode in currentRoomNodeGraph.roomNodeList)
        {
            roomNode.Draw(roomNodeStyle);
        }

        GUI.changed = true;
    }
}
