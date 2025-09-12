using FluentAssertions;
using Wordle.Api.Domains.Wordle;
using Wordle.Api.Tests.Framework;
using Xunit;

namespace Wordle.Api.Tests.Tests;

[Collection("ApiTest")]
public class WordleGameTests
{
    private readonly ApiTestFixture _fixture;

    public WordleGameTests(ApiTestFixture fixture) => _fixture = fixture;

    [Fact]
    public async Task MakeGuess_ShouldReturnUpdatedGameState_WithFeedback()
    {
        var client = _fixture.CreateJsonClient();
        
        var request = new GuessRequest
        {
            Guess = "WEARY",
            GameState = new GameState
            {
                Guesses = new List<string>(),
                Feedbacks = new List<string>(),
                Answer = "WHALE"
            }
        };
        
        var response = await client.PostAsync<GuessRequest, GameState>("/wordle/guess", request);
        
        response.Should().NotBeNull();
        response.Guesses.Should().HaveCount(1);
        response.Guesses[0].Should().Be("WEARY");
        response.Feedbacks.Should().HaveCount(1);
        response.Feedbacks[0].Should().Be("gog--");
        response.Answer.Should().Be("WHALE");
    }

    [Fact]
    public async Task MakeGuess_WithExistingGuesses_ShouldAppendNewGuess()
    {
        var client = _fixture.CreateJsonClient();
        
        var request = new GuessRequest
        {
            Guess = "WHALE",
            GameState = new GameState
            {
                Guesses = new List<string> { "WATER", "WEARY" },
                Feedbacks = new List<string> { "go-o-", "gog--" },
                Answer = "WHALE"
            }
        };
        
        var response = await client.PostAsync<GuessRequest, GameState>("/wordle/guess", request);
        
        response.Should().NotBeNull();
        response.Guesses.Should().HaveCount(3);
        response.Guesses[2].Should().Be("WHALE");
        response.Feedbacks.Should().HaveCount(3);
        response.Feedbacks[2].Should().Be("ggggg");
        response.Answer.Should().Be("WHALE");
    }

    [Fact]
    public async Task MakeGuess_ShouldHandleCaseInsensitiveInput()
    {
        var client = _fixture.CreateJsonClient();
        
        var request = new GuessRequest
        {
            Guess = "whale",
            GameState = new GameState
            {
                Guesses = new List<string>(),
                Feedbacks = new List<string>(),
                Answer = "WHALE"
            }
        };
        
        var response = await client.PostAsync<GuessRequest, GameState>("/wordle/guess", request);
        
        response.Should().NotBeNull();
        response.Guesses.Should().HaveCount(1);
        response.Guesses[0].Should().Be("WHALE");
        response.Feedbacks.Should().HaveCount(1);
        response.Feedbacks[0].Should().Be("ggggg");
    }
}

