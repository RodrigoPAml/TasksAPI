using Api.Base;
using Domain.Base;
using Microsoft.AspNetCore.Mvc;
using Application.CQRS.Tasks.CreateTask;
using Application.CQRS.Tasks.UpdateTask;
using Application.CQRS.Tasks.DeleteTask;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;
using Application.CQRS.Tasks.GetTask;
using TaskEntity = Domain.Entities.Task;
using Application.CQRS.Tasks.GetPagedTasks;
using Domain.Models.Responses;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController : BaseController
    {
        public TaskController(IServiceProvider provider) : base(provider)
        {
        }

        /// <summary>
        /// Create a new Task
        /// </summary>
        [Authorize]
        [HttpPost]
        [SwaggerResponse(StatusCodes.Status201Created, Type = typeof(BaseResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(BaseResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(BaseResponse))]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskCommand command)
        {
            return await HandleApplicationResponse<Operation>(
                  command,
                  (resp) =>
                  {
                      return new()
                      {
                          Success = resp.Success,
                          Response = null,
                          ErrorMessage = resp.Message,
                          Code = resp.Success ? 201 : 400
                      };
                  }
            );
        }

        /// <summary>
        /// Update a Task
        /// </summary>
        [Authorize]
        [HttpPut]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(BaseResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(BaseResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(BaseResponse))]
        public async Task<IActionResult> UpdateTask([FromBody] UpdateTaskCommand command)
        {
            return await HandleApplicationResponse<Operation>(
                  command,
                  (resp) =>
                  {
                      return new()
                      {
                          Success = resp.Success,
                          Response = null,
                          ErrorMessage = resp.Message,
                          Code = resp.Success ? 200 : 400
                      };
                  }
            );
        }

        /// <summary>
        /// Delete a Task
        /// </summary>
        [Authorize]
        [HttpDelete]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(BaseResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(BaseResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(BaseResponse))]
        public async Task<IActionResult> DeleteTask([FromBody] DeleteTaskCommand command)
        {
            return await HandleApplicationResponse<Operation>(
                  command,
                  (resp) =>
                  {
                      return new()
                      {
                          Success = resp.Success,
                          Response = null,
                          ErrorMessage = resp.Message,
                          Code = resp.Success ? 200 : 400
                      };
                  }
            );
        }

        /// <summary>
        /// Get a Task
        /// </summary>
        [Authorize]
        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(BaseResponse<TaskEntity>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(BaseResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(BaseResponse))]
        public async Task<IActionResult> GetTask([FromQuery] GetTaskQuery query)
        {
            return await HandleApplicationResponse<Result<TaskEntity>>(
                  query,
                  (resp) =>
                  {
                      return new()
                      {
                          Success = resp.Success,
                          Response = resp.Content,
                          ErrorMessage = resp.Message,
                          Code = resp.Success ? 200 : 400
                      };
                  }
            );
        }

        /// <summary>
        /// Get paged tasks
        /// </summary>
        [Authorize]
        [HttpGet("GetPaged")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(BaseResponse<GetPagedResponse<TaskEntity>>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(BaseResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(BaseResponse))]
        public async Task<IActionResult> GetPaged([FromQuery] GetPagedTasksQuery query)
        {
            return await HandleApplicationResponse<Result<GetPagedResponse<TaskEntity>>>(
                  query,
                  (resp) =>
                  {
                      return new()
                      {
                          Success = resp.Success,
                          Response = resp.Content,
                          ErrorMessage = resp.Message,
                          Code = resp.Success ? 200 : 400
                      };
                  }
            );
        }
    }
}