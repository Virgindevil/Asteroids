using Game.Core;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.UI
{
    public class PlayerStatusBarView : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private Slider[] _chargeSliders;
        [SerializeField] private Vector3 _offset = new(0, -1.2f, 0);

        private PlayerViewModel _viewModel;

        private void LateUpdate()
        {
            transform.rotation = Quaternion.identity;
            transform.position = _target.position + _offset;

            UpdateVisuals();
        }

        [Inject]
        public void Construct(PlayerViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private void UpdateVisuals()
        {
            for (var i = 0; i < _chargeSliders.Length; i++) _chargeSliders[i].value = _viewModel.GetChargeForSlider(i);
        }
    }
}