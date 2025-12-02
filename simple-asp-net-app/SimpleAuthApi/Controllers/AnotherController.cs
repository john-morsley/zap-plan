namespace SimpleAuthApi.Controllers;

[ApiController]
[Route("another")]
[Authorize]
public class AnotherController : ControllerBase
{
    [HttpGet]
    
    public IActionResult Get()
    {
        if (User.Identity?.IsAuthenticated == false) return Unauthorized();

        return Ok("Goodbye");
    }
}