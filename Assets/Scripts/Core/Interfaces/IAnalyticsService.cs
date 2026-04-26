namespace Game.Core
{
    public interface IAnalyticsService
    {
        void LogEvent(string eventName, string parameterName = null, string parameterValue = null);
    }
}