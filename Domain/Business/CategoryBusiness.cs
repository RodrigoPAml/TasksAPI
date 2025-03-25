using Domain.Base;
using Domain.Entities;
using Domain.Models.Categories;
using Domain.Interfaces.Business;
using Domain.Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.Business
{
    /// <summary>
    /// Business service implementation for Category entity
    /// </summary>
    public class CategoryBusiness : BaseBusiness, ICategoryBusiness
    {
        public CategoryBusiness(IServiceProvider provider) : base(provider)
        {
        }

        public async Task<Result<Category>> Create(CreateCategoryModel model)
        {
            if (model == null)
                return Result.MakeFailure<Category>("Model is null");

            var categoryResult = Category.Create(model.Name, model.UserId);

            if (!categoryResult.Success)
                return categoryResult;

            var repoUser = _provider.GetService<IUserRepository>();

            if(!await repoUser.ExistsById(model.UserId))
                return Result.MakeFailure<Category>("User not found for category");

            var repoCategory = _provider.GetService<ICategoryRepository>();

            if (await repoCategory.ExistsByName(model.Name, model.UserId))
                return Result.MakeFailure<Category>("Category with this name already exists");

            return Result.MakeSuccess(categoryResult.Content);
        }

        public async Task<Operation> Update(UpdateCategoryModel model)
        {
            if (model == null || model.Info == null)
                return Operation.MakeFailure("Model is null");

            if (model.Category == null)
                return Operation.MakeFailure("Category is null");

            var validation = model.Category.ChangeName(model.Info.Name);

            if (!validation.Success)
                return validation;

            var repoCategory = _provider.GetService<ICategoryRepository>();

            if (await repoCategory.ExistsByName(model.Info.Name, model.Category.UserId, model.Category.Id))
                return Operation.MakeFailure("Category with this name already exists");

            return Operation.MakeSuccess();
        }

        public async Task<Operation> CanDelete(DeleteCategoryModel model)
        {
            if (model == null)
                return Operation.MakeFailure("Model is null");

            if (model.Category == null)
                return Operation.MakeFailure("Category is null");

            var repo = _provider.GetService<ICategoryRepository>();

            if (await repo.IsUsedInTasks(model.Category.Id))
                return Operation.MakeFailure("Category is in use");

            return Operation.MakeSuccess();
        }
    }
}
