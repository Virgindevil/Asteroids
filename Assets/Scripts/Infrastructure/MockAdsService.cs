using System;
using UnityEngine;
using Game.Core;

namespace Game.Infrastructure
{
    public class MockAdsService : IAdsService
    {
        public void ShowRewardedVideo(Action onComplete)
        {
            Debug.Log("[ADS] Показ видеорекламы начался...");
            // Имитация успешного просмотра
            onComplete?.Invoke();
        }
    }
}