# TasksAPI

Base template project for development with ASP.NET server using DDD, CQRS and Clean Architecture principles.

The project contains some base entities to simulate tasks, categories and users.

You may need configure *appsettings.json* to get it to work.

This is a sample, you may change it and adapt to your needs.

## Features

* Entity framework ORM (tested with SQL Server)
* DDD based implementation
* Unit and Integration Tests
* CQRS pattern for use cases
* Cross-cutting concerns: Auto logging with independent transaction, error handler, auto rollback
* Migrations system
* Result pattern return, avoiding throws 
* Api fixed base response
* Unity of Work and repository pattern
* API Base result format
* JWT Authentication
* BCrypt for security of passwords

# Layers

The project has a layered structure as follows.

![image](https://github.com/user-attachments/assets/64e72fce-3414-4816-8668-91877f938ba4)

## API (Presentation)

This layer is responsible for the presentation of the application, defining endpoints (controllers) and responses.

This layer typically receives data from the front-end and calls a mediator through the CQRS pattern, which then invokes the application layer.

It should also handle how the response data should be presented.

```C#
/// <summary>
/// Create a new category
/// </summary>
[Authorize]
[HttpPost]
[SwaggerResponse(StatusCodes.Status201Created, Type = typeof(BaseResponse))]
[SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(BaseResponse))]
[SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(BaseResponse))]
public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryCommand command)
{
    return await HandleApplicationResponse<Operation>(
          command,
          (resp) =>
          {
              return new()
              {
                  Success = resp.Success,
                  Response = null,
                  ErrorMessage = resp.Message,
                  Code = resp.Success ? 201 : 400
              };
          }
    );
}
```

## Application (Use cases)

This layer coordinates the use cases, interacting with the domain layer, infrastructure layer, and presentation layer. 

In our project, the CQRS pattern is used. In the code, you can see that the application layer is responsible for invoking the object creation, validating it, persisting it, and returning the result.

```C#
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
```

## Domain (Core)

This layer contains the business logic and is independent of all other layers. Here, we have the domain entities modeled with the business logic.

```C#
/// <summary>
/// Category entity
/// </summary>
public sealed class Category : BaseEntity
{
    public string Name { get; private set; }
    public int UserId { get; private set; }

    private Category() { }

    private Category(string name, int userId)
    {
        Name = name;
        UserId = userId;
    }

    public static Result<Category> Create(string name, int userId)
    {
        var result = ValidateAll(name, userId);

        if (!result.Success)
            return Result.MakeFailure<Category>(result.Message);

        var category = new Category(name, userId);

        return Result.MakeSuccess(category);
    }
...
```

## Infraestructure

This layer is responsable for data persistence and other services, usually will contain the code for the ORM and return domain entities.

In this layer we also have the persistence entities that are used by the ORM.

```C#
  /// <summary>
  /// Repository implementation for the Category entity
  /// </summary>
  public class CategoryRepository : Repository<DbCategory>, ICategoryRepository
  {
      public CategoryRepository(DatabaseContext ctx, IServiceProvider provider) : base(ctx.Categories, provider)
      {
      }

      public async Task<bool> ExistsByName(string name, int? userId, int? currentId)
      {
          var filter = new Filter<DbCategory>(x => x.Name == name);

          if(userId.HasValue && userId != default)
              filter.And(x => x.UserId == userId.Value);

          if(currentId.HasValue && currentId != default)
              filter.And(x => x.Id != currentId.Value);
          
          return await _dbSet.AnyAsync(filter.GetExpression());
      }
...
```

## IoC (Inversion of Control)

Responsible for the dependency injection (DI) and resolving dependencies of services.

# Swagger

Swagger is configured with basic documentation. It's possible to see the input data and the returning data according to the response code

![image](https://github.com/user-attachments/assets/3e375be0-16c4-41bc-aebc-df7e09d5c0e0)

# Tests

The project contains two test projects: unit tests and integration tests.

## Unit tests

In our project, unit tests were performed in the domain layer, at the entity and domain service levels, using Moq to ensure there were no external dependencies (from repositories).

You may need configure *appsettings.Tests.json* to get it to work.

### Entities
![image](https://github.com/user-attachments/assets/0750163d-ceea-4ffa-8f0a-e45068f44737)
### Domain services
![image](https://github.com/user-attachments/assets/82a2db13-135f-4e17-b27f-afa4a1e6c37e)

## Integration tests

In our project, integration tests are performed using a test SQL Server with fixed data at the infrastructure, application, and API layers.

### Infrastructure
![image](https://github.com/user-attachments/assets/6b301d58-06d8-4dc4-b48e-c868f35b28ae)
### Application
![image](https://github.com/user-attachments/assets/85905bed-1c4a-42bc-ac3e-14336ad77269)
### Api
![image](https://github.com/user-attachments/assets/757237af-f4ae-42d5-9436-a0209b4c5e38)

