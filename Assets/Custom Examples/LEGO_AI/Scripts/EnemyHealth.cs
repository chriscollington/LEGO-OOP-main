using UnityEngine;
using TMPro; // Import TextMeshPro namespace

namespace Unity.LEGO.Minifig
{
    public class EnemyHealth : MonoBehaviour
    {
        public int currentHealth = 100;

        public PlayerUIHandler playerUIHandler;

    private bool playerInside = false; // Is the player touching the object?


        EnemyAI enemyAI;

        private GameObject currentTarget;

        private void Update()
        {
        
        }


        public void TakeDamage(int amount)
        {
            currentHealth -= amount;
            currentHealth = Mathf.Max(currentHealth, 0); // Ensure health doesn't go below zero
            Debug.Log($"Health: {currentHealth}");

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Debug.Log("Target has died.");
            // Add death logic here (e.g., destroy the GameObject, play death animation, etc.)
            Destroy(gameObject);
        }

    
        private void OnTriggerEnter(Collider other)
        {
        // Set the current target to the object this GameObject has collided with
            currentTarget = other.gameObject;
            
            if (other.CompareTag("Player"))
            {
            Debug.Log("Player entered trigger with enemy: " + other.gameObject.name);
            playerUIHandler.playerInside = true;
            // Show the warning panel
            playerUIHandler.ShowWarningPanel();
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerUIHandler.playerInside = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Player exited trigger with enemy: " + other.gameObject.name);
                playerUIHandler.playerInside = false;    
                playerUIHandler.HideWarningPanel();
            }
        }
    }
}