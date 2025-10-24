using System.Collections.Generic;
using UnityEngine;
using Unity.LEGO.Behaviours.Triggers;
using Unity.LEGO.Game;
using UnityEditor.VersionControl;

namespace Unity.LEGO.Behaviours.Actions
{
    public abstract class InfoAction : Action
    {
        public abstract InfoConfiguration GetDefaultInfoConfiguration(Trigger trigger);

        [SerializeField]
        List<InfoConfiguration> m_InfoConfigurations = new List<InfoConfiguration>();
        [SerializeField]
        List<Trigger> m_Triggers = new List<Trigger>();

        public override void Activate()
        {
            PlayAudio(spatial: false, destroyWithAction: false);

            base.Activate();

            m_Active = false;
        }

        protected override void Start()
        {
            base.Start();

            if (IsPlacedOnBrick())
            {
                InfoConfiguration infoConfiguration;

                var targetingTriggers = GetTargetingTriggers();

                if (targetingTriggers.Count == 0)
                {
                    infoConfiguration = GetDefaultInfoConfiguration(null);
                    AddInfo(null, infoConfiguration.Title, infoConfiguration.Description, infoConfiguration.Hidden);
                }

                // Find all Triggers and create UI for them.
                foreach (var trigger in targetingTriggers)
                {
                    var triggerIndex = m_Triggers.IndexOf(trigger);
                    if (triggerIndex >= 0)
                    {
                        infoConfiguration = m_InfoConfigurations[triggerIndex];
                    }
                    else
                    {
                        infoConfiguration = GetDefaultInfoConfiguration(trigger);
                    }

                    // Add Info to this game object.
                    AddInfo(trigger, infoConfiguration.Title, infoConfiguration.Description, infoConfiguration.Hidden);
                }
            }
        }

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            var gizmoBounds = GetGizmoBounds();

            if (GetTargetingTriggers().Count == 0)
            {
                Gizmos.DrawIcon(gizmoBounds.center + Vector3.up * 2, "Assets/LEGO/Gizmos/LEGO Behaviour Icons/Warning.png");
            }
        }

        void AddInfo(Trigger trigger, string title, string description, bool hidden)
        {
        var objective = gameObject.AddComponent<Info>();
            objective.m_Trigger = trigger;
            objective.m_Title = title;
            objective.m_Description = description;
 
            objective.m_Hidden = hidden;
        }

    }
}
