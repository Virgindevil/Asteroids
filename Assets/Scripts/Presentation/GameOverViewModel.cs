using UnityEngine;
using Zenject;
using Game.Core;
using Game.Infrastructure;

namespace Game.Presentation
{
    public class GameOverViewModel
    {
        private readonly IAdsService _adsService;
        private readonly IAnalyticsService _analytics;
        private readonly GameStateService _gameState;
        private readonly SignalBus _signalBus;
        private bool _shouldRevive;

        public GameOverViewModel(IAdsService adsService, IAnalyticsService analytics, SignalBus signalBus, GameStateService  gameState)
        {
            _adsService = adsService;
            _analytics = analytics;
            _signalBus = signalBus;
            _gameState = gameState;
        }

        public void OnGameOver()
        {
            _gameState.PauseGame();
        }

        public void OnExitClicked()
        {
            _analytics.LogEvent("game_quit", "reason", "player_dead");
            _gameState.ResumeGame();
            Application.Quit();
        }

        public void OnContinueClicked()
        {
            _analytics.LogEvent("ad_start_click");
            _shouldRevive = false;

            _adsService.ShowRewardedVideo(
                onReward: () =>
                {
                    _shouldRevive = true;
                    _analytics.LogEvent("ad_watched_complete");
                },
                onClosed: () =>
                {
                    if (_shouldRevive)
                    {
                        _gameState.ResumeGame(); // Запускаем время
                        _signalBus.Fire(new PlayerRevivedSignal());
                    }
                }
            );
        }
    }
}