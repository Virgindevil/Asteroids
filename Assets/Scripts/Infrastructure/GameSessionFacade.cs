using Game.Core;
using UnityEngine;
using Zenject;

namespace Game.Infrastructure
{ 
    public class GameSessionFacade
    {
        private readonly GameStateService _gameState;
        private readonly SignalBus _signalBus;
        private readonly IAnalyticsService _analytics;

        public GameSessionFacade(GameStateService gameState,
            SignalBus signalBus, IAnalyticsService analytics)
        {
            _gameState = gameState;
            _signalBus = signalBus;
            _analytics = analytics;
        }

        public void OnGameOver()
        {
            _gameState.PauseGame();
            _analytics.LogEvent("game_over");
        }

        public void OnPlayerRevived()
        {
            _gameState.ResumeGame();
            _signalBus.Fire(new PlayerRevivedSignal());
            _analytics.LogEvent("player_revived");
        }

        public void OnGameQuit()
        {
            _analytics.LogEvent("game_quit", "reason", "player_dead");
            _gameState.ResumeGame();
            Application.Quit();
        }
    }
}
