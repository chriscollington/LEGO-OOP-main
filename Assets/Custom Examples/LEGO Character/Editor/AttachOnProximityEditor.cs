using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AttachOnProximity))]
public class AttachOnProximityEditor : Editor
{
    private string[] handTagOptions = { "LeftHand", "RightHand" }; // Options for dropdown

    public override void OnInspectorGUI()
    {
        AttachOnProximity script = (AttachOnProximity)target;

        DrawDefaultInspector();

         // Dropdown for handTag
       
        GUILayout.Space(10);

        // Add a dropdown for selecting the hand tag
        EditorGUILayout.LabelField("Hand Tag", EditorStyles.boldLabel);
        int selectedIndex = ArrayUtility.IndexOf(handTagOptions, script.handTag); // Find current index
        if (selectedIndex < 0) selectedIndex = 0; // Default to first option if not found

        selectedIndex = EditorGUILayout.Popup("Hand Tag", selectedIndex, handTagOptions);
        script.handTag = handTagOptions[selectedIndex]; // Update the selected tag

        // Add some space before the custom UI elements
        GUILayout.Space(10);

        // Add instructions for the user
        EditorGUILayout.LabelField("Instructions:", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("To save offsets during play mode:\n\n" +
                                "1. Adjust the object's position and rotation in the Scene View.\n" +
                                "2. Click the 'Save Offsets' button below.\n" +
                                "3. The offsets will be saved to the script variables.", MessageType.Info);

        // Add some space before the button
        GUILayout.Space(10); // 10 pixels of space above the button

        // Add a button to save the current offsets during play mode
        if (Application.isPlaying && GUILayout.Button("Save Offsets"))
        {
            script.SaveOffsets();
        }
    }
}
