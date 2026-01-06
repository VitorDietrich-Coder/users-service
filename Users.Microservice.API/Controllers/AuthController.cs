using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Users.Microservice.API.Controllers;
using Users.Microservice.Application.Auth.Commands;
using Users.Microservice.Application.Auth.Models.Response;

namespace Users.Microservice.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "v1")]
[Route("api/v{version:apiVersion}/auth")]
public class AuthController : ApiControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    [SwaggerOperation(
        Summary = "User authentication",
        Description = "Authenticates the user and returns a JWT token."
    )]
    public async Task<ActionResult<LoginResponse>> LoginAsync(
        [FromBody] LoginCommand command)
    {
        return await Mediator.Send(command);
    }
}
