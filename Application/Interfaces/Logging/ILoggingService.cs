using Application.Enums;

namespace Application.Interfaces.Logging
{
    /// <summary>
    /// Logging interface for application layer
    /// </summary>
    public interface ILoggingService
    {
        public void LogError(string message, string details = "");
        public void LogWarning(string message, string details = "");
        public void LogInformation(string message, string details = "");
        public void Log(string message, LogTypeEnum type);
        public void Log(string message, string details, LogTypeEnum type);
        public Task Persist();
    }
}