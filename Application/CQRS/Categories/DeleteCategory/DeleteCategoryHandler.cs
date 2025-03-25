using MediatR;
using Domain.Base;
using AutoMapper;
using Domain.Models.Categories;
using Domain.Interfaces.Business;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Authentication;
using Application.Interfaces.Transaction;
using Microsoft.Extensions.DependencyInjection;

namespace Application.CQRS.Categories.DeleteCategory
{
    /// <summary>
    /// Handler for Category update
    /// </summary>
    public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, Operation>
    {
        private readonly ICategoryBusiness _categoryBusiness;
        private readonly ICategoryRepository _categoryRepo;

        private readonly ITokenService _tokenService;
        private readonly IUnityOfWork _uow;
        private readonly IMapper _mapper;

        public DeleteCategoryHandler(IServiceProvider provider)
        {
            _categoryBusiness = provider.GetService<ICategoryBusiness>();
            _categoryRepo = provider.GetService<ICategoryRepository>();

            _tokenService = provider.GetService<ITokenService>();
            _uow = provider.GetService<IUnityOfWork>();
            _mapper = provider.GetService<IMapper>();
        }

        public async Task<Operation> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                return Operation.MakeFailure("Invalid request");

            var userId = _tokenService.GetToken().Id;

            var deleteModel = new DeleteCategoryModel()
            {
                Category = await _categoryRepo.GetById(request.Id)
            };

            if(deleteModel.Category != null && deleteModel.Category.UserId != userId)
                return Operation.MakeFailure("Not Allowed");

            var updateResult = await _categoryBusiness.CanDelete(deleteModel);

            if(!updateResult.Success)
                return updateResult;  

            await _uow.Begin();

            await _categoryRepo.Delete(deleteModel.Category.Id);

            await _uow.Save();
            await _uow.Commit();

            return Operation.MakeSuccess();
        }
    }
}