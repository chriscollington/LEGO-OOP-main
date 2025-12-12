using UnityEngine;

public class QuitButton : MonoBehaviour
{
    public void QuitGame()
    {
#if UNITY_EDITOR
        Debug.Log("QuitGame() called — would quit in a build.");
#else
            Application.Quit();
#endif
    }
}
