using MediatR;
using Domain.Base;
using AutoMapper;
using Domain.Models.Categories;
using Domain.Interfaces.Business;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Authentication;
using Application.Interfaces.Transaction;
using Microsoft.Extensions.DependencyInjection;

namespace Application.CQRS.Categories.CreateCategory
{
    /// <summary>
    /// Handler for Category creation
    /// </summary>
    public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, Operation>
    {
        private readonly ICategoryBusiness _categoryBusiness;
        private readonly ICategoryRepository _categoryRepo;

        private readonly ITokenService _tokenService;
        private readonly IUnityOfWork _uow;
        private readonly IMapper _mapper;

        public CreateCategoryHandler(IServiceProvider provider)
        {
            _categoryBusiness = provider.GetService<ICategoryBusiness>();
            _categoryRepo = provider.GetService<ICategoryRepository>();

            _tokenService = provider.GetService<ITokenService>();
            _uow = provider.GetService<IUnityOfWork>();
            _mapper = provider.GetService<IMapper>();
        }

        public async Task<Operation> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                return Operation.MakeFailure("Invalid request");

            var createModel = _mapper.Map<CreateCategoryModel>(request);
            createModel.UserId = _tokenService.GetToken().Id;

            var newCategoryResult = await _categoryBusiness.Create(createModel);

            if(!newCategoryResult.Success)
                return Operation.MakeFailure(newCategoryResult.Message);  

            await _uow.Begin();

            await _categoryRepo.Create(newCategoryResult.Content);

            await _uow.Save();
            await _uow.Commit();

            return Operation.MakeSuccess();
        }
    }
}