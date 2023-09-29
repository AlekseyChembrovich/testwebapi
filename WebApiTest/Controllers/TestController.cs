using Microsoft.AspNetCore.Mvc;

namespace WebApiTest.Controllers;

[ApiController]
[Route("tests")]
public class TestController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        var result = Enumerable.Range(1, 5) // Comment #1
            .Select(x => new
            {
                Date = DateTime.Now.AddDays(x),
                Temperature = Random.Shared.Next(-20, 20)
            })
            .ToList();

        return Ok(result);
    }
}
