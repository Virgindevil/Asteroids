using System;
using GoogleMobileAds.Api;
using UnityEngine;
using Game.Core;

namespace Game.Infrastructure
{
    public class AdMobService : IAdsService
    {
        private RewardedAd _rewardedAd;
        
        // Тестовый ID для Rewarded Video (Android). 
        // НЕ меняй на реальный, пока не проверишь работу на тестовом.
        private const string AdUnitId = "ca-app-pub-3940256099942544/5224354917";

        public AdMobService()
        {
            // Инициализация SDK при старте
            MobileAds.Initialize(status => {
                Debug.Log("[AdMob] SDK инициализирован.");
                LoadRewardedAd();
            });
        }

        public void LoadRewardedAd()
        {
            // Очистка старой рекламы перед загрузкой новой
            if (_rewardedAd != null)
            {
                _rewardedAd.Destroy();
                _rewardedAd = null;
            }

            var adRequest = new AdRequest();
            RewardedAd.Load(AdUnitId, adRequest, (ad, error) => {
                if (error != null || ad == null)
                {
                    Debug.LogError("[AdMob] Ошибка загрузки рекламы: " + error);
                    return;
                }

                _rewardedAd = ad;
                Debug.Log("[AdMob] Реклама загружена и готова к показу.");
            });
        }

        public void ShowRewardedVideo(Action onComplete)
        {
            if (_rewardedAd != null && _rewardedAd.CanShowAd())
            {
                _rewardedAd.Show((Reward reward) => {
                    Debug.Log("[AdMob] Игрок получил награду.");
                    onComplete?.Invoke();
                    LoadRewardedAd(); // Загружаем следующую рекламу заранее
                });
            }
            else
            {
                Debug.LogWarning("[AdMob] Реклама еще не готова. Выполняем действие по умолчанию (для теста).");
                // В демо можно оставить вызов действия, но в релизе лучше показать UI "Реклама загружается"
                onComplete?.Invoke(); 
            }
        }

        public void Dispose()
        {
            _rewardedAd?.Destroy();
        }
    }
}