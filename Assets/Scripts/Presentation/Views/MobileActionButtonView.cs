using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;
using Game.Infrastructure;
using Game.Core;

namespace Game.Presentation
{
    public class MobileActionButtonView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private enum ActionType { Bullet, Laser }
        [SerializeField] private ActionType _type;

        [Inject(Optional = true)]
        private MobileInputStrategy _mobileInput;

        [Inject]
        private WorldConfig _worldConfig;

        private void Start()
        {
            // Если _mobileInput == null — мы точно не на мобилке
            bool isMobile = _mobileInput != null || _worldConfig.ForceMobileInput;
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
    }
}