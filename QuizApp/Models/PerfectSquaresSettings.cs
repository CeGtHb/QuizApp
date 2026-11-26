namespace QuizApp.Models;

public class PerfectSquaresSettings
{
    public int Min { get; set; } = 1;

    public int Max { get; set; } = 25;

    public QuestionOrder Order { get; set; } = QuestionOrder.Random;
}