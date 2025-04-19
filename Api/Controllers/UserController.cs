using Api.Base;
using Domain.Base;
using Microsoft.AspNetCore.Mvc;
using Application.CQRS.Users.DeleteUser;
using Application.CQRS.Users.UpdateUsername;
using Application.CQRS.Users.UpdateUserPassword;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : BaseController
    {
        public UserController(IServiceProvider provider) : base(provider)
        {
        }

        /// <summary>
        /// Update current username
        /// </summary>
        [Authorize]
        [HttpPatch("UpdateUsername")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(BaseResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(BaseResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(BaseResponse))]
        public async Task<IActionResult> UpdateUsername([FromBody] UpdateUsernameCommand command)
        {
            return await HandleApplicationResponse<Operation>(
                  command,
                  (resp) =>
                  {
                      return new()
                      {
                          Success = resp.Success,
                          Data = null,
                          ErrorMessage = resp.Message,
                          Code = resp.Success ? 200 : 400
                      };
                  }
            );
        }

        /// <summary>
        /// Update current user password
        /// </summary>
        [Authorize]
        [HttpPatch("UpdateUserPassword")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(BaseResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(BaseResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(BaseResponse))]
        public async Task<IActionResult> UpdateUserPassword([FromBody] UpdateUserPasswordCommand command)
        {
            return await HandleApplicationResponse<Operation>(
                  command,
                  (resp) =>
                  {
                      return new()
                      {
                          Success = resp.Success,
                          Data = null,
                          ErrorMessage = resp.Message,
                          Code = resp.Success ? 200 : 400
                      };
                  }
            );
        }

        /// <summary>
        /// Delete current user
        /// </summary>
        [Authorize]
        [HttpDelete]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(BaseResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(BaseResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(BaseResponse))]
        public async Task<IActionResult> DeleteUser([FromBody] DeleteUserCommand command)
        {
            return await HandleApplicationResponse<Operation>(
                  command,
                  (resp) =>
                  {
                      return new()
                      {
                          Success = resp.Success,
                          Data = null,
                          ErrorMessage = resp.Message,
                          Code = resp.Success ? 200 : 400
                      };
                  }
            );
        }
    }
}