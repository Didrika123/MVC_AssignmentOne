using System;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using System.Net.Http;
using AngleSharp.Html.Dom;
using System.Net;
/*
* SUT = SYSTEM UNDER TEST
Integration tests in ASP.NET Core require the following:

A test project is used to contain and execute the tests. The test project has a reference to the SUT.
The test project creates a test web host for the SUT and uses a test server client to handle requests and responses with the SUT.
A test runner is used to execute the tests and report the test results.

Integration tests follow a sequence of events that include the usual Arrange, Act, and Assert test steps:

The SUT's web host is configured.
A test server client is created to submit requests to the app.
The Arrange test step is executed: The test app prepares a request.
The Act test step is executed: The client submits the request and receives the response.
The Assert test step is executed: The actual response is validated as a pass or fail based on an expected response.
The process continues until all of the tests are executed.
The test results are reported.
*/
namespace AssignmentOne.Tests
{
    // https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-5.0
    // Using help methods in  https://github.com/dotnet/AspNetCore.Docs/tree/master/aspnetcore/test/integration-tests/samples/3.x/IntegrationTestsSample/tests/RazorPagesProject.Tests/Helpers
    //install nuget package .mvc.testing

    public class HelloTestWorld
    : IClassFixture<WebApplicationFactory<AssignmentOne.Startup>>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<AssignmentOne.Startup> _factory;

        public HelloTestWorld(WebApplicationFactory<AssignmentOne.Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/Home/Index")]
        [InlineData("/Home/About")]
        [InlineData("/Home/Contact")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [InlineData("32", "cold")]
        [InlineData("37", "ok")]
        [InlineData("38.5", "hot")]
        public async Task Post_GivenTemperatureValue_ReturnsCorrectFeverResult(string inputTemperature, string feverResultStr)
        {
            // Arrange
            var defaultPage = await _client.GetAsync("/fevercheck");
            var content = await MyHtmlHelper.GetDocumentAsync(defaultPage); //HtmlHelpers to parse pages so u can manage it using simple c# code instead of managing plain text
                                                                            // Its done using is a 3rd party library (install nuget AngleSharp for example)

            //Act   //Note this is using an extension SendAsync, defined in HttpClientExtensions.cs
            var temp = (IHtmlInputElement)content.QuerySelector("input[id='temperature']");
            temp.Value = inputTemperature;

            var response = await _client.SendAsync(       
                (IHtmlFormElement)content.QuerySelector("form[id='formTemperature']"),
                (IHtmlButtonElement)content.QuerySelector("button[id='btnSubmitTemperature']"));

            string plainTextResponse = await response.Content.ReadAsStringAsync();
            var responseContent = MyHtmlHelper.GetDocumentAsync(response);
            IHtmlHeadingElement feverResultH2 = (IHtmlHeadingElement)responseContent.Result.QuerySelector("h2");

            // Assert
            Assert.Equal(HttpStatusCode.OK, defaultPage.StatusCode);
            Assert.Contains(feverResultStr, feverResultH2.InnerHtml.ToLower());
           // Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            //Assert.Equal("/fevercheck", response.Headers.Location.OriginalString);
        }
    }
}
