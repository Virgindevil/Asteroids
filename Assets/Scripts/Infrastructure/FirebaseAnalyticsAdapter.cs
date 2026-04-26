using UnityEngine;
using Game.Core;

namespace Game.Infrastructure
{
    public class FirebaseAnalyticsAdapter : IAnalyticsService
    {
        public void LogEvent(string eventName, string parameterName = null, string parameterValue = null)
        {
            Debug.Log($"[ANALYTICS] Отправлен ивент: {eventName} | {parameterName} : {parameterValue}");
            // Здесь будет реальный код Firebase
        }
    }
}