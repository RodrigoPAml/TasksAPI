using IoC.Utils;
using Infra.Entities;
using Domain.Entities;
using Domain.Models.Users;
using Domain.Models.Tasks;
using Domain.Models.Categories;
using Application.CQRS.Users.UpdateUserPassword;
using Application.CQRS.Users.UpdateUsername;
using Application.CQRS.Categories.CreateCategory;
using Application.CQRS.Categories.UpdateCategory;
using Microsoft.Extensions.DependencyInjection;
using Application.CQRS.Authentication.SignUp;
using Application.CQRS.Tasks.CreateTask;
using Application.CQRS.Tasks.UpdateTask;
using Task = Domain.Entities.Task;

namespace IoC.Register
{
    /// <summary>
    /// Register AutoMapper configurations
    /// </summary>
    public static class AutoMapperRegister
    {
        public static void Invoke(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile).Assembly); 
        }
    }

    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            MapApplication();
            MapEntities();
        }

        private void MapApplication()
        {
            CreateMap<SignUpCommand, CreateUserModel>().ReverseMap();
            CreateMap<UpdateUsernameCommand, UpdateUsernameModel>().ReverseMap();
            CreateMap<UpdateUserPasswordCommand, UpdateUserPasswordModel>().ReverseMap();

            CreateMap<CreateCategoryCommand, CreateCategoryModel>().ReverseMap();
            CreateMap<UpdateCategoryCommand, UpdateCategoryInfoModel>().ReverseMap();

            CreateMap<CreateTaskCommand, CreateTaskModel>().ReverseMap();
            CreateMap<UpdateTaskCommand, UpdateTaskInfoModel>().ReverseMap();
        }

        private void MapEntities()
        {
            CreateMap<DbUser, User>()
                .BypassConstructor()
                .ForMember(x => x.HashedPassword, opt => opt.MapFrom(y => y.Password))
                .ForMember(x => x.Password, opt => opt.Ignore());

            CreateMap<User, DbUser>()
                .ForMember(x => x.Password, opt => opt.MapFrom(y => y.HashedPassword))
                .LinkReference();

            CreateMap<DbCategory, Category>()
               .BypassConstructor();

            CreateMap<Category, DbCategory>()
                .LinkReference();

            CreateMap<DbTask, Task>()
                .BypassConstructor();

            CreateMap<Task, DbTask>()
                .LinkReference();

            CreateMap<DbVerificationCode, VerificationCode>()
                .BypassConstructor();

            CreateMap<VerificationCode, DbVerificationCode>()
                .LinkReference();
        }
    }
}
