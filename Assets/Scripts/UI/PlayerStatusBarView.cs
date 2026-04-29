using Game.Core;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.UI
{
    public class PlayerStatusBarView : MonoBehaviour
    {
        [SerializeField] private Slider[] _chargeSliders;
        [SerializeField] private Vector3 _offset = new Vector3(0, -1.2f, 0);

        private PlayerViewModel _viewModel;
        private Quaternion _fixedRotation;

        [Inject]
        public void Construct(PlayerViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private void Start()
        {
            _fixedRotation = Quaternion.identity;
        }

        private void LateUpdate()
        {
            // Техническая логика отображения (позиция/поворот) остается во View,
            // так как это визуальное поведение объекта в мире.
            transform.rotation = _fixedRotation;
            transform.position = transform.parent.position + _offset;

            UpdateVisuals();
        }

        private void UpdateVisuals()
        {
            for (int i = 0; i < _chargeSliders.Length; i++)
            {
                // View просто спрашивает: "Сколько мне нарисовать в i-м слайдере?"
                _chargeSliders[i].value = _viewModel.GetChargeForSlider(i);
            }
        }
    }
}
