using System.Net;
using System.Threading.Tasks;
using Alba;
using tomware.OpenIddict.UI.Api;
using tomware.OpenIddict.UI.Tests.Helpers;
using Xunit;

namespace tomware.OpenIddict.UI.Tests.Integration
{
  public class RoleApiTest : IClassFixture<WebAppFixture>
  {
    private readonly SystemUnderTest _system;

    public RoleApiTest(WebAppFixture app)
    {
      _system = app.SystemUnderTest;
    }

    [Theory]
    [InlineData("/api/roles", HttpVerb.Get)]
    [InlineData("/api/roles/id", HttpVerb.Get)]
    [InlineData("/api/roles", HttpVerb.Post)]
    // [InlineData("/api/roles", HttpVerb.Put)]
    // [InlineData("/api/roles/01D7ACA3-575C-4E60-859F-DB95B70F8190", HttpVerb.Delete)]
    public async Task IsNotAuthenticated_ReturnsUnauthorized(
      string endpoint,
      HttpVerb verb
    )
    {
      // Arrange

      // Act
      var response = await _system.Scenario(s =>
      {
        switch (verb)
        {
          case HttpVerb.Post:
            s.Post.Json(new RoleViewModel()).ToUrl(endpoint);
            break;
          case HttpVerb.Put:
            s.Put.Json(new RoleViewModel()).ToUrl(endpoint);
            break;
          case HttpVerb.Delete:
            s.Delete.Url(endpoint);
            break;
          default:
            s.Get.Url(endpoint);
            break;
        }

        // Assert
        s.StatusCodeShouldBe(HttpStatusCode.Unauthorized);
      });

      Assert.NotNull(response);
      Assert.Equal(401, response.Context.Response.StatusCode);
    }
  }
}