using UnityEngine;
using Unity.LEGO.Minifig;
using System.Collections.Generic;

namespace Unity.LEGO.Behaviours.Actions
{
    public class EmotionAction : Action
    {
        [SerializeField] private MinifigFaceAnimationController m_FaceAnimationController;
        [SerializeField] private float m_AnimationFPS = 24.0f;
        [SerializeField] public List<MinifigFaceAnimationController.FaceAnimation> m_FaceAnimations = new List<MinifigFaceAnimationController.FaceAnimation>();

        private int m_SelectedAnimationIndex;

        public int SelectedAnimationIndex => m_SelectedAnimationIndex;

        protected override void Reset()
        {
            base.Reset();
            m_IconPath = "Assets/LEGO/Gizmos/LEGO Behaviour Icons/Face Action.png";
        }

  

        protected void Update()
        {
            if (m_Active)
            {
                // TriggerFaceAnimation can still be called directly by the editor, even if m_Active is false
                TriggerFaceAnimation();

                 m_Active = false;
            }
        }

        public void SelectAnimationToPlay(int selectedIndex)
        {
            if (selectedIndex >= 0 && selectedIndex < m_FaceAnimations.Count)
            {
                m_SelectedAnimationIndex = selectedIndex;
            }
        }

        public void TriggerFaceAnimation()
        {
            if (m_FaceAnimationController != null && m_FaceAnimations.Count > 0)
            {
                var animation = m_FaceAnimations[m_SelectedAnimationIndex];
                m_FaceAnimationController.PlayAnimation(animation, m_AnimationFPS);
            }
            else
            {
                Debug.LogWarning("No face animation controller or animations available.");
            }
        }
    }
}
