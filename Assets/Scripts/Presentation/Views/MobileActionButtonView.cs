using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;
using Game.Infrastructure;
using Game.Core;

namespace Game.Presentation
{
    // Вешаем этот скрипт на кнопку пули и кнопку лазера
    public class MobileActionButtonView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private enum ActionType { Bullet, Laser }
        [SerializeField] private ActionType _type;

        private MobileInputStrategy _mobileInput;
        private WorldConfig _worldConfig;

        [Inject]
        public void Construct(IInputStrategy inputStrategy, WorldConfig worldConfig)
        {
            _mobileInput = inputStrategy as MobileInputStrategy;
            _worldConfig = worldConfig;
        }

        private void Start()
        {
            bool isMobile = Application.isMobilePlatform || _worldConfig.ForceMobileInput;
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