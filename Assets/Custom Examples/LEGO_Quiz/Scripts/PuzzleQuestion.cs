using UnityEngine;

public class PuzzleQuestion
{
    public string Question { get; set; }
    public string[] Boxes { get; set; }
    public int[] Answer { get; set; }
    public bool Asked { get; set; }

    // New property for an optional image associated with the question
    public Sprite QuestionImage { get; set; }
}