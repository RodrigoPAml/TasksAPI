using Domain.Base;
using Domain.Entities;
using Domain.Models.Categories;

namespace Domain.Interfaces.Business
{
    /// <summary>
    /// Business Service interface for Category entity
    /// </summary>
    public interface ICategoryBusiness
    {
        public Task<Result<Category>> Create(CreateCategoryModel model);
        public Task<Operation> Update(UpdateCategoryModel model);
        public Task<Operation> CanDelete(DeleteCategoryModel model);
    }
}
