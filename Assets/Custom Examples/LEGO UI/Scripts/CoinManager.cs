using TMPro;  // Required for TextMesh Pro
using Unity.LEGO.Game;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    // Reference to the TextMesh Pro text component
    public TMP_Text coinText;

    // Reference to the Variable object that holds the coin count
    [SerializeField]
    public Variable coinVariable; // This will be the variable being modified by CounterAction

    // Reference to the CounterAction script to modify the variable in that script
    //public CounterAction counterAction;

    // Variable to hold the number of coins collected
    private int coinCount = 0;

    // Method to increment the coin count and update the text

     protected  void Start()
        {
            // set positon to the coinvariable

            VariableManager.RegisterVariable(coinVariable);
        }

    public void CollectCoin()
    {
        coinCount++;  // Increment coin count
        UpdateCoinText();  // Update the text to show the new coin count

         // Update the coinVariable with the new coin count
        if (coinVariable != null)
        {
           // VariableManager.SetValue(coinVariable,VariableManager.GetValue(coinVariable) + coinCount);
           VariableManager.SetValue(coinVariable,coinCount);
        }
        else
        {
            Debug.LogError("Coin Variable is not assigned in the inspector!");
        }
    }

    // Method to update the coin text in the UI
    private void UpdateCoinText()
    {
        if (coinText != null)
        {
            coinText.text = "Coins Collected: " + coinCount;  // Update the UI text
        }
        else
        {
            Debug.LogError("Coin Text is not assigned in the inspector!");
        }

       
    }
}
