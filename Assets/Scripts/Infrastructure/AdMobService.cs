using System;
using Game.Core;
using GoogleMobileAds.Api;

namespace Game.Infrastructure
{
    public class AdMobService : IAdsService, IDisposable
    {
        private const string AdUnitId = "ca-app-pub-3940256099942544/5224354917";
        private RewardedAd _rewardedAd;

        public AdMobService()
        {
#if UNITY_ANDROID || UNITY_IOS
                MobileAds.Initialize(status => {
                    LoadRewardedAd();
                });
#endif
        }

        public void ShowRewardedVideo(Action onReward, Action onClosed)
        {
#if UNITY_ANDROID || UNITY_IOS
                if (_rewardedAd != null && _rewardedAd.CanShowAd())
                {
                    _rewardedAd.OnAdFullScreenContentClosed += () =>
                    {
                        onClosed?.Invoke();
                        LoadRewardedAd(); 
                    };

                    _rewardedAd.Show((Reward reward) =>
                    {
                        onReward?.Invoke();
                    });
                }
                else
                {
                    LoadRewardedAd();
                }
#else
            onReward?.Invoke();
            onClosed?.Invoke();
#endif
        }


        public void Dispose()
        {
            _rewardedAd?.Destroy();
            _rewardedAd = null;
        }

        public void LoadRewardedAd()
        {
            if (_rewardedAd != null)
            {
                _rewardedAd.Destroy();
                _rewardedAd = null;
            }

            var adRequest = new AdRequest();
            RewardedAd.Load(AdUnitId, adRequest, (ad, error) => { _rewardedAd = ad; });
        }
    }
}