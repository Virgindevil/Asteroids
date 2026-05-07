using Game.Core;
using Game.Infrastructure;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Game.Presentation
{
    public class MobileActionButtonView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private ActionType _type;

        [Inject(Optional = true)] private MobileInputStrategy _mobileInput;

        [Inject] private WorldConfig _worldConfig;

        private void Start()
        {
            var isMobile = _mobileInput != null || _worldConfig.ForceMobileInput;
            gameObject.SetActive(isMobile);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_mobileInput == null) return;

            if (_type == ActionType.Bullet) _mobileInput.SetShooting(true);
            else _mobileInput.SetLaser(true);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_mobileInput == null) return;

            if (_type == ActionType.Bullet) _mobileInput.SetShooting(false);
            else _mobileInput.SetLaser(false);
        }

        private enum ActionType
        {
            Bullet,
            Laser
        }
    }
}