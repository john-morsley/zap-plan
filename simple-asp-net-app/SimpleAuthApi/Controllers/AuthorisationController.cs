namespace SimpleAuthApi.Controllers;

[ApiController]
[Route("authorisation")]
[AllowAnonymous]
public class AuthorisationController : ControllerBase
{
    private readonly byte[] _key;

    public AuthorisationController(byte[] key)
    {
        _key = key;
    }

    [HttpPost]
    public IActionResult Post([FromQuery] string id, [FromQuery] string secret)
    {
        if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(secret))
        {
            return BadRequest();
        }

        if (id != "ABC" || secret != "123")
        {
            return Forbid();
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
             Subject = new ClaimsIdentity(new[]
             {
                 new Claim(ClaimTypes.Name, id),
                 new Claim("id", id)
             }),
             Expires = DateTime.UtcNow.AddHours(1),
             SigningCredentials = new SigningCredentials(
                 new SymmetricSecurityKey(_key),
                 SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return Ok(tokenString);
    }
}