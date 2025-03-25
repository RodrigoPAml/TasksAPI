using MediatR;
using Domain.Base;
using Domain.Entities;
using Domain.Models.Responses;

namespace Application.CQRS.Categories.GetAllCategories
{
    /// <summary>
    /// Query to get all Categories
    /// </summary>
    public class GetAllCategoriesQuery : IRequest<Result<GetAllResponse<Category>>>
    {
    }
}
