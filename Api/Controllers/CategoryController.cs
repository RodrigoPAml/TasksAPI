using Api.Base;
using Domain.Base;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;
using Application.CQRS.Categories.CreateCategory;
using Application.CQRS.Categories.UpdateCategory;
using Application.CQRS.Categories.DeleteCategory;
using Application.CQRS.Categories.GetCategory;
using Application.CQRS.Categories.GetAllCategories;
using Domain.Models.Responses;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : BaseController
    {
        public CategoryController(IServiceProvider provider) : base(provider)
        {
        }

        /// <summary>
        /// Create a new category
        /// </summary>
        [Authorize]
        [HttpPost]
        [SwaggerResponse(StatusCodes.Status201Created, Type = typeof(BaseResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(BaseResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(BaseResponse))]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryCommand command)
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
        /// Update an category
        /// </summary>
        [Authorize]
        [HttpPut]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(BaseResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(BaseResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(BaseResponse))]
        public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryCommand command)
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
        /// Delete an category
        /// </summary>
        [Authorize]
        [HttpDelete]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(BaseResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(BaseResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(BaseResponse))]
        public async Task<IActionResult> DeleteCategory([FromBody] DeleteCategoryCommand command)
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
        /// Get a Category
        /// </summary>
        [Authorize]
        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(BaseResponse<Category>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(BaseResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(BaseResponse))]
        public async Task<IActionResult> GetCategory([FromQuery] GetCategoryQuery query)
        {
            return await HandleApplicationResponse<Result<Category>>(
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
        /// Get all categories from current user
        /// </summary>
        [Authorize]
        [HttpGet("GetAll")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(BaseResponse<GetAllResponse<Category>>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(BaseResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(BaseResponse))]
        public async Task<IActionResult> GetAll([FromQuery] GetAllCategoriesQuery query)
        {
            return await HandleApplicationResponse<Result<GetAllResponse<Category>>>(
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