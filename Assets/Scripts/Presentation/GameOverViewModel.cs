using UnityEngine;
using Zenject;
using Game.Core;
using Game.Infrastructure;

namespace Game.Presentation
{
    public class GameOverViewModel
    {
        private readonly IAdsService _adsService;
        private readonly GameSessionFacade _session;
        private bool _shouldRevive;

        public GameOverViewModel(IAdsService adsService, GameSessionFacade session)
        {
            _adsService = adsService;
            _session = session;
        }

        public void OnGameOver()
        {
            _session.OnGameOver();
        }

        public void OnExitClicked()
        {
            _session.OnGameQuit();
        }

        public void OnContinueClicked()
        {
            _adsService.ShowRewardedVideo(
                onReward: () => _shouldRevive = true,
                onClosed: () =>
                {
                    if (_shouldRevive)
                    {
                        _shouldRevive = false; 
                        _session.OnPlayerRevived();
                    }
                }
            );
        }
    }
}