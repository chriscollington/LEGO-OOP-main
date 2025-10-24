using UnityEngine;
using UnityEngine.UI;
using Unity.LEGO.Game;

namespace Unity.LEGO.UI
{
    // This is the component that is responsible for the Objectives display in the top left corner of the screen

    [RequireComponent(typeof(RectTransform))]
    public class Info : MonoBehaviour
    {
        [Header("References")]

        [SerializeField, Tooltip("The text that will display the title.")]
        TMPro.TextMeshProUGUI m_TitleText = default;

        [SerializeField, Tooltip("The text that will display the description.")]
        TMPro.TextMeshProUGUI m_DescriptionText = default;


        [Header("Movement")]

        [SerializeField, Tooltip("The animation curve for moving in.")]
        AnimationCurve m_MoveInCurve = default;

        float m_Time;
        const float s_Margin = 25;
        const float s_TitleAndProgressSpacing = 4;

        public bool UseUI = true;
        public GameObject UI;

        RectTransform m_RectTransform;

        public void Initialize(string title, string description)
        {
            m_RectTransform = GetComponent<RectTransform>();


            // Set title text
            m_TitleText.text = title;

            // Set description text.
            m_DescriptionText.text = description;
        }




        void Update()
        {
            // Update time.
            m_Time += Time.deltaTime;

            // Move in.
            m_RectTransform.anchoredPosition = new Vector2((m_RectTransform.sizeDelta.x + s_Margin) * m_MoveInCurve.Evaluate(m_Time), m_RectTransform.anchoredPosition.y);
        }
    }
}
