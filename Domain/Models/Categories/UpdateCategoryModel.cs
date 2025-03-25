using Domain.Entities;

namespace Domain.Models.Categories
{
    public class UpdateCategoryModel
    {
        public Category Category { get; set; }
        public UpdateCategoryInfoModel Info { get; set; }
    }
}
