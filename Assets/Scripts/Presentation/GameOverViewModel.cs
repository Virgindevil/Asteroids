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
        private bool _shouldRevive;

        public GameOverViewModel(IAdsService adsService, IAnalyticsService analytics, SignalBus signalBus)
        {
            _adsService = adsService;
            _analytics = analytics;
            _signalBus = signalBus;
        }

        public void OnGameOver()
        {
            Time.timeScale = 0f;
        }

        public void OnExitClicked()
        {
            _analytics.LogEvent("game_quit", "reason", "player_dead");
            Time.timeScale = 1f;
            Application.Quit();
        }

        public void OnContinueClicked()
        {
            _analytics.LogEvent("ad_start_click");
            _shouldRevive = false;

            _adsService.ShowRewardedVideo(
                onReward: () =>
                {
                    // Игрок досмотрел до конца, помечаем, что его надо воскресить
                    _shouldRevive = true;
                    _analytics.LogEvent("ad_watched_complete");
                },
                onClosed: () =>
                {
                    // Пользователь нажал на "X"
                    if (_shouldRevive)
                    {
                        Time.timeScale = 1f; // Запускаем время
                        _signalBus.Fire(new PlayerRevivedSignal());
                    }
                    else
                    {
                        // Опционально: если закрыл раньше времени, 
                        // оставляем Time.timeScale = 0 и панель смерти висеть
                        Debug.Log("Реклама закрыта без получения награды");
                    }
                }
            );
        }
    }
}