using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Unity.LEGO.Behaviours.Actions;
using Unity.LEGO.Behaviours.Triggers;

namespace Unity.LEGO.EditorExt
{
    [CustomEditor(typeof(InfoAction), true)]
    public class InfoActionEditor : ActionEditor
    {
        protected InfoAction m_InfoAction;

        Trigger m_FocusedTrigger = null;

        SerializedProperty m_InfoConfigurationsProp;
        SerializedProperty m_TriggersProp;

        protected override void OnEnable()
        {
            base.OnEnable();

            m_InfoAction = (InfoAction)m_Action;

            m_InfoConfigurationsProp = serializedObject.FindProperty("m_InfoConfigurations");
            m_TriggersProp = serializedObject.FindProperty("m_Triggers");
        }

        protected override void CreateGUI()
        {
            // Collect all Triggers that target this Info Action.
            List<Trigger> targetingTriggers = m_InfoAction.GetTargetingTriggers();

            // Check if trigger is already registered with the info Action.
            foreach (var targetingTrigger in targetingTriggers)
            {
                var foundTrigger = false;
                for (var i = 0; i < m_TriggersProp.arraySize; ++i)
                {
                    if (m_TriggersProp.GetArrayElementAtIndex(i).objectReferenceValue == targetingTrigger)
                    {
                        foundTrigger = true;
                        break;
                    }
                }

                // If trigger was new, set up the corresponding info with reasonable default values.
                if (!foundTrigger)
                {
                    m_TriggersProp.arraySize++;
                    m_InfoConfigurationsProp.arraySize++;
                    m_TriggersProp.GetArrayElementAtIndex(m_TriggersProp.arraySize - 1).objectReferenceValue = targetingTrigger;

                    var infoConfigurationProp = m_InfoConfigurationsProp.GetArrayElementAtIndex(m_InfoConfigurationsProp.arraySize - 1);
                    var infoConfiguration = m_InfoAction.GetDefaultInfoConfiguration(targetingTrigger);
                    infoConfigurationProp.FindPropertyRelative("Title").stringValue = infoConfiguration.Title;
                    infoConfigurationProp.FindPropertyRelative("Description").stringValue = infoConfiguration.Description;
                    infoConfigurationProp.FindPropertyRelative("Hidden").boolValue = infoConfiguration.Hidden;
                }
            }

            EditorGUI.BeginDisabledGroup(EditorApplication.isPlaying);

            // Show the info of the currently targeting triggers.
            for (var i = 0; i < m_TriggersProp.arraySize; ++i)
            {
                var trigger = (Trigger)m_TriggersProp.GetArrayElementAtIndex(i).objectReferenceValue;

                if (targetingTriggers.Contains(trigger))
                {
                    var label = trigger.GetType().ToString();
                    label = label.Substring(label.LastIndexOf('.') + 1);
                    label = ObjectNames.NicifyVariableName(label);
                    var infoConfigurationProp = m_InfoConfigurationsProp.GetArrayElementAtIndex(i);
                    GUI.SetNextControlName("Trigger " + i);
                    if (EditorGUILayout.PropertyField(infoConfigurationProp, new GUIContent("Info for " + label), false))
                    {
                        EditorGUI.indentLevel++;
                        GUI.SetNextControlName("Trigger Title " + i);
                        EditorGUILayout.PropertyField(infoConfigurationProp.FindPropertyRelative("Title"), new GUIContent("Title", "The title of the info."));
                        GUI.SetNextControlName("Trigger Description " + i);
                        EditorGUILayout.PropertyField(infoConfigurationProp.FindPropertyRelative("Description"), new GUIContent("Details", "The details of the info."));
                        GUI.SetNextControlName("Trigger Hidden " + i);
                        EditorGUILayout.PropertyField(infoConfigurationProp.FindPropertyRelative("Hidden"), new GUIContent("Hidden", "Hide the info from the player."));
                        EditorGUI.indentLevel--;
                    }
                }
            }

            EditorGUI.EndDisabledGroup();

            var previousFocusedTrigger = m_FocusedTrigger;

            // Find the currently focused Trigger.
            var focusedControlName = GUI.GetNameOfFocusedControl();
            var lastSpace = focusedControlName.LastIndexOf(' ');
            if (focusedControlName.StartsWith("Trigger") && lastSpace >= 0)
            {
                var index = int.Parse(focusedControlName.Substring(lastSpace + 1));
                m_FocusedTrigger = (Trigger)m_TriggersProp.GetArrayElementAtIndex(index).objectReferenceValue;
            }
            else
            {
                m_FocusedTrigger = null;
            }

            if (m_FocusedTrigger != previousFocusedTrigger)
            {
                SceneView.RepaintAll();
            }
        }

        public override void OnSceneGUI()
        {
            if (Event.current.type == EventType.Repaint)
            {
                if (m_InfoAction)
                {
                    DrawConnections(m_InfoAction, m_InfoAction.GetTargetingTriggers(), false, Color.cyan, m_FocusedTrigger);
                }
            }
        }
    }
}
