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

        private PlayerViewModel _viewModel;

        [Inject]
        public void Construct(PlayerViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private void Update()
        {
            if (_viewModel == null) return;

            // Координаты (форматируем до 1 знака после запятой)
            _coordsText.text = $"POS: {_viewModel.Position.x:F1} : {_viewModel.Position.y:F1}";

            // Угол поворота
            _rotationText.text = $"ROT: {Mathf.RoundToInt(_viewModel.Rotation)}°";

            // Мгновенная скорость
            _speedText.text = $"SPEED: {_viewModel.Speed:F2} m/s";

            // Лазер (заряды и таймер пока в заглушке, скоро оживим)
            //_laserText.text = $"LASER: {_viewModel.LaserCharges} | CD: 0.0s";
        }
    }
}