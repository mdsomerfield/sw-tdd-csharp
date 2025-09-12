namespace Wordle.Api.Domains.Wordle;

public class GuessRequest
{
    public string Guess { get; set; } = string.Empty;
    public GameState GameState { get; set; } = new();
}

public class GameState
{
    public List<string> Guesses { get; set; } = new();
    public List<string> Feedbacks { get; set; } = new();
    public string Answer { get; set; } = "WHALE";
}