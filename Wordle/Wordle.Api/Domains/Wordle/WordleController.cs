using Microsoft.AspNetCore.Mvc;

namespace Wordle.Api.Domains.Wordle;

[ApiController]
[Route("wordle")]
public class WordleController : ControllerBase
{
    [HttpPost("guess")]
    public GameState MakeGuess(GuessRequest request)
    {
        var guess = request.Guess.ToUpper();
        var answer = request.GameState.Answer.ToUpper();
        
        var feedback = GenerateFeedback(guess, answer);
        
        var updatedGameState = new GameState
        {
            Guesses = new List<string>(request.GameState.Guesses) { guess },
            Feedbacks = new List<string>(request.GameState.Feedbacks) { feedback },
            Answer = request.GameState.Answer
        };
        
        return updatedGameState;
    }
    
    private static string GenerateFeedback(string guess, string answer)
    {
        if (guess.Length != 5 || answer.Length != 5)
            throw new ArgumentException("Both guess and answer must be 5 characters long");
            
        var feedback = new char[5];
        var answerChars = answer.ToCharArray();
        
        // First pass: mark exact matches (green 'g')
        for (int i = 0; i < 5; i++)
        {
            if (guess[i] == answer[i])
            {
                feedback[i] = 'g';
                answerChars[i] = '*'; // Mark as used
            }
        }
        
        // Second pass: mark partial matches (yellow 'o') for non-exact matches
        for (int i = 0; i < 5; i++)
        {
            if (feedback[i] != 'g') // Not already an exact match
            {
                // Look for this character in unused positions of the answer
                for (int j = 0; j < 5; j++)
                {
                    if (answerChars[j] == guess[i])
                    {
                        feedback[i] = 'o';
                        answerChars[j] = '*'; // Mark as used
                        break;
                    }
                }
                
                // If not found anywhere, mark as incorrect (gray '-')
                if (feedback[i] == '\0')
                {
                    feedback[i] = '-';
                }
            }
        }
        
        return new string(feedback);
    }
}