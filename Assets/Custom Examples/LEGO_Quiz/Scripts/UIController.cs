using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI; // Required for TextMesh Pro components

public class UIController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject questionContainer; // Parent GameObject containing all answer buttons

    [SerializeField] private TMP_Text questionText; // For the question text
    [SerializeField] private Image questionImage; // For the question image
    [SerializeField] private GameObject answerButtonContainer; // Parent GameObject containing all answer buttons
    [SerializeField] private GameObject correctAnswerPopup; // Correct answer popup
    [SerializeField] private GameObject wrongAnswerPopup; // Wrong answer popup

    [Header("Score UI")]
    [SerializeField] public TMP_Text scoreText; // Text UI element for displaying score


    [Header("Final Score Screen")]
    [SerializeField] private GameObject finalScoreScreen; // GameObject displayed when all questions are answered
    [SerializeField] private TMP_Text finalScoreText; // TMP_Text to show final score on final score screen

    private Button[] answerButtons; // Array to store references to answer buttons

    private void Awake()
    {
        // Automatically populate answerButtons from children of the container
        answerButtons = answerButtonContainer.GetComponentsInChildren<Button>();
    }

    private void Update()
    {
        // Update score every frame (optional, could be triggered when score changes)
        UpdateScoreDisplay();
    }

    // Update the score UI
    private void UpdateScoreDisplay()
    {
        // Access the QuizScoreManager instance to get current score
        string scoreString = QuizScoreManager.Instance.GetScoreString();
        if (scoreText != null)
        {
            scoreText.text = "Score: " + scoreString; // Display score in the UI Text field
        }
        else
        {
            Debug.LogError("ScoreText UI element is not assigned.");
        }
    }


    public void SetupUIForPuzzle(PuzzleQuestion question)
    {
        // Hide the final score screen in case it's shown
        finalScoreScreen.SetActive(false);

        correctAnswerPopup.SetActive(false);
        wrongAnswerPopup.SetActive(false);

        questionText.text = question.Question;

        // Set question image if available (optional)
        if (questionImage != null)
        {
            questionImage.sprite = question.QuestionImage; // Set the image for the question
            questionImage.gameObject.SetActive(true); // Enable the image object
        }
        else
        {
            questionImage.gameObject.SetActive(false); // Hide the image object if no image is set
        }
    }

    public void SetupUIForQuestion(QuizQuestion question)
    {
        // Hide the final score screen in case it's shown
        finalScoreScreen.SetActive(false);

        correctAnswerPopup.SetActive(false);
        wrongAnswerPopup.SetActive(false);

        questionText.text = question.Question;

        // Set question image if available (optional)
        if (questionImage != null)
        {
            questionImage.sprite = question.QuestionImage; // Set the image for the question
            questionImage.gameObject.SetActive(true); // Enable the image object
        }
        else
        {
            questionImage.gameObject.SetActive(false); // Hide the image object if no image is set
        }

        if (question != null){
        // Set answers
        for (int i = 0; i < question.Answers.Length; i++)
        {
            var answerText = answerButtons[i].GetComponentInChildren<TMP_Text>(); // Fetch TMP_Text from children
            if (answerText != null)
            {
                answerText.text = question.Answers[i]; // Set answer text
            }
            else
            {
                Debug.LogError($"Button {answerButtons[i].name} is missing a TMP_Text component in its children.");
            }

            answerButtons[i].gameObject.SetActive(true); // Activate button
        }

        // Hide any extra buttons if the question has fewer answers
        for (int i = question.Answers.Length; i < answerButtons.Length; i++)
        {
            answerButtons[i].gameObject.SetActive(false);
        }
        }
    }

    public void HandleSubmittedAnswer(bool isCorrect)
    {
        ToggleAnswerButtons(false);
        if (isCorrect)
        {
            ShowCorrectAnswerPopup();
            QuizScoreManager.Instance.IncrementCorrectAnswers(); // Increment correct answers when answer is correct
        }
        else
        {
            ShowWrongAnswerPopup();
        }

        //QuizScoreManager.Instance.IncrementTotalQuestions(); 
        // Increment total questions after each question
        UpdateScoreDisplay(); // Update score display after answer is submitted
    }

    private void ToggleAnswerButtons(bool value)
    {
        foreach (var button in answerButtons)
        {
            button.gameObject.SetActive(value);
        }
    }


    private void ShowCorrectAnswerPopup()
    {
        correctAnswerPopup.SetActive(true);
    }

    private void ShowWrongAnswerPopup()
    {
        wrongAnswerPopup.SetActive(true);
    }

    public void DisplayFinalScore()
    {
        ToggleAnswerButtons(false); // Disable answer buttons
        correctAnswerPopup.SetActive(false);
        wrongAnswerPopup.SetActive(false);

        // Show the final score screen
        finalScoreScreen.SetActive(true);
        questionContainer.SetActive(false);


        // Update the final score text
        if (finalScoreText != null)
        {
            finalScoreText.text = $"Final Score: {QuizScoreManager.Instance.GetScoreString()}";
        }
        else
        {
            Debug.LogError("Final Score Text is not assigned in the UIController.");
        }
    }


    public void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = QuizScoreManager.Instance.GetScoreString();
        }
        else
        {
            Debug.LogError("ScoreText UI element is not assigned.");
        }
    }

    public void ResetUI()
    {
        // Hide all answer buttons
        foreach (var button in answerButtons)
        {
            button.gameObject.SetActive(false);
        }

        // Hide popups
        correctAnswerPopup.SetActive(false);
        wrongAnswerPopup.SetActive(false);

        // Reset the question and score UI
        questionText.text = "";
        if (questionImage != null)
        {
            questionImage.gameObject.SetActive(false);
        }

        UpdateScoreDisplay();

        // Hide final score screen
        finalScoreScreen.SetActive(false);
        questionContainer.SetActive(true);
    }


    
}
