using UnityEditor;
using UnityEngine;
using Unity.LEGO.Minifig;

namespace Unity.LEGO.EditorExt
{

    [CustomEditor(typeof(MinifigFaceAnimationController))]

    public class MinifigFaceAnimationControllerEditor : Editor
    {
        MinifigFaceAnimationController controller;
        SerializedProperty AnimationsProp;

        void OnEnable()
        {
            controller = (MinifigFaceAnimationController)target;
            AnimationsProp = serializedObject.FindProperty("animations");
        }

       public override void OnInspectorGUI()
        {

                // Create a new GUIStyle with a bold font
                GUIStyle boldStyle = new GUIStyle(GUI.skin.label);
                boldStyle.fontStyle = FontStyle.Bold;

             // Draw the default inspector
            DrawDefaultInspector();

            if (AnimationsProp != null)
            {

                 GUILayout.Label("List animations", boldStyle);

                if (AnimationsProp.arraySize == 0)
                {
                   GUILayout.Label("No animations prepared", boldStyle);
                }
                else
                {
                    for (var i = 0; i < AnimationsProp.arraySize; ++i)
                    {
                        EditorGUILayout.BeginHorizontal();

                        GUILayout.Label(ObjectNames.NicifyVariableName(((MinifigFaceAnimationController.FaceAnimation)AnimationsProp.GetArrayElementAtIndex(i).enumValueIndex).ToString()));
                        EditorGUI.BeginDisabledGroup(!Application.isPlaying);

                        // Play button
                        if (GUILayout.Button(new GUIContent("Play", !Application.isPlaying ? "Only works in Play Mode" : "")))
                        {
                            controller.PlayAnimation((MinifigFaceAnimationController.FaceAnimation)(AnimationsProp.GetArrayElementAtIndex(i).enumValueIndex));
                        }
                        
                        EditorGUI.EndDisabledGroup();
                        EditorGUILayout.EndHorizontal();
                    }
                }
            }
        }

    }

}