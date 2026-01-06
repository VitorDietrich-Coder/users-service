using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using Users.Microservice.API.Attributes;
using Users.Microservice.Application.Commands;
using Users.Microservice.Application.Common;
using Users.Microservice.Application.Queries;
using Users.Microservice.Application.Users.Models.Response;

namespace Users.Microservice.API.Controllers
{
    /// <summary>
    /// Manages operations related to system users.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/v{version:apiVersion}/users")]
    public class UsersController : ApiControllerBase
    {
        /// <summary>
        /// Creates a new regular user.
        /// </summary>
        [AllowAnonymous]
        [HttpPost("create-user")]
        [SwaggerOperation(
            Summary = "Creates a new user.",
            Description = "Registers a new user in the system."
        )]
        [SwaggerResponseProfile("User.Register")]
        [ProducesResponseType(typeof(SuccessResponse<Guid>), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<Guid>> CreateAsync([FromBody] CreateUserCommand command)
        {
            return await Mediator.Send(command);
        }

        /// <summary>
        /// Deactivates a user (logical delete).
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPatch("{id:guid}/delete")]
        [SwaggerOperation(
            Summary = "Deactivates a user.",
            Description = "Marks the user as inactive. This is a logical delete."
        )]
        [SwaggerResponseProfile("User.Delete")]

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeactivateAsync(Guid id)
        {
            await Mediator.Send(new DeactivateUserCommand(id));
            return NoContent();
        }

        /// <summary>
        /// Changes user password.
        /// </summary>
        [Authorize(Roles = "User,Admin")]
        [HttpPatch("{id:guid}/update")]
        [SwaggerOperation(
            Summary = "Changes user password.",
            Description = "Allows changing the user password."
        )]
        [SwaggerResponseProfile("User.Update")]

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> ChangePasswordAsync(
            Guid id,
            [FromBody] UpdateUserCommand command)
        {
            await Mediator.Send(command with { UserId = id });
            return NoContent();
        }

        /// <summary>
        /// Retrieves a user by ID.
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("{id:guid}")]
        [SwaggerOperation(
            Summary = "Retrieves a user by ID.",
            Description = "Returns user details."
        )]
        [SwaggerResponseProfile("User.Get")]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<UserResponse>> GetByIdAsync(Guid id)
        {
            return await Mediator.Send(new GetUserByIdQuery(id));
        }

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [SwaggerOperation(
            Summary = "Retrieves all users.",
            Description = "Returns all users."
        )]
        [SwaggerResponseProfile("User.GetAll")]

        [ProducesResponseType(typeof(List<UserResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<UserResponse>>> GetAllAsync()
        {
            return await Mediator.Send(new GetAllUsersQuery());
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("create-admin")]
        [SwaggerResponseProfile("Admin.Register")]

        public async Task<ActionResult<UserResponse>> CreateAdminAsync([FromBody] CreateAdminCommand command)
        {
            var id = await Mediator.Send(command);
            return CreatedAtAction(nameof(GetByIdAsync), new { id }, new { id });
        }
    }
}
