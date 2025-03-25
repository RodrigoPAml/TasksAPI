using Domain.Base;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.CQRS.Categories.UpdateCategory
{
    /// <summary>
    /// Command to update a Category
    /// </summary>
    public class UpdateCategoryCommand : IRequest<Operation>
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
