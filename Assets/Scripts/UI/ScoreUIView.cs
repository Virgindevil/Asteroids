using UnityEngine;
using TMPro;
using Zenject;
using Game.Core;

namespace Game.UI
{
    public class ScoreUIView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Start()
        {
            // Подписываемся на изменение счета
            _signalBus.Subscribe<ScoreChangedSignal>(OnScoreChanged);
            _scoreText.text = "0"; // Начальное значение
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