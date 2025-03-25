using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.Base
{
    /// <summary>
    /// Base business service implementation
    /// </summary>
    public class BaseBusiness
    {
        protected readonly IServiceProvider _provider;
        protected readonly IMapper _mapper;

        public BaseBusiness(IServiceProvider provider)
        {
            _provider = provider;
            _mapper = _provider.GetService<IMapper>();
        }
    }
}
