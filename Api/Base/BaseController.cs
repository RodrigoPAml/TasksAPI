using MediatR;
using System.Text.Json;
using Application.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using Application.Interfaces.Logging;
using Domain.Interfaces.Authentication;
using Application.Interfaces.Transaction;

namespace Api.Base
{
    /// <summary>
    /// Base controller
    /// Provide a generic way to decide the api response
    /// Provide automatic rollback
    /// Provide automatic logging
    /// </summary>
    public class BaseController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IServiceProvider _provider;
        private readonly ILoggingService _logger;
        private readonly ITokenService _tokenService;

        public BaseController(IServiceProvider provider)
        {
            _mediator = provider.GetService<IMediator>();
            _provider = provider.GetService<IServiceProvider>();
            _logger = provider.GetService<ILoggingService>();
            _tokenService = provider.GetService<ITokenService>();
        }

        private void SetToken()
        {
            var authorizationHeader = Request?.Headers?.ContainsKey("Authorization") ?? false
                ? Request?.Headers["Authorization"].ToString()
                : null; 

            if(!string.IsNullOrEmpty(authorizationHeader))
                _tokenService.SetCurrentToken(authorizationHeader);
        }

        private async Task Log(object request, BaseResponse response, Exception ex)
        {
            var url = $"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}";
            var headers = string.Join(", ", Request.Headers.Select(h => $"{h.Key}: {h.Value}"));
            var body = string.Empty;

            if (!Request.Body.CanSeek)
                Request.EnableBuffering();

            Request.Body.Position = 0;

            using (var reader = new StreamReader(Request.Body, leaveOpen: true))
            {
                body = await reader.ReadToEndAsync();
                Request.Body.Position = 0;
            }

            var information = new 
            {
                url = url,
                headers = headers,
                exception = ex?.Message + ex?.StackTrace + ex?.InnerException?.Message + ex?.InnerException?.StackTrace,
                token = _tokenService.GetToken(),
                request = request,
                body = body,
                response = response,
            };

            var jsonConfig = new JsonSerializerOptions();
            jsonConfig.WriteIndented = true;
            jsonConfig.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;

            _logger.Log(
                request.GetType().Name, 
                JsonSerializer.Serialize(information, options: jsonConfig), 
                response.Success
                    ? LogTypeEnum.Information
                    : LogTypeEnum.Error
            );
        }

        private async Task<ObjectResult> GetApiResponse(BaseResponse response, object request = null, Exception ex = null)
        {
            await Log(request, response, ex);

            return StatusCode(response.Code, response);
        }

        protected async Task<ObjectResult> HandleApplicationResponse<R>(object request, Func<R, BaseResponse> apiAction)
        {
            SetToken();

            try
            {
                if(request == null)
                {
                    return await GetApiResponse
                    (
                        new()
                        {
                            Response = null,
                            Success = false,
                            ErrorMessage = "Invalid request",
                            Code = 400
                        },
                        request
                    );
                }

                if(!ModelState.IsValid)
                {
                    string error = ModelState.Values
                        .FirstOrDefault()
                        ?.Errors?
                        .FirstOrDefault()?
                        .ErrorMessage ?? "Unknow error";

                    if(error.EndsWith('.'))
                        error = error[..^1];

                    return await GetApiResponse
                    (
                        new()
                        {
                            Response = null,
                            Success = false,
                            ErrorMessage = error,
                            Code = 400
                        },
                        request
                    );
                }

                var appResponse = await _mediator.Send(request);
                var apiResponse = apiAction((R)appResponse);

                return await GetApiResponse(apiResponse, request);
            }
            catch (Exception ex)
            {
                var uow = _provider.GetService<IUnityOfWork>();

                uow?.Rollback();

                return await GetApiResponse
                (
                    new()
                    {
                        Response = null,
                        Success = false,
                        ErrorMessage = ex.Message,
                        Code = 500
                    },
                    request,
                    ex
                );
            }
            finally
            {
                await _logger.Persist();
            }
        }
    }
}