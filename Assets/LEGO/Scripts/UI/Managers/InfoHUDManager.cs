using System.Collections.Generic;
using Unity.LEGO.Game;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.LEGO.UI
{
    public class InfoHUDManager : MonoBehaviour
    {
        [Header("References")]

        [SerializeField, Tooltip("The UI panel containing the layoutGroup for displaying messages.")]
        RectTransform m_infoPanel = default;

        [SerializeField, Tooltip("The prefab for Message .")]
        GameObject m_MessagePrefab = default;

        
        const int s_TopMargin = 10;
        const int s_Spacing = 10;
        float m_NextY;

        protected void Awake()
        {
            EventManager.AddListener<InfoAdded>(OnInfoAdded);
        }

        void OnInfoAdded(InfoAdded evt)
        {
               if (!evt.Info.m_Hidden)
            {
           
                GameObject go = Instantiate(m_MessagePrefab, m_infoPanel.transform);

                // Initialise the objective element.
                Info info = go.GetComponent<Info>();
                info.Initialize(evt.Info.m_Title, evt.Info.m_Description);

            }
        }

        void OnDestroy()
        {
            EventManager.RemoveListener<InfoAdded>(OnInfoAdded);
        }
    }
}
