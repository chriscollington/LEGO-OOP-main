
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestionCollection : MonoBehaviour
{
    [Header("ScriptableObject for Questions (optional)")]
    public QuestionData[] scriptableQuestions; // Array of scriptable objects

    [Header("Options")]
        [Tooltip("If enabled, any incorrectly answered questions will be asked again until answered correctly.")]
    public bool loopQuestions = true; // Whether to loop the questions
    public bool randomOrder = true;  // Whether to show questions in random order

    private List<QuizQuestion> allQuestions = new List<QuizQuestion>();
    private List<int> incorrectQuestionIndices = new List<int>(); // Track indices of incorrectly answered questions

    private int currentIndex = 0; // Tracks the index of the current question
    internal static readonly object Instance;

    public void MarkQuestionAsIncorrect(int index)
    {
        if (!incorrectQuestionIndices.Contains(index))
        {
            incorrectQuestionIndices.Add(index);
        }
    }

    public int GetQuestionIndex(QuizQuestion question)
    {
        return allQuestions.IndexOf(question);
    }

   private void Awake()
    {
        LoadQuestionsFromScriptableObjects();

        if (randomOrder)
        {
            ShuffleQuestions();
        }

        // Set the total number of unique questions
        QuizScoreManager.Instance.InitializeTotalQuestions(scriptableQuestions.Length);
    }

    private void LoadQuestionsFromScriptableObjects()
    {
        if (scriptableQuestions.Length > 0)
        {
            allQuestions = scriptableQuestions.Select(scriptableQuestion =>
                new QuizQuestion
                {
                    Question = scriptableQuestion.question,
                    Answers = scriptableQuestion.answers.Select(a => a.answerText).ToArray(),
                    CorrectAnswer = scriptableQuestion.correctAnswer,
                    QuestionImage = scriptableQuestion.questionImage                }).ToList();
                /*
                new QuizQuestion
                {
                    Question = scriptableQuestion.question,
                    Answers = scriptableQuestion.answers.Select(a => a.answerText).ToArray(),
                    CorrectAnswer = scriptableQuestion.correctAnswer,
                    QuestionImage = scriptableQuestion.questionImage,
                    AnswerImages = scriptableQuestion.answers.Select(a => a.answerImage).ToArray()
                }).ToList();
                */
        }
        else
        {
            Debug.LogWarning("No questions available! Please assign ScriptableObjects.");
        }
    }

    private void ShuffleQuestions()
    {
        allQuestions = allQuestions.OrderBy(_ => Random.value).ToList();
    }
    public QuizQuestion GetUnaskedQuestion()
    {
    // If we are at the end of the main question list
        if (currentIndex >= allQuestions.Count)
        {
            // If looping is enabled and there are incorrect questions
            if (loopQuestions && incorrectQuestionIndices.Count > 0)
            {
                // Present the incorrectly answered questions
                var incorrectQuestions = incorrectQuestionIndices.Select(i => allQuestions[i]).ToList();
                incorrectQuestionIndices.Clear(); // Clear the incorrect list after loading
                allQuestions.AddRange(incorrectQuestions);

                return allQuestions[currentIndex++];
            }

            // No more questions and no looping
            return null;
        }

        return allQuestions[currentIndex++];
    }

    private void ResetQuestions()
    {
        currentIndex = 0;
        incorrectQuestionIndices.Clear();

        if (randomOrder)
        {
            ShuffleQuestions();
        }
    }

    public void ResetQuiz()
    {
        ResetQuestions();
    }

}
