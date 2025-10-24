using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class LoadSceneAdditive : MonoBehaviour
{
    // This function will be called to load the scene additively
    public string sceneName;

     // Called before Start() - to load the scene asynchronously
    private void Awake()
    {
        // Start a coroutine to load the scene asynchronously before Start
        StartCoroutine(LoadSceneAsyncBeforeStart());
    }

    // Coroutine to load the scene before the Start method
    private IEnumerator LoadSceneAsyncBeforeStart()
    {
     
        // Begin loading the scene asynchronously
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        
        // Optionally, you can wait until the scene is fully loaded
        while (!asyncOperation.isDone)
        {
            yield return null;  // Wait for the next frame
        }

        // The scene is loaded and can now be used
        Debug.Log("Scene '" + sceneName + "' is loaded before Start.");
    }

    // Start is called after the scene is loaded, but you can use this for other initializations
    private void Start()
    {
        Debug.Log("Start is called.");
    }

    public void LoadScene(string sceneName)
    {
        // Check if the scene is already loaded to avoid unnecessary load
        if (!SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            Debug.Log("Loading scene: " + sceneName);
        }
        else
        {
            Debug.Log("Scene " + sceneName + " is already loaded.");
        }
    }

    // Optional: You can trigger scene loading from a UI button or other events
    public void OnButtonPress()
    {
        // Example of loading a scene named "ExampleScene"
        LoadScene("ExampleScene");
    }


}

