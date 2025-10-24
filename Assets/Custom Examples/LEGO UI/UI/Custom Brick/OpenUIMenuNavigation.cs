using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Unity.LEGO.Behaviours.Actions;
using Unity.VisualScripting;

namespace Unity.LEGO.UI
{
    // A script that allows to target a specific selectable.
    // It is used in various menus to preselect the main button.
    // This allows the UI navigation with WASD and arrow keys.

    public class OpenUIMenuNavigation : MonoBehaviour
    {
        [Serialize]
        public OpenUICanvasAction canvasController;

        public GameObject m_Canvas;


        public void CloseCanvas()
        {
            if(canvasController != null)
            {
                canvasController.CloseCanvas(m_Canvas);
            }
        }
    }
    

}