using System;
using UnityEngine;
using Unity.LEGO.Behaviours.Triggers;
using Unity.LEGO.Game;

namespace Unity.LEGO.Behaviours
{
    public class Info : MonoBehaviour
    {
        public Trigger m_Trigger;
        public string m_Title { get; set; }
        public string m_Description { get; set; }
        public bool m_Hidden { get; set; }



        void Start()
        {
            InfoAdded evt = Events.InfoAddedEvent;
            EventManager.Broadcast(evt);

            if (m_Trigger)
            {
                m_Trigger.OnActivate += Activate;
            }
            else
            {
                Activate();
            }
        }

        void Activate()
        {
   

        }


    }
}
