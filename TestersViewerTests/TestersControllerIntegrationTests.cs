using Fizzler.Systems.HtmlAgilityPack;
using FluentAssertions;
using HtmlAgilityPack;

namespace TestersViewerTests;

public class TestersControllerIntegrationTests : IClassFixture<CustomWebAppFactory>
{
    private readonly HttpClient _client;

    public TestersControllerIntegrationTests(CustomWebAppFactory factory)
    {
       _client = factory.CreateClient();
    }
    
    #region Index

    [Fact]
    public async Task Index_ShallReturnView()
    {
        HttpResponseMessage response = await _client.GetAsync("/Testers/Index");
        response.Should().BeSuccessful();

        var responseContent = response.Content.ReadAsStreamAsync();

        var htmlDocument = new HtmlDocument();
        htmlDocument.Load(responseContent.Result);
        var document = htmlDocument.DocumentNode;

        document.QuerySelectorAll("table.table").Should().NotBeNull();
    }

    #endregion
}