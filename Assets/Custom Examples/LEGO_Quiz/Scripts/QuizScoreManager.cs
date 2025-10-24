using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QuizScoreManager : MonoBehaviour
{
    private int totalQuestions = 0;
    private int correctAnswers = 0;

    private HashSet<int> answeredQuestions = new HashSet<int>(); // Track questions that have been answered (using their index or unique identifier)

    public int TotalQuestions => totalQuestions; // Expose total questions (read-only)
    public int CorrectAnswers => correctAnswers; // Expose correct answers (read-only)

    // Singleton pattern (optional if only one instance of ScoreManager is needed)
    private static QuizScoreManager instance;

    public static QuizScoreManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<QuizScoreManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject); // Enforce singleton
        }
    }

    /// <summary>
    /// Set the total number of questions at the start of the quiz.
    /// </summary>
    /// <param name="total">Total number of questions in the quiz.</param>
    public void SetTotalQuestions(int total)
    {
        totalQuestions = total;
    }

    /// <summary>
    /// Mark a question as answered and increment the number of correct answers if applicable.
    /// </summary>
    /// <param name="questionID">Unique identifier for the question.</param>
    /// <param name="isCorrect">Whether the answer is correct.</param>
    public void AnswerQuestion(int questionID, bool isCorrect)
    {
        if (!answeredQuestions.Contains(questionID))
        {
            answeredQuestions.Add(questionID);

            if (isCorrect)
            {
                IncrementCorrectAnswers();
            }
        }
    }


    public void IncrementTotalQuestions()
    {
        totalQuestions++;
    }

     /// <summary>
    /// Increment the number of correct answers.
    /// </summary>
    public void IncrementCorrectAnswers()
    {
        correctAnswers++;
    }

    /// <summary>
    /// Increment the total number of questions presented.
    /// </summary>
    public void InitializeTotalQuestions(int total)
    {
        totalQuestions = total;
    }

     /// <summary>
    /// Reset the score.
    /// </summary>
    public void ResetScore()
    {
        //totalQuestions = 0;
        correctAnswers = 0;
        answeredQuestions.Clear();

        //QuestionCollection.Instance.ResetQuiz();

    }
    /// <summary>
    /// Get a formatted score string, e.g., "3/10".
    /// </summary>
    public string GetScoreString()
    {
        return $"{correctAnswers}/{totalQuestions}";
    }

    /// <summary>
    /// Check if all questions have been answered correctly.
    /// </summary>
    public void CheckAllCorrect()
    {
        if (correctAnswers == totalQuestions && totalQuestions > 0)
        {
            Debug.Log("All questions answered correctly!");
        }
    }

   
}
