using System;
using UnityEngine;

namespace Unity.LEGO.Game
{
    public interface Info
    {
         string m_Title { get; }
        string m_Description { get; }
 
        bool m_Hidden { get; }

 
    }
}
