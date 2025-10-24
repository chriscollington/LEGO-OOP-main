using System.Collections;
using UnityEngine;

public class QuizController : MonoBehaviour
{
    private QuestionCollection questionCollection;
    private QuizQuestion currentQuestion;
    private UIController uiController;

    [SerializeField]
    private float delayBetweenQuestions = 3f;

    private void Awake()
    {
        questionCollection = FindObjectOfType<QuestionCollection>();
        
        uiController = FindObjectOfType<UIController>();
    }

    private void Start()
    {
        PresentQuestion();
        //QuizScoreManager.Instance.SetTotalQuestions(10);


    }

   private void PresentQuestion()
    {
        currentQuestion = questionCollection.GetUnaskedQuestion();

        if (currentQuestion != null)
        {
            // Increment total questions for each presented question
            //QuizScoreManager.Instance.IncrementTotalQuestions();
            uiController.SetupUIForQuestion(currentQuestion);
        }
        else
        {
            Debug.Log("All questions have been answered!");
            // Optionally show a final score screen
            uiController.DisplayFinalScore(); // Show final score screen
            Debug.Log($"Final Score: {QuizScoreManager.Instance.GetScoreString()}");
        }
    }



    public void SubmitAnswer(int answerNumber)
    {
        bool isCorrect = answerNumber == currentQuestion.CorrectAnswer;

        /*
        if (isCorrect)
        {
            QuizScoreManager.Instance.IncrementCorrectAnswers(); // Increment correct answers
        }
        */

        // Track the index of the current question
        int currentQuestionIndex = questionCollection.GetQuestionIndex(currentQuestion);

        if (!isCorrect)
        {
            questionCollection.MarkQuestionAsIncorrect(currentQuestionIndex);
        }

        uiController.HandleSubmittedAnswer(isCorrect);

        StartCoroutine(ShowNextQuestionAfterDelay());
    }


    private IEnumerator ShowNextQuestionAfterDelay()
    {
        yield return new WaitForSeconds(delayBetweenQuestions);
        PresentQuestion();
    }


    public void ResetQuiz()
    {
        // Reset score and question states
        QuizScoreManager.Instance.ResetScore();
        questionCollection.ResetQuiz();

        // Reset the UI
        uiController.ResetUI();

        // Restart the quiz
        PresentQuestion();
    }

}

