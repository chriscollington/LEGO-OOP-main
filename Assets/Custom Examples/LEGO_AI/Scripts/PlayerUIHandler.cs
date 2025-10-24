using UnityEngine;
using UnityEngine.UI; // Required for UI elements
using TMPro; // Import TextMeshPro namespace
using System.Collections;
using Unity.LEGO.Minifig;
using Unity.LEGO.Game;
using Unity.VisualScripting;


public class PlayerUIHandler : MonoBehaviour
{
    [Header("Settings")]
    public float countdownSpeed = 1f; // Speed of the countdown (value decrease per second)
    public float recoverySpeed = 10f; // Speed of the health bar recovery (value increase per second)

    // Countdown damage variables
    public int damagePerSecond = 5;
    
    public MinifigController m_MinifigController;

    
    [Header("Health Panel")]
    public GameObject healthPanel; // The health UI Canvas object
    // Reference to the health panel
    public Slider healthBar; // Assuming health is shown with a slider
    public TextMeshProUGUI healthText; // Text element to display health as text


   
    // Player's health
    public float currentHealth = 100f; // Current health value


    [Header("Warning Panel")]
    public GameObject warningPanel; // The warning UI Canvas object
    public TMP_Text warningPanelText; // The TextMeshPro field in the warning UI

    public string message; // The warning text displayed in the UI
    public Image warningImage; // Image in the warning UI
    public Sprite warningIcon; // Sprite to set for the warning image in the Inspector

    // Duration for which the warning panel should display
    private float warningDuration = 2f;
    private float warningTimer;

     [Header("Other")]

    public bool playerInside = false; // Is the player touching the object?
    private bool hasLost = false; // Flag to ensure Lose() is only called once

     private Coroutine warningCoroutine; // Coroutine for delaying warning panel

     private GameObject currentTarget;

    public float sightRange;
    public float attackRange = 1.5f;

    public LayerMask groundLayer, enemyLayer;


    private void Start()
    {

        if (warningPanelText != null)
            warningPanelText.text = message;

        if (warningImage != null && warningIcon != null)
            warningImage.sprite = warningIcon;

        if (healthPanel != null)
            healthPanel.SetActive(false); // Ensure the canvas is hidden at start

        if (warningPanel != null)
            warningPanel.SetActive(false); // Ensure the canvas is hidden at start

        
    }


      private void Update()
    {
        bool enemyInSightRange = Physics.CheckSphere(transform.position, sightRange, enemyLayer);
        bool enemyInAttackRange = Physics.CheckSphere(transform.position, attackRange, enemyLayer);

        if (enemyInSightRange && enemyInAttackRange)
        {
           playerInside = true;

        } else {

           playerInside = false;
        }
            
         if (playerInside == true)
            {
               ShowWarningPanel(); 
               
            }
            else 
            {
               HideWarningPanel();
            }
    }


   private  void FixedUpdate()
    {
        TakeDamage();
    }

    public void TakeDamage()
    {
         if (playerInside == true)
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
        if (healthPanel != null && healthText != null)
            healthText.text = Mathf.RoundToInt(currentHealth).ToString();

         if (healthBar != null)
            healthBar.value = currentHealth / 100f; // Normalize for slider (0 to 1)

         if (currentHealth >= 100f)
            warningPanel.SetActive(false); // Hide the warning panel   
            
    }

    
    public void ShowWarningPanel()
    {

            if (healthPanel != null && !healthPanel.activeSelf)
            {
                healthPanel.SetActive(true); // Show the health panel
            }

            if (warningPanel != null)
            {
                if (warningCoroutine != null)
                {
                    StopCoroutine(warningCoroutine); // Cancel any previous coroutine to prevent early disabling
                }
                warningPanel.SetActive(true); // Show the warning panel
            }


    }

    public void HideWarningPanel()
    {

        if (warningPanel != null)
        {
            if (warningCoroutine != null)
            {
                StopCoroutine(warningCoroutine); // Cancel any previous coroutine
            }
            warningCoroutine = StartCoroutine(HideWarningPanelAfterDelay(2f)); // Start the delay coroutine
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

        
        // Hide the panels
        healthPanel.SetActive(false);
        warningPanel.SetActive(false); // Hide the warning panel   

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
