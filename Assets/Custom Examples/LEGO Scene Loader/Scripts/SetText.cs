using UnityEngine;
using TMPro; // Ensure you have the TextMeshPro namespace

public class SetText : MonoBehaviour
{
    [SerializeField] private TMP_Text textField; // Reference to the TextMeshPro field

    public string  newText; 

    // Start is called before the first frame update
    void Start()
    {
            SetFieldText(newText);
    }


    public void SetFieldText(string newText)
    {
        // Example: Set initial text
        if (textField != null)
        {
            textField.text = newText;
        }
        else
        {
            Debug.LogError("TextMeshPro field is not assigned!", this);
        }
    }
}
