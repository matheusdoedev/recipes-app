using Microsoft.AspNetCore.Mvc;

namespace RecipesApp.Controllers;

[ApiController]
[Route("")]
public class HealthCheckController
{
  [HttpGet]
  public string Get()
  {
    return DateTime.UtcNow.ToLongDateString();
  }
}
