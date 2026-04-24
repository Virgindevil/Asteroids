using UnityEngine;
using TMPro;
using Zenject;
using Game.Core;

namespace Game.UI
{
    public class PlayerUIView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _coordsText;
        [SerializeField] private TextMeshProUGUI _rotationText;
        [SerializeField] private TextMeshProUGUI _speedText;
        [SerializeField] private TextMeshProUGUI _laserText;
        [SerializeField] private TextMeshProUGUI _scoreText;

        private SignalBus _signalBus;
        private PlayerViewModel _viewModel;

        [Inject]
        public void Construct(PlayerViewModel viewModel, SignalBus signalBus)
        {
            _viewModel = viewModel;
            _signalBus = signalBus;
        }

        private void Start()
        {
            // Подписываемся на изменение счета
            _signalBus.Subscribe<ScoreChangedSignal>(OnScoreChanged);
            _scoreText.text = "0"; // Начальное значение
        }

        private void Update()
        {
            if (_viewModel == null) return;

            // Координаты (форматируем до 1 знака после запятой)
            _coordsText.text = $"POS: \n{_viewModel.Position.x:F1} : {_viewModel.Position.y:F1}";

            // Угол поворота
            _rotationText.text = $"ROT: \n{Mathf.RoundToInt(_viewModel.RoundRotation)}°";

            // Мгновенная скорость
            _speedText.text = $"SPEED: \n{_viewModel.Speed:F2} m/s";

            // Лазер (заряды и таймер пока в заглушке, скоро оживим)

            _laserText.text = _viewModel.LaserStatusText;
        }


        private void OnScoreChanged(ScoreChangedSignal signal)
        {
            _scoreText.text = signal.TotalScore.ToString();
        }

        private void OnDestroy()
        {
            _signalBus?.TryUnsubscribe<ScoreChangedSignal>(OnScoreChanged);
        }
    }
}