using Game.Core;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game.UI
{
    public class PlayerUIView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _coordsText;
        [SerializeField] private TextMeshProUGUI _rotationText;
        [SerializeField] private TextMeshProUGUI _speedText;
        [SerializeField] private TextMeshProUGUI _scoreText;

        private SignalBus _signalBus;
        private PlayerViewModel _viewModel;

        private void Start()
        {
            _signalBus.Subscribe<ScoreChangedSignal>(OnScoreChanged);
            _scoreText.text = "0";
        }

        private void Update()
        {
            if (_viewModel == null) return;
            _coordsText.text = "POS: " +
                               $"\n{_viewModel.Position.x:F1} : {_viewModel.Position.y:F1}";
            _rotationText.text = "ROT: " +
                                 $"\n{Mathf.RoundToInt(_viewModel.RoundRotation)}°";
            _speedText.text = "SPEED: " +
                              $"\n{_viewModel.Speed:F2} m/s";
        }

        private void OnDestroy()
        {
            _signalBus?.TryUnsubscribe<ScoreChangedSignal>(OnScoreChanged);
        }

        [Inject]
        public void Construct(PlayerViewModel viewModel, SignalBus signalBus)
        {
            _viewModel = viewModel;
            _signalBus = signalBus;
        }


        private void OnScoreChanged(ScoreChangedSignal signal)
        {
            _scoreText.text = signal.TotalScore.ToString();
        }
    }
}