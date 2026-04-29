using UnityEngine;
using UnityEngine.EventSystems;
using Game.Core;
using Game.Infrastructure;
using Zenject;

namespace Game.Presentation
{
    public class VirtualJoystickView : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [SerializeField] private RectTransform _background;
        [SerializeField] private RectTransform _handle;
        [SerializeField] private float _radius = 80f;

        private MobileInputStrategy _mobileInput;
        private Vector2 _center;

        [Inject]
        public void Construct(IInputStrategy inputStrategy)
        {
            // Работает только если забинден MobileInputStrategy
            _mobileInput = inputStrategy as MobileInputStrategy;
        }
        
        private void Start()
        {
            bool isMobile = Application.isMobilePlatform;
            gameObject.SetActive(isMobile);
        }
        
        public void OnPointerDown(PointerEventData e)
        {
            _center = e.position;
            _background.position = _center;
            _background.gameObject.SetActive(true);
        }

        public void OnDrag(PointerEventData e)
        {
            if (_mobileInput == null) return;

            Vector2 delta = e.position - _center;
            Vector2 clamped = Vector2.ClampMagnitude(delta, _radius);
            _handle.anchoredPosition = clamped;
            _mobileInput.SetMoveDirection(clamped / _radius);
        }

        public void OnPointerUp(PointerEventData e)
        {
            if (_mobileInput == null) return;

            _handle.anchoredPosition = Vector2.zero;
            _background.gameObject.SetActive(false);
            _mobileInput.SetMoveDirection(Vector2.zero);
        }
    }
}