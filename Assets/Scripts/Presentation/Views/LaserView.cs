using UnityEngine;
using Zenject;
using Game.Core;

namespace Game.Presentation
{
    [RequireComponent(typeof(LineRenderer))]
    public class LaserView : MonoBehaviour
    {
        private LineRenderer _lineRenderer;
        private SignalBus _signalBus;
        private bool _isActive;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<LaserStateChangedSignal>(OnLaserStateChanged);
        }

        private void Start()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.enabled = false;
        }

        private void OnLaserStateChanged(LaserStateChangedSignal signal)
        {
            _isActive = signal.IsActive;
            _lineRenderer.enabled = _isActive;
        }

        private void Update()
        {
            if (!_isActive) 
                return;

            // Рисуем луч от игрока вперед
            _lineRenderer.SetPosition(0, transform.position);
            // Допустим, длина лазера 10 метров
            Vector3 endPoint = transform.position + transform.right * 10f; 
            _lineRenderer.SetPosition(1, endPoint);
        }

        private void OnDestroy()
        {
            _signalBus?.Unsubscribe<LaserStateChangedSignal>(OnLaserStateChanged);
        }
    }
}