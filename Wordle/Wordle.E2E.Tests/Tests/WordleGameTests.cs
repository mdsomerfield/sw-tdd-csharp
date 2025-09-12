using FluentAssertions;
using Microsoft.Playwright;
using Wordle.E2E.Tests.Framework;

namespace Wordle.E2E.Tests.Tests;

public class WordleGameTests : E2ETestBase
{
    public WordleGameTests(RealServerTestFixture serverFixture) : base(serverFixture)
    {
    }

    [Fact]
    public async Task WordleGame_ShouldDisplayGameContainer_WithSixRowsAndInputForm()
    {
        await Page.GotoAsync(ServerFixture.WebBaseUrl);
        
        var gameContainer = await Page.QuerySelectorAsync(".wordle-game");
        gameContainer.Should().NotBeNull("The page should have a container with class 'wordle-game'");
        
        var rows = await Page.QuerySelectorAllAsync(".wordle-game .wordle-row");
        rows.Count.Should().Be(6, "The game should have exactly 6 rows for guesses");
        
        var form = await Page.QuerySelectorAsync(".wordle-game form");
        form.Should().NotBeNull("The game should have a form for entering guesses");
        
        var input = await Page.QuerySelectorAsync(".wordle-game form input[type='text']");
        input.Should().NotBeNull("The form should have a text input field");
        
        var maxLength = await input!.GetAttributeAsync("maxlength");
        maxLength.Should().Be("5", "The input should only allow 5 letters");
        
        var pattern = await input.GetAttributeAsync("pattern");
        pattern.Should().Be("[A-Za-z]{5}", "The input should only accept 5 letters");
    }
}