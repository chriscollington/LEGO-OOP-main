using UnityEngine;

[CreateAssetMenu(fileName = "NewQuestion", menuName = "Quiz/Question")]
public class QuestionData : ScriptableObject
{
    [Header("Question")]
    [TextArea]
    public string question; // The question text
    public Sprite questionImage; // Optional image for the question

    [Header("Answers")]
    public AnswerData[] answers; // List of possible answers

    public int correctAnswer; // The index of the correct answer in the array

    // Optional: Validate correctAnswer index
    private void OnValidate()
    {
        if (correctAnswer < 0 || correctAnswer >= answers.Length)
        {
            Debug.LogWarning($"Correct answer index is out of bounds for question: {question}");
        }
    }
}

[System.Serializable]
public class AnswerData
{
    public string answerText; // Text of the answer
    public Sprite answerImage; // Optional image for the answer
}
