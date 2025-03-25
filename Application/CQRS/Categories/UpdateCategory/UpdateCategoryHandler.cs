using MediatR;
using Domain.Base;
using AutoMapper;
using Domain.Models.Categories;
using Domain.Interfaces.Business;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Authentication;
using Application.Interfaces.Transaction;
using Microsoft.Extensions.DependencyInjection;

namespace Application.CQRS.Categories.UpdateCategory
{
    /// <summary>
    /// Handler for Category update
    /// </summary>
    public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, Operation>
    {
        private readonly ICategoryBusiness _categoryBusiness;
        private readonly ICategoryRepository _categoryRepo;

        private readonly ITokenService _tokenService;
        private readonly IUnityOfWork _uow;
        private readonly IMapper _mapper;

        public UpdateCategoryHandler(IServiceProvider provider)
        {
            _categoryBusiness = provider.GetService<ICategoryBusiness>();
            _categoryRepo = provider.GetService<ICategoryRepository>();

            _tokenService = provider.GetService<ITokenService>();
            _uow = provider.GetService<IUnityOfWork>();
            _mapper = provider.GetService<IMapper>();
        }

        public async Task<Operation> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                return Operation.MakeFailure("Invalid request");

            var userId = _tokenService.GetToken().Id;

            var updateModel = new UpdateCategoryModel();
            updateModel.Category = await _categoryRepo.GetById(request.Id);
            updateModel.Info = _mapper.Map<UpdateCategoryInfoModel>(request);

            if (updateModel.Category != null && updateModel.Category.UserId != userId)
                return Operation.MakeFailure("Not Allowed");

            var updateResult = await _categoryBusiness.Update(updateModel);

            if(!updateResult.Success)
                return updateResult;  

            await _uow.Begin();

            await _categoryRepo.Update(updateModel.Category);

            await _uow.Save();
            await _uow.Commit();

            return Operation.MakeSuccess();
        }
    }
}