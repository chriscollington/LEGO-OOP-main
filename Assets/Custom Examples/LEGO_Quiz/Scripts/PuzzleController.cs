using System.Collections;
using UnityEngine;

public class PuzzleController : MonoBehaviour
{
    private PuzzleCollection puzzleCollection;
    private PuzzleQuestion currentQuestion;
    private UIController uiController;

    [SerializeField]
    private float delayBetweenQuestions = 3f;

    private void Awake()
    {
        puzzleCollection = FindObjectOfType<PuzzleCollection>();
        
        uiController = FindObjectOfType<UIController>();
    }

    private void Start()
    {
        PresentQuestion();
        //QuizScoreManager.Instance.SetTotalQuestions(10);


    }

   private void PresentQuestion()
    {
        currentQuestion = puzzleCollection.GetUnaskedQuestion();

        if (currentQuestion != null)
        {
            // Increment total questions for each presented question
            //QuizScoreManager.Instance.IncrementTotalQuestions();
            uiController.SetupUIForPuzzle(currentQuestion);
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
        //bool isCorrect = answerNumber == currentQuestion.Answer;
        bool isCorrect = true;
        /*
        if (isCorrect)
        {
            QuizScoreManager.Instance.IncrementCorrectAnswers(); // Increment correct answers
        }
        */

        // Track the index of the current question
        int currentQuestionIndex = puzzleCollection.GetQuestionIndex(currentQuestion);

        if (!isCorrect)
        {
            puzzleCollection.MarkQuestionAsIncorrect(currentQuestionIndex);
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
        puzzleCollection.ResetQuiz();

        // Reset the UI
        uiController.ResetUI();

        // Restart the quiz
        PresentQuestion();
    }

}

