using UnityEditor;
using Unity.LEGO.Behaviours.Actions;

namespace Unity.LEGO.EditorExt
{
    [CustomEditor(typeof(OpenUICanvasAction), true)]
    public class OpenUICanvasActionEditor : ActionEditor
    {
        SerializedProperty m_CanvasProp;
        SerializedProperty m_AutoCloseProp;
        SerializedProperty m_CloseTimeProp;
        SerializedProperty m_CooldownTime;



        protected override void OnEnable()
        {
            base.OnEnable();

            m_CanvasProp = serializedObject.FindProperty("m_Canvas");
            m_AutoCloseProp = serializedObject.FindProperty("m_AutoClose");
            m_CloseTimeProp = serializedObject.FindProperty("m_CloseTime");
            m_CooldownTime = serializedObject.FindProperty("m_CooldownTime");
        }

        protected override void CreateGUI()
        {
 
            EditorGUILayout.PropertyField(m_CanvasProp);
            EditorGUILayout.PropertyField(m_AutoCloseProp);

            // Enable or disable CloseTime field based on AutoClose toggle
            EditorGUI.BeginDisabledGroup(!m_AutoCloseProp.boolValue);
            EditorGUILayout.PropertyField(m_CloseTimeProp);
            EditorGUILayout.PropertyField(m_CooldownTime);
            EditorGUI.EndDisabledGroup();
        }
    }
}
