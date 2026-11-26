using Microsoft.AspNetCore.Components;
using QuizApp.Models;
using System.Text.Json;
using Microsoft.JSInterop;

namespace QuizApp.Pages;

public partial class PerfectSquares : ComponentBase
{
    [Inject]
    private IJSRuntime JS { get; set; } = default!;

    private const string StorageKey = "quizapp.perfect-squares.settings";
    
    private PerfectSquaresSettings settings = new();
    private PerfectSquaresSettings draftSettings = new();
    private bool showSettings;

    private bool AreSettingsValid => IsValid(draftSettings);

    private static bool IsValid(PerfectSquaresSettings value)
    {
        return value.Min >= 1 &&
               value.Max <= 100 &&
               value.Min <= value.Max &&
               Enum.IsDefined(value.Order);
    }

    private List<Question> questions = new();
    private Question currentQuestion = new();

    private int questionIndex;
    private bool showAnswer;
    private bool isFinished;

    private bool isTheory;
    private bool showTheoryAnswer;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            var json = await JS.InvokeAsync<string?>(
                "localStorage.getItem", StorageKey);

            if (json is not null)
            {
                var savedSettings =
                    JsonSerializer.Deserialize<PerfectSquaresSettings>(json);

                if (savedSettings is not null && IsValid(savedSettings))
                {
                    settings = savedSettings;
                }
            }
        }
        catch (JsonException)
        {
            settings = new PerfectSquaresSettings();
        }
        catch (JSException)
        {
            settings = new PerfectSquaresSettings();
        }

        StartQuiz();
    }

    private void StartQuiz()
    {
        var numbers = Enumerable
            .Range(settings.Min, settings.Max - settings.Min + 1)
            .ToArray();

        switch (settings.Order)
        {
            case QuestionOrder.Random:
                Random.Shared.Shuffle(numbers);
                break;

            case QuestionOrder.Descending:
                Array.Reverse(numbers);
                break;
        }

        questions = numbers
            .Select(number => new Question
            {
                Text = $"{number}^2 = ?",
                CorrectAnswer = number * number
            })
            .ToList();

        questionIndex = 0;
        currentQuestion = questions[questionIndex];

        showAnswer = false;
        isFinished = false;
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
        StartQuiz();
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
    
    private void ToggleSettings()
    {
        if (showSettings)
        {
            CancelSettings();
            return;
        }

        draftSettings = new PerfectSquaresSettings
        {
            Min = settings.Min,
            Max = settings.Max,
            Order = settings.Order
        };

        showSettings = true;
    }

    private void CancelSettings()
    {
        showSettings = false;
    }

    private void ResetSettings()
    {
        draftSettings = new PerfectSquaresSettings();
    }

    private async Task ApplySettings()
    {
        if (!AreSettingsValid)
            return;

        settings = draftSettings;
        showSettings = false;

        StartQuiz();

        try
        {
            var json = JsonSerializer.Serialize(settings);

            await JS.InvokeVoidAsync(
                "localStorage.setItem", StorageKey, json);
        }
        catch (JSException)
        {
            // Quiz-ul rămâne funcțional dacă salvarea nu este disponibilă.
        }
    }
}