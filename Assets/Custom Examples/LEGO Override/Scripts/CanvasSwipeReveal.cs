using UnityEngine;
using System.Collections;

public class CanvasSwipeReveal : MonoBehaviour
{
    public RectTransform revealMask;  // The RectTransform that acts as the mask (the swipe area)
    public RectTransform canvasPanel; // The panel that you want to reveal (inside the canvas)
    public float swipeDuration = 1f;  // Duration of the swipe animation
    public float revealDelay = 0.5f;  // Delay before the swipe starts (optional)

    private Vector2 initialMaskPosition;  // Starting position of the mask

    private void Start()
    {
        // Store the initial position of the mask
        if (revealMask != null)
        {
            initialMaskPosition = revealMask.anchoredPosition;
            revealMask.anchoredPosition = new Vector2(-revealMask.rect.width, initialMaskPosition.y); // Move offscreen to the left initially
        }

        // Start the swipe reveal after the delay
        StartCoroutine(SwipeReveal());
    }

    private IEnumerator SwipeReveal()
    {
        // Wait for the reveal delay before starting the swipe
        yield return new WaitForSeconds(revealDelay);

        float elapsedTime = 0f;
        Vector2 targetPosition = initialMaskPosition; // The target position of the mask to reveal the panel

        // Animate the mask moving to reveal the panel
        while (elapsedTime < swipeDuration)
        {
            float t = elapsedTime / swipeDuration;
            revealMask.anchoredPosition = Vector2.Lerp(revealMask.anchoredPosition, targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the mask is fully in place at the end
        revealMask.anchoredPosition = targetPosition;
    }

    // You can call this method to trigger the swipe reveal manually
    public void TriggerSwipeReveal()
    {
        StopAllCoroutines(); // Stop any previous swipe animations
        StartCoroutine(SwipeReveal());
    }
}
