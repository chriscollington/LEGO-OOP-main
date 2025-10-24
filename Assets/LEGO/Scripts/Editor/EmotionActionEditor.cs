using UnityEditor;
using UnityEngine;
using Unity.LEGO.Minifig;
using Unity.LEGO.Behaviours.Actions;
using System;
using System.Linq;

[CustomEditor(typeof(EmotionAction))]
public class EmotionActionEditor : Editor
{
    private EmotionAction m_TalkAction;
    private SerializedProperty m_FaceAnimationControllerProp;
    private SerializedProperty m_AnimationFPSProp;

    private MinifigFaceAnimationController[] m_AvailableControllers;
    private string[] m_ControllerNames;
    private int m_SelectedControllerIndex;

    private void OnEnable()
    {
        m_TalkAction = (EmotionAction)target;
        m_FaceAnimationControllerProp = serializedObject.FindProperty("m_FaceAnimationController");
        m_AnimationFPSProp = serializedObject.FindProperty("m_AnimationFPS");

        // Retrieve all MinifigFaceAnimationController objects in the scene
        m_AvailableControllers = FindObjectsOfType<MinifigFaceAnimationController>();
        m_ControllerNames = m_AvailableControllers.Select(controller => controller.gameObject.name).ToArray();

        // Set the current selection index based on the existing reference
        m_SelectedControllerIndex = Array.IndexOf(m_AvailableControllers, m_FaceAnimationControllerProp.objectReferenceValue as MinifigFaceAnimationController);
        
        UpdateAvailableAnimations();
    }

    public override void OnInspectorGUI()
    {
        // Update serialized object before modifying
        serializedObject.Update();

        // Dropdown for selecting a controller from available objects in the scene
        if (m_AvailableControllers.Length > 0)
        {
            EditorGUILayout.LabelField("Select from available controllers:");
            int newSelectedControllerIndex = EditorGUILayout.Popup(m_SelectedControllerIndex, m_ControllerNames);

            // Update reference if a new selection is made
            if (newSelectedControllerIndex != m_SelectedControllerIndex)
            {
                m_SelectedControllerIndex = newSelectedControllerIndex;
                m_FaceAnimationControllerProp.objectReferenceValue = m_AvailableControllers[m_SelectedControllerIndex];
                UpdateAvailableAnimations();
            }
        }
        else
        {
            EditorGUILayout.HelpBox("No MinifigFaceAnimationController objects found in the scene.", MessageType.Warning);
        }

        // Display FPS setting
        EditorGUILayout.PropertyField(m_AnimationFPSProp, new GUIContent("Animation FPS"));

        // Display the available animations in the selected controller
        if (m_FaceAnimationControllerProp.objectReferenceValue != null)
        {
            MinifigFaceAnimationController selectedController = (MinifigFaceAnimationController)m_FaceAnimationControllerProp.objectReferenceValue;

            if (selectedController != null)
            {
                // Get available animations from the selected controller
                var availableAnimations = selectedController.GetAvailableAnimations();
                string[] animationNames = availableAnimations.Select(anim => anim.ToString()).ToArray();

                // Display the dropdown to select the animation
                int selectedIndex = EditorGUILayout.Popup("Select Animation", m_TalkAction.SelectedAnimationIndex, animationNames);

                // Update the selected animation if it changes
                if (selectedIndex != m_TalkAction.SelectedAnimationIndex)
                {
                    m_TalkAction.SelectAnimationToPlay(selectedIndex);
                }

                // Button to trigger the animation
                if (GUILayout.Button("Play Selected Animation"))
                {
                    m_TalkAction.TriggerFaceAnimation();
                }
            }
        }
        else
        {
            EditorGUILayout.HelpBox("Please assign a Face Animation Controller to play animations.", MessageType.Info);
        }

        // Apply any modified properties to the serialized object
        serializedObject.ApplyModifiedProperties();
    }

    // This function updates available animations for the selected controller
    private void UpdateAvailableAnimations()
    {
        if (m_FaceAnimationControllerProp.objectReferenceValue is MinifigFaceAnimationController controller)
        {
            m_TalkAction.m_FaceAnimations.Clear();
            m_TalkAction.m_FaceAnimations.AddRange(controller.GetAvailableAnimations());
        }
    }
}
