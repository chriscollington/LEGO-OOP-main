using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro; // Required for TextMesh Pro




public class MoveToTargetCoroutine : MonoBehaviour
{
    // The target position to move towards
    public RectTransform uiTarget;

    // Speed of movement (duration for the move)
    public float moveDuration = 1f;
    public float movementSpeed = 2f;

   // Find or reference your TextMeshProUGUI component
   public  TMP_Text textMeshPro;

    // Reference to the sprite's RectTransform
    private RectTransform rectTransform;

    public CoinManager coinManager;  // Reference to the CoinManager to update the coin count


    void Start()
    {
        // Get the RectTransform of this GameObject
        rectTransform = GetComponent<RectTransform>();

        // Start the coroutine to move the sprite to the target
        if (uiTarget != null)
        {
            StartCoroutine(Move2DCoinAlongCurve());
            
        }
    }

    private IEnumerator Move2DCoinAlongCurve()
{
    //RectTransform rectTransform = coin.GetComponent<RectTransform>();

    // Get the target position in screen space
    Vector2 startPosition = rectTransform.position;
    Vector2 targetPosition = uiTarget.GetComponent<RectTransform>().position;

    // Define a control point for the Bezier curve in the same plane
    Vector2 controlPoint = (startPosition + targetPosition) / 2 + Vector2.up * 100f;

    float distance = Vector2.Distance(startPosition, targetPosition);
    float duration = distance / movementSpeed; // Adjust duration based on speed
    float elapsedTime = 0;

    // Debugging the duration and initial conditions
    // Debug.Log($"Start Position: {startPosition}, Target Position: {targetPosition}, Duration: {duration}");

    // Ensure the loop runs at least for a small amount of time, regardless of the speed
    while (elapsedTime < duration)
    {
        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / duration); // Ensure `t` stays in [0, 1]

        // Calculate the position on the quadratic Bezier curve
        Vector3 positionOnCurve = Mathf.Pow(1 - t, 2) * startPosition +
                                  2 * (1 - t) * t * controlPoint +
                                  Mathf.Pow(t, 2) * targetPosition;

        // Update the RectTransform position
        rectTransform.position = positionOnCurve;

        // Debugging the position and time progress
        // Debug.Log($"ElapsedTime: {elapsedTime}, t: {t}, PositionOnCurve: {positionOnCurve}, Distance to Target: {Vector3.Distance(rectTransform.position, targetPosition)}");

        // Yield here to ensure the loop runs every frame
        yield return null;
    }

    // Ensure the coin reaches the exact target position after exiting the loop
    rectTransform.position = targetPosition;

    // Destroy the 2D coin after it reaches the target
    Destroy(this);

    // Call the CoinManager's CollectCoin method to update the coin count and text
    if (coinManager != null)
    {
        coinManager.CollectCoin();  // Increment coin count and update UI
    }
    else
    {
        Debug.LogError("CoinManager reference is missing!");
    }
    }

}
