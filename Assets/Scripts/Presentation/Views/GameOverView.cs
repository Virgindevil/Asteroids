using Game.Core;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Presentation
{
    public class GameOverView : MonoBehaviour
    {
        [SerializeField] private Button _exitButton;
        [SerializeField] private Button _continueButton;
        [SerializeField] private GameObject _gameOverPanel;
        private SignalBus _signalBus;

        private GameOverViewModel _viewModel;

        private void Awake()
        {
            _gameOverPanel.SetActive(false);

            _exitButton.onClick.AddListener(_viewModel.OnExitClicked);
            _continueButton.onClick.AddListener(_viewModel.OnContinueClicked);

            _signalBus.Subscribe<GameOverSignal>(ShowPanel);
            _signalBus.Subscribe<PlayerRevivedSignal>(HidePanel);
        }

        private void OnDestroy()
        {
            _signalBus?.TryUnsubscribe<GameOverSignal>(ShowPanel);
            _signalBus?.TryUnsubscribe<PlayerRevivedSignal>(HidePanel);
        }

        [Inject]
        public void Construct(GameOverViewModel viewModel, SignalBus signalBus)
        {
            _viewModel = viewModel;
            _signalBus = signalBus;
        }

        private void ShowPanel()
        {
            _viewModel.OnGameOver();
            _gameOverPanel.SetActive(true);
        }

        private void HidePanel()
        {
            _gameOverPanel.SetActive(false);
        }
    }
}