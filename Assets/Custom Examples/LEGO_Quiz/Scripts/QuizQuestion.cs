using UnityEngine;

public class QuizQuestion
{
    public string Question { get; set; }
    public string[] Answers { get; set; }
    public int CorrectAnswer { get; set; }
    public bool Asked { get; set; }

    // New property for an optional image associated with the question
    public Sprite QuestionImage { get; set; }
}