using MediatR;
using Domain.Base;
using System.ComponentModel.DataAnnotations;

namespace Application.CQRS.Categories.CreateCategory
{
    /// <summary>
    /// Command to create a new Category
    /// </summary>
    public class CreateCategoryCommand : IRequest<Operation>
    {
        [Required]
        public string Name { get; set; }
    }
}
