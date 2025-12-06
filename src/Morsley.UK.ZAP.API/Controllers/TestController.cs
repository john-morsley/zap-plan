namespace SimpleAuthApi.Controllers;

[ApiController]
[Route("test")]
[Authorize]
public class TestController : ControllerBase
{
    [HttpGet]
    
    public IActionResult Get()
    {
        if (User.Identity?.IsAuthenticated == false) return Unauthorized("Token required");

        return Ok("Hello");
    }
}