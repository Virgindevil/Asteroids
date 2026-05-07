using Game.Core;
using UnityEngine;
using Zenject;

namespace Game.Presentation
{
    [RequireComponent(typeof(LineRenderer))]
    public class LaserView : MonoBehaviour
    {
        private bool _isActive;
        private LineRenderer _lineRenderer;
        private SignalBus _signalBus;
        private PlayerViewModel _viewModel;

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.enabled = false;
        }

        private void Start()
        {
            _signalBus.Subscribe<LaserStateChangedSignal>(OnLaserStateChanged);
        }

        private void Update()
        {
            if (!_isActive) return;

            _lineRenderer.SetPosition(0, transform.position);
            var endPoint = transform.position + transform.right * _viewModel.LaserLength;
            _lineRenderer.SetPosition(1, endPoint);
        }

        private void OnDestroy()
        {
            _signalBus?.Unsubscribe<LaserStateChangedSignal>(OnLaserStateChanged);
        }

        [Inject]
        public void Construct(SignalBus signalBus, PlayerViewModel viewModel)
        {
            _signalBus = signalBus;
            _viewModel = viewModel;
        }

        private void OnLaserStateChanged(LaserStateChangedSignal signal)
        {
            _isActive = signal.IsActive;
            _lineRenderer.enabled = _isActive;
        }
    }
}