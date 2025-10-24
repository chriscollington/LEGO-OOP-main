using UnityEngine;
using UnityEngine.Animations;

namespace Unity.LEGO.Behaviours.Actions
{
    public class OpenUICanvasAction : Action
    {
        [SerializeField, Tooltip("The Canvas GameObject to open.")]
        private GameObject m_Canvas;

        [SerializeField, Tooltip("Automatically close the canvas after a specified time.")]
        private bool m_AutoClose = false;

        [SerializeField, Tooltip("The time in seconds before the canvas closes.")]
        private float m_CloseTime = 5.0f;

        private float m_TimeElapsed = 0.0f;
        private bool m_IsCanvasOpen = false;

         [SerializeField, Tooltip("Time in seconds before the trigger becomes active again.")]
        private float m_CooldownTime = 2f;

        private bool m_IsCoolingDown = false;
        protected override void Reset()
        {
            base.Reset();

            m_AudioVolume = 0.5f; // Retaining base class audio settings if needed
            m_IconPath = "Assets/LEGO/Gizmos/LEGO Behaviour Icons/UI Action.png";
        }

        protected void Update()
        {
            if (m_Active && !m_IsCoolingDown )
            {
                // Open the canvas if not already open
                if (!m_IsCanvasOpen)
                {
                    OpenCanvas();


                    // Handle auto-close logic
                    if (m_AutoClose)
                    {
                        m_TimeElapsed += Time.deltaTime;

                        if (m_TimeElapsed >= m_CloseTime)
                        {
                             
                             CloseCanvas();
                           
                        }
                    }
                }
            }
        }

        private void OpenCanvas()
        {
            if (m_Canvas)
            {
                m_Canvas.SetActive(true);
                m_IsCanvasOpen = true;
                m_IsCoolingDown = true;

            }
            else
            {
                Debug.LogWarning("No Canvas assigned to OpenUICanvasAction.");
            }
        }

        private void CloseCanvas()
        {
            m_Active = false; // Deactivate action

            if (m_Canvas)
            {
                m_Canvas.SetActive(false);
                m_IsCanvasOpen = false;

                // Start cooldown
                StartCooldown();
            }

            m_TimeElapsed = 0.0f; // Reset timer for next activation
        }

        public void CloseCanvas(GameObject m_Canvas)
        {
            
            m_Active = false; // Deactivate action

            if (m_Canvas)
            {
                m_Canvas.SetActive(false);
                m_IsCanvasOpen = false;

                // Start cooldown
                StartCooldown();
            }

            m_TimeElapsed = 0.0f; // Reset timer for next activation
        }

         private void StartCooldown()
        {
            m_IsCoolingDown = true;
            Invoke(nameof(EndCooldown), m_CooldownTime);
        }

        private void EndCooldown()
        {
            m_IsCoolingDown = false;
            m_Active = false; // Deactivate action
        }
    }
}
