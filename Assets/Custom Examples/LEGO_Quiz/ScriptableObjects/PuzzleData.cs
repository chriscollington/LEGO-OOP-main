using UnityEngine;

[CreateAssetMenu(fileName = "NewPuzzleData", menuName = "Puzzle/PuzzleData")]
public class PuzzleData : ScriptableObject
{

    [Header("Question")]
    [TextArea]
    public string question; // The question text
    public Sprite questionImage; // Optional image for the question

    [System.Serializable]
    public class BoxData
    {
        public string boxName;         // Name for the box
        public Sprite boxImage;        // Image for the box (optional)
        public string boxText;         // Text for the box (optional)
        public Color boxColor;         // Background color for the box (optional)
    }

    public BoxData[] boxes;            // Array of box details
    public int[] correctOrder;         // Solution as an array of indices
}
