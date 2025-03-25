using MediatR;
using Domain.Base;
using Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Application.CQRS.Categories.GetCategory
{
    /// <summary>
    /// Query to get a Category
    /// </summary>
    public class GetCategoryQuery : IRequest<Result<Category>>
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int Id { get; set; }
    }
}
