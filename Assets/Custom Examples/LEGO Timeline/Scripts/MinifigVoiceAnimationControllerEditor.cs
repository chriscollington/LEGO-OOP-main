using UnityEditor;
using UnityEngine;
using Unity.LEGO.Minifig;
using UnityEditorInternal;

namespace Unity.LEGO.EditorExt
{
    [CustomEditor(typeof(MinifigVoiceAnimationController))]
    public class MinifigVoiceAnimationControllerEditor : Editor
    {
        private MinifigVoiceAnimationController controller;
        private SerializedProperty voiceAnimationsProp;
        private ReorderableList reorderableList;

        private void OnEnable()
        {
            controller = (MinifigVoiceAnimationController)target;
            voiceAnimationsProp = serializedObject.FindProperty("voiceAnimations");

            // Initialize the ReorderableList
            reorderableList = new ReorderableList(serializedObject, voiceAnimationsProp, true, true, true, true)
            {
                // Draw Header
                drawHeaderCallback = (Rect rect) =>
                {
                    EditorGUI.LabelField(rect, "Voice Animations (Drag to Reorder)");
                },

                // Draw each element in the list
                drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    var element = voiceAnimationsProp.GetArrayElementAtIndex(index);
                    var animationClipProp = element.FindPropertyRelative("animationClip");
                    var audioClipProp = element.FindPropertyRelative("audioClip");

                    rect.y += 2;

                    // Draw AnimationClip field
                    EditorGUI.PropertyField(
                        new Rect(rect.x, rect.y, rect.width - 120, EditorGUIUtility.singleLineHeight),
                        animationClipProp,
                        GUIContent.none
                    );

                    // Draw AudioClip field
                    EditorGUI.PropertyField(
                        new Rect(rect.x + rect.width - 115, rect.y, 100, EditorGUIUtility.singleLineHeight),
                        audioClipProp,
                        GUIContent.none
                    );

                    // Play button
                    if (GUI.Button(new Rect(rect.x + rect.width - 55, rect.y, 50, EditorGUIUtility.singleLineHeight), "Play"))
                    {
                        var voiceAnimation = new VoiceAnimation
                        {
                            animationClip = animationClipProp.objectReferenceValue as AnimationClip,
                            audioClip = audioClipProp.objectReferenceValue as AudioClip
                        };
                        controller.PlayVoiceAnimation(voiceAnimation);
                    }
                },

                // Element Height
                elementHeightCallback = (index) => EditorGUIUtility.singleLineHeight + 6,

                // Add Element
                onAddCallback = (ReorderableList list) =>
                {
                    voiceAnimationsProp.arraySize++;
                    list.index = voiceAnimationsProp.arraySize - 1;
                },

                // Remove Element with confirmation dialog
                onRemoveCallback = (ReorderableList list) =>
                {
                    if (EditorUtility.DisplayDialog("Warning!", "Are you sure you want to delete this animation?", "Yes", "No"))
                    {
                        ReorderableList.defaultBehaviours.DoRemoveButton(list);
                    }
                }
            };
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // Draw the ReorderableList
            reorderableList.DoLayoutList();

            // Button to play all animations in sequence with their audio
            if (GUILayout.Button("Play All Voice Animations"))
            {
                controller.PlayAllVoiceAnimations();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
