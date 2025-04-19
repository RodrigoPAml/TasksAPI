namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Default",
                    policy => policy
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
          
            IoC.Resolver.Invoke(builder.Services, builder.Configuration);

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseCors("Default");
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.MapControllers();
          
            app.Use((context, next) =>
            {
                context.Request.EnableBuffering();
                return next();
            });

            app.Run();
        }
    }
}