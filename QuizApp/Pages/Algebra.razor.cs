using Microsoft.AspNetCore.Components;
using QuizApp.Models;

namespace QuizApp.Pages;

public partial class Algebra : ComponentBase
{
    private List<Question> questions = new();
    private Question currentQuestion = new();

    private int questionIndex = 0;
    private bool showAnswer = false;
    private bool isFinished = false;

    protected override void OnInitialized()
    {
        questions = Enumerable.Range(1, 25)
            .Select(number => new Question
            {
                Text = $"Cât este {number}²?",
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
    
}
