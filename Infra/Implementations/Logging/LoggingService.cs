using Infra.Entities;
using Infra.Contexts;
using Application.Enums;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces.Logging;
using Domain.Interfaces.Authentication;

namespace Infra.Implementations.Logging
{
    /// <summary>
    /// Logging service implementation
    /// </summary>
    public class LoggingService : ILoggingService
    {
        private readonly InfraContext _context;
        private readonly DbSet<DbLog> _dbSet;
        private readonly List<DbLog> _logs = new();
        private ITokenService _tokenService;

        public LoggingService(InfraContext context, ITokenService tokenService)
        {
            _context = context;
            _dbSet = context.Logs;
            _tokenService = tokenService;
        }

        public void LogError(string message, string details)
        {
            _logs.Add(new()
            {
                Message = message,
                Details = details,
                Type = (int)LogTypeEnum.Error,
                Date = DateTime.UtcNow,
                UserId = _tokenService.GetToken()?.Id,
            });
        }

        public void LogInformation(string message, string details)
        {
            _logs.Add(new()
            {
                Message = message,
                Details = details,
                Type = (int)LogTypeEnum.Information,
                Date = DateTime.UtcNow,
                UserId = _tokenService.GetToken()?.Id,
            });
        }

        public void LogWarning(string message, string details)
        {
            _logs.Add(new()
            {
                Message = message,
                Details = details,
                Type = (int)LogTypeEnum.Warning,
                Date = DateTime.UtcNow,
                UserId = _tokenService.GetToken()?.Id,
            });
        }

        public void Log(string message, LogTypeEnum type)
        {
            _logs.Add(new()
            {
                Message = message,
                Type = (int)type,
                Date = DateTime.UtcNow,
                UserId = _tokenService.GetToken()?.Id,
            });

        }

        public void Log(string message, string details, LogTypeEnum type)
        {
            _logs.Add(new()
            {
                Message = message,
                Details = details,
                Type = (int)type,
                Date = DateTime.UtcNow,
                UserId = _tokenService.GetToken()?.Id,
            });
        }

        public async Task Persist()
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                foreach (var log in _logs)
                {
                    await _dbSet.AddAsync(log);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }

            _logs.Clear();
        }
    }
}
