using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CanvasClockwiseReveal : MonoBehaviour
{
    public RectTransform canvasPanel; // The panel that you want to reveal (inside the canvas)
    public Image revealMaskImage;    // The circular mask (can be an Image or RawImage)
    public float swipeDuration = 2f; // Duration of the mask reveal
    public float revealDelay = 0.5f; // Delay before the swipe starts (optional)

    private Vector3 initialScale;    // Initial scale of the mask (to keep it consistent)
    private float initialRotation;   // Initial rotation of the mask

    private void Start()
    {
        // Ensure the mask is fully covering the panel at the start
        if (revealMaskImage != null)
        {
            revealMaskImage.fillAmount = 1f;  // Start fully covered
            revealMaskImage.gameObject.SetActive(true);
        }

        // Wait for the delay and then start the reveal
        StartCoroutine(ClockwiseReveal());

        //StartCoroutine(HorizontalWipe(canvasPanel,  swipeDuration));
    }

    private IEnumerator ClockwiseReveal()

    
    {
        // Wait for the reveal delay before starting the swipe
        yield return new WaitForSeconds(revealDelay);

        // Start the mask reveal (clockwise)
        float elapsedTime = 0f;
        float startFillAmount = revealMaskImage.fillAmount;
        float endFillAmount = 0f; // We want to reveal the full panel by reducing the fillAmount

        while (elapsedTime < swipeDuration)
        {
            float t = elapsedTime / swipeDuration;
            revealMaskImage.fillAmount = Mathf.Lerp(startFillAmount, endFillAmount, t);  // Gradually reduce the fillAmount
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the mask is fully removed at the end
        revealMaskImage.fillAmount = endFillAmount;
    }


    public IEnumerator HorizontalWipe(RectTransform mask, float duration)
    {
        float elapsedTime = 0f;
        Vector2 startSize = mask.sizeDelta;
        Vector2 targetSize = new Vector2(0, mask.sizeDelta.y); // Start completely hidden, then wipe right

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            mask.sizeDelta = Vector2.Lerp(startSize, targetSize, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        mask.sizeDelta = targetSize;
    }

    
    // You can call this method to trigger the clockwise reveal manually
    public void TriggerClockwiseReveal()
    {
        StopAllCoroutines(); // Stop any previous reveal animations
        StartCoroutine(ClockwiseReveal());
    }
}
