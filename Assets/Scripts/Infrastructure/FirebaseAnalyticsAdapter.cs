using Firebase;
using Firebase.Analytics;
using UnityEngine;
using Game.Core;

namespace Game.Infrastructure
{
    public class FirebaseAnalyticsAdapter : IAnalyticsService
    {
        private bool _isInitialized;

        public FirebaseAnalyticsAdapter()
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
                var dependencyStatus = task.Result;
                if (dependencyStatus == DependencyStatus.Available) {
                    _isInitialized = true;
                } 
            });
        }

        public void LogEvent(string eventName, string parameterName = null, string parameterValue = null)
        {
            if (!_isInitialized) return;

            if (!string.IsNullOrEmpty(parameterName) && !string.IsNullOrEmpty(parameterValue))
            {
                FirebaseAnalytics.LogEvent(eventName, parameterName, parameterValue);
            }
            else
            {
                FirebaseAnalytics.LogEvent(eventName);
            }
        }
    }
}