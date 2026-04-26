using UnityEngine;
using Zenject;
using Game.Core;

namespace Game.Presentation
{
    public class GameOverViewModel
    {
        private readonly IAdsService _adsService;
        private readonly IAnalyticsService _analytics;
        private readonly SignalBus _signalBus;

        public GameOverViewModel(IAdsService adsService, IAnalyticsService analytics, SignalBus signalBus)
        {
            _adsService = adsService;
            _analytics = analytics;
            _signalBus = signalBus;
        }

        public void OnExitClicked()
        {
            _analytics.LogEvent("game_quit", "reason", "player_dead");
            Application.Quit();
        }

        public void OnContinueClicked()
        {
            _analytics.LogEvent("ad_start_click");
            
            _adsService.ShowRewardedVideo(() => 
            {
                _analytics.LogEvent("ad_watched_complete");
                _signalBus.Fire(new PlayerRevivedSignal());
            });
        }
    }
}