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

        [Inject]
        public void Construct(IInputStrategy inputStrategy)
        {
            _mobileInput = inputStrategy as MobileInputStrategy;
        }

        private void Start()
        {
            // Скрываем кнопки, если мы не на мобилках
            gameObject.SetActive(Application.isMobilePlatform);
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