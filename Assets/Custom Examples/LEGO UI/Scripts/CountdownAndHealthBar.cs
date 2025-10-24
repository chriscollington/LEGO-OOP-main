using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Import TextMeshPro namespace
using System.Collections;
using Unity.LEGO.Minifig;
using Unity.LEGO.Game;

public class CountdownAndHealthBar : MonoBehaviour
{
    [Header("Settings")]
    public float countdownSpeed = 1f; // Speed of the countdown (value decrease per second)
    public float recoverySpeed = 10f; // Speed of the health bar recovery (value increase per second)

    [SerializeField]
    public MinifigController m_MinifigController;

    [Header("Health Panel")]
    public GameObject healthPanel; // The health UI Canvas object
    public TextMeshProUGUI countdownText; // The TextMeshPro field in the health UI
    public Slider healthBar; // The health bar slider
    public float currentHealth = 100f; // Current health value

    [Header("Warning Panel")]
    public GameObject warningPanel; // The warning UI Canvas object
    public TextMeshProUGUI warningPanelText; // The TextMeshPro field in the warning UI
    public string message; // The warning text displayed in the UI
    public Image warningImage; // Image in the warning UI
    public Sprite warningIcon; // Sprite to set for the warning image in the Inspector

    
    private bool playerInside = false; // Is the player touching the object?
    private bool hasLost = false; // Flag to ensure Lose() is only called once
    private Coroutine warningCoroutine; // Coroutine for delaying warning panel


    private void Start()
    {
        
        if (warningPanelText != null)
            warningPanelText.text = message;

        if (warningImage != null && warningIcon != null)
            warningImage.sprite = warningIcon;

        if (healthPanel != null)
            healthPanel.SetActive(false); // Ensure the canvas is hidden at start
       
    }

    private void Update()
    {
        if (playerInside)
        {
            // Decrease health if the player is inside the collider
            currentHealth -= countdownSpeed * Time.deltaTime;
            currentHealth = Mathf.Clamp(currentHealth, 0, 100); // Clamp between 0 and 100

            if (currentHealth <= 0 && !hasLost)
            {
                Lose(); // Call the lose function when health reaches 0
                hasLost = true; // Prevent multiple calls to Lose()
            }
        }
        else if (currentHealth < 100f && !hasLost)
        {
            // Slowly recover health if the player is outside the collider
            currentHealth += recoverySpeed * Time.deltaTime;
            currentHealth = Mathf.Clamp(currentHealth, 0, 100); // Clamp between 0 and 100
        }

        // Hide the health panel only when health has fully recovered
        if (!playerInside && currentHealth >= 100f && healthPanel.activeSelf)
        {
            healthPanel.SetActive(false);
        }

        // Update UI elements
        if (healthPanel != null && countdownText != null)
            countdownText.text = Mathf.RoundToInt(currentHealth).ToString();

        if (healthBar != null)
            healthBar.value = currentHealth / 100f; // Normalize for slider (0 to 1)
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure the collider belongs to the player
        {
            playerInside = true;
            Debug.Log("hit" + playerInside);

            if (healthPanel != null && !healthPanel.activeSelf)
                healthPanel.SetActive(true); // Show the health panel

            if (warningPanel != null)
            {
                if (warningCoroutine != null)
                {
                    StopCoroutine(warningCoroutine); // Cancel any previous coroutine to prevent early disabling
                }
                warningPanel.SetActive(true); // Show the warning panel
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            Debug.Log("hit" + playerInside);

            if (warningPanel != null)
            {
                if (warningCoroutine != null)
                {
                    StopCoroutine(warningCoroutine); // Cancel any previous coroutine
                }
                warningCoroutine = StartCoroutine(HideWarningPanelAfterDelay(2f)); // Start the delay coroutine
            }
        }
    }

    private IEnumerator HideWarningPanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay
        warningPanel.SetActive(false); // Hide the warning panel
    }

    private void Lose()
    {
        Debug.Log("You Lose!"); // Log for debugging; replace with actual lose logic
        // Add your lose logic here, e.g., load a "Game Over" scene or show a lose UI

        m_MinifigController.Explode();

        GameOverEvent evt = Events.GameOverEvent;
        evt.Win = false;
        EventManager.Broadcast(evt);

    }

    private void Win()
    {
        Debug.Log("You Win!"); // Log for debugging; replace with actual win logic
        // Add your win logic here, e.g., load a "Game Over" scene or show a win UI

        m_MinifigController.SpecialAnimationFinished();

        GameOverEvent evt = Events.GameOverEvent;
        evt.Win = true;
        EventManager.Broadcast(evt);

    }
}
