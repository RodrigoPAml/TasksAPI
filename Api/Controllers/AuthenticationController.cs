using Api.Base;
using Domain.Base;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Application.CQRS.Authentication.Login;
using Application.CQRS.Authentication.SignUp;
using Application.CQRS.Authentication.ResetPassword;
using Application.CQRS.Authentication.VerifyEmailUsed;
using Application.CQRS.Authentication.SendEmailVerification;
using Application.CQRS.Authentication.SendResetPasswordVerification;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : BaseController
    {
        public AuthenticationController(IServiceProvider provider) : base(provider)
        {
        }

        /// <summary>
        /// Login into the system
        /// </summary>
        [HttpPost("Login")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(BaseResponse<string>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(BaseResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(BaseResponse))]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            return await HandleApplicationResponse<Result<string>>(
                 command,
                 (resp) =>
                 {
                     var response = new BaseResponse
                     {
                         Success = resp.Success,
                         Response = resp.Content,
                         ErrorMessage = resp.Message,
                         Code = resp.Success ? 200 : 400
                     };

                     return response;
                 }
            );
        }

        /// <summary>
        /// Send an email verification code for sign up
        /// </summary>
        [HttpPost("SendEmailVerification")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(BaseResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(BaseResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(BaseResponse))]
        public async Task<IActionResult> SendEmailVerification([FromBody] SendEmailVerificationCommand command)
        {
            return await HandleApplicationResponse<Operation>(
                 command,
                 (resp) =>
                 {
                     var response = new BaseResponse
                     {
                         Success = resp.Success,
                         Response = null,
                         ErrorMessage = resp.Message,
                         Code = resp.Success ? 200 : 400
                     };

                     return response;
                 }
            );
        }

        /// <summary>
        /// Send an email verification code for the password reset
        /// </summary>
        [HttpPost("SendResetPasswordVerification")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(BaseResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(BaseResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(BaseResponse))]
        public async Task<IActionResult> SendResetPasswordVerification([FromBody] SendResetPasswordVerificationCommand command)
        {
            return await HandleApplicationResponse<Operation>(
                 command,
                 (resp) =>
                 {
                     var response = new BaseResponse
                     {
                         Success = resp.Success,
                         Response = null,
                         ErrorMessage = resp.Message,
                         Code = resp.Success ? 200 : 400
                     };

                     return response;
                 }
            );
        }

        /// <summary>
        /// Create a new account
        /// </summary>
        [HttpPost("SignUp")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(BaseResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(BaseResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(BaseResponse))]
        public async Task<IActionResult> SignUp([FromBody] SignUpCommand command)
        {
            return await HandleApplicationResponse<Operation>(
                 command,
                 (resp) =>
                 {
                     var response = new BaseResponse
                     {
                         Success = resp.Success,
                         Response = null,
                         ErrorMessage = resp.Message,
                         Code = resp.Success ? 200 : 400
                     };

                     return response;
                 }
            );
        }

        /// <summary>
        /// Resets the account password
        /// </summary>
        [HttpPost("ResetPassword")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(BaseResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(BaseResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(BaseResponse))]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
        {
            return await HandleApplicationResponse<Operation>(
                 command,
                 (resp) =>
                 {
                     var response = new BaseResponse
                     {
                         Success = resp.Success,
                         Response = null,
                         ErrorMessage = resp.Message,
                         Code = resp.Success ? 200 : 400
                     };

                     return response;
                 }
            );
        }

        /// <summary>
        /// Verify if an email is in use
        /// </summary>
        [HttpGet("VerifyEmailInUse")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(BaseResponse<bool>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(BaseResponse<bool>))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(BaseResponse))]
        public async Task<IActionResult> VerifyEmailInUse([FromQuery] VerifyEmailUsedQuery query)
        {
            return await HandleApplicationResponse<Result<bool>>(
                 query,
                 (resp) =>
                 {
                     var response = new BaseResponse
                     {
                         Success = resp.Success,
                         Response = resp.Content,
                         ErrorMessage = resp.Message,
                         Code = resp.Success ? 200 : 400
                     };

                     return response;
                 }
            );
        }
    }
}