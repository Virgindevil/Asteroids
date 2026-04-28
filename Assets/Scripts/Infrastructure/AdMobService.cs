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

        public void ShowRewardedVideo(Action onReward, Action onClosed)
        {
            if (_rewardedAd != null && _rewardedAd.CanShowAd())
            {
                // 1. Подписываемся на закрытие
                _rewardedAd.OnAdFullScreenContentClosed += () =>
                {
                    Debug.Log("Реклама закрыта пользователем");
                    onClosed?.Invoke();
                    LoadRewardedAd(); // Загружаем следующую
                };

                // 2. Показываем и обрабатываем награду
                _rewardedAd.Show((Reward reward) =>
                {
                    Debug.Log("Награда получена");
                    onReward?.Invoke();
                });
            }
            else
            {
                LoadRewardedAd();
            }
        }


        public void Dispose()
        {
            _rewardedAd?.Destroy();
        }
    }
}