using MediatR;
using Domain.Base;
using System.ComponentModel.DataAnnotations;

namespace Application.CQRS.Categories.DeleteCategory
{
    /// <summary>
    /// Command to delete a Category
    /// </summary>
    public class DeleteCategoryCommand : IRequest<Operation>
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int Id { get; set; }
    }
}
