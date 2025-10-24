using System.Collections.Generic;
using UnityEngine;

namespace Unity.LEGO.Game
{
    public class InfoManager : MonoBehaviour
    {
        List<Info> m_Messages;

        protected void Awake()
        {
            m_Messages = new List<Info>();

            EventManager.AddListener<InfoAdded>(OnInfoAdded);
        }

        void OnInfoAdded(InfoAdded evt)
        {
            m_Messages.Add(evt.Info);
        }


        void OnDestroy()
        {
            EventManager.RemoveListener<InfoAdded>(OnInfoAdded);
        }
    }
}
