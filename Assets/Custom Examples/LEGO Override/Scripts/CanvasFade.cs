using UnityEngine;
using System.Collections;

public class CanvasFade : MonoBehaviour
{
    public CanvasGroup canvasGroup;  // Reference to the CanvasGroup component
    public float fadeDuration = 1f;  // Duration of the fade effect
    public float visibleDuration = 3f;  // Time the canvas stays visible before fading out

    private void OnEnable()
    {
        // Make sure the CanvasGroup is properly initialized
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        // Start the fade-in effect as soon as the canvas is enabled
        FadeIn();
    }

    // Fade the canvas in
    public void FadeIn()
    {
        StartCoroutine(FadeCanvas(0f, 1f));  // Start from completely transparent to visible
    }

    // Fade the canvas out
    public void FadeOut()
    {
        StartCoroutine(FadeCanvas(1f, 0f));  // Start from fully visible to completely transparent
    }

    // Coroutine to handle fading
    private IEnumerator FadeCanvas(float fromAlpha, float toAlpha)
    {
        float elapsedTime = 0f;
        
        // Ensure the CanvasGroup is interactable and blocks raycasts if we're fading in
        if (toAlpha > 0f)
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
        else
        {
            // Disable interactions when fading out (optional, adjust as necessary)
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        // Perform the fade animation
        while (elapsedTime < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(fromAlpha, toAlpha, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the alpha is exactly the target value at the end
        canvasGroup.alpha = toAlpha;

        // If we just faded in, wait for the "visibleDuration" before fading out
        if (toAlpha > 0f)
        {
            yield return new WaitForSeconds(visibleDuration);
            FadeOut();  // Automatically fade out after visibleDuration has passed
        }
    }
}
