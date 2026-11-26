using Microsoft.AspNetCore.Components;
using QuizApp.Models;

namespace QuizApp.Pages;

public partial class PerfectSquares : ComponentBase
{
    private List<Question> questions = new();
    private Question currentQuestion = new();

    private int questionIndex;
    private bool showAnswer;
    private bool isFinished;

    private bool isTheory;
    private bool showTheoryAnswer;

    protected override void OnInitialized()
    {
        questions = Enumerable.Range(1, 25)
            .Select(number => new Question
            {
                Text = $"{number}^2 = ?",
                CorrectAnswer = number * number
            })
            .OrderBy(_ => Random.Shared.Next())
            .ToList();

        currentQuestion = questions[questionIndex];
    }

    private void RevealAnswer()
    {
        showAnswer = true;
    }

    private void NextQuestion()
    {
        questionIndex++;

        if (questionIndex >= questions.Count)
        {
            isFinished = true;
            return;
        }

        currentQuestion = questions[questionIndex];
        showAnswer = false;
    }

    private void RestartQuiz()
    {
        questions = questions
            .OrderBy(_ => Random.Shared.Next())
            .ToList();

        questionIndex = 0;
        currentQuestion = questions[questionIndex];

        showAnswer = false;
        isFinished = false;
    }

    private void ShowExercises()
    {
        isTheory = false;
    }

    private void ShowTheory()
    {
        isTheory = true;
        showTheoryAnswer = false;
    }

    private void ToggleTheoryAnswer()
    {
        showTheoryAnswer = !showTheoryAnswer;
    }
}