using MediatR;
using Domain.Base;
using Domain.Entities;
using Domain.Models.Responses;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Application.CQRS.Categories.GetAllCategories
{
    /// <summary>
    /// Handler to get all Categories
    /// </summary>
    public class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesQuery, Result<GetAllResponse<Category>>>
    {
        private readonly ICategoryRepository _categoryRepo;
        private readonly ITokenService _tokenService;

        public GetAllCategoriesHandler(IServiceProvider provider)
        {
            _categoryRepo = provider.GetService<ICategoryRepository>();
            _tokenService = provider.GetService<ITokenService>();
        }

        public async Task<Result<GetAllResponse<Category>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
                return Result.MakeFailure<GetAllResponse<Category>>("Invalid request");

            var userId = _tokenService.GetToken().Id;
            var categories = await _categoryRepo.GetByUser(userId);

            return Result.MakeSuccess(new GetAllResponse<Category>()
            {
                Count = categories.Count,
                Items = categories
            });
        }
    }
}