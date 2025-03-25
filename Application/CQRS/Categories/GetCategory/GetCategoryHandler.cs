using MediatR;
using Domain.Base;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Application.CQRS.Categories.GetCategory
{
    /// <summary>
    /// Handler to get a Category
    /// </summary>
    public class GetCategoryHandler : IRequestHandler<GetCategoryQuery, Result<Category>>
    {
        private readonly ICategoryRepository _categoryRepo;
        private readonly ITokenService _tokenService;

        public GetCategoryHandler(IServiceProvider provider)
        {
            _categoryRepo = provider.GetService<ICategoryRepository>();
            _tokenService = provider.GetService<ITokenService>();
        }

        public async Task<Result<Category>> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
                return Result.MakeFailure<Category>("Invalid request");

            var category = await _categoryRepo.GetById(request.Id);

            if (category == null)
                return Result.MakeFailure<Category>("Category not found");

            var userId = _tokenService.GetToken().Id;

            if (category.UserId != userId)
                return Result.MakeFailure<Category>("Not allowed");

            return Result.MakeSuccess(category);
        }
    }
}