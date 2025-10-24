using UnityEngine;
using TMPro;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance;

    private PuzzleData puzzleData; // Set dynamically by PuzzleGenerator
    private Transform[] slots;    // Set dynamically
    private Transform[] boxes;    // Set dynamically

    public TMP_Text feedbackText; // Optional feedback text


    private void Awake()
    {
        Instance = this;
    }

    public void Initialize(PuzzleData data, Transform[] generatedSlots, Transform[] generatedBoxes)
    {
        puzzleData = data;
        slots = generatedSlots;
        boxes = generatedBoxes;
    }

    public void CheckSolution()
    {
        for (int i = 0; i < puzzleData.correctOrder.Length; i++)
        {
            int correctBoxIndex = puzzleData.correctOrder[i];

            // Check if the slot has the correct box in place
            if (slots[i].childCount == 0 || slots[i].GetChild(0).name != puzzleData.boxes[correctBoxIndex].boxName)
            {
                if (feedbackText != null)
                {
                    feedbackText.text = "Keep trying!";
                }
                return;
            }
        }

        if (feedbackText != null)
        {
            feedbackText.text = "Puzzle solved!";
        }
    }

    // New method to reset the puzzle


    public void ResetPuzzle()
    {
         PuzzleWireGenerator.Instance.GeneratePuzzle();
    }
}
