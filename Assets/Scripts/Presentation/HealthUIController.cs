using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Game.Core;

namespace Game.Presentation
{
    public class HealthUIController : MonoBehaviour
    {
        [SerializeField] private HeartView _heartPrefab;
        [SerializeField] private Transform _container; // Тот самый объект с Layout Group

        private readonly List<HeartView> _hearts = new();
        private SignalBus _signalBus;
        private PlayerConfig _config;

        [Inject]
        public void Construct(SignalBus signalBus, PlayerConfig config)
        {
            _signalBus = signalBus;
            _config = config;
        }

        private void Start()
        {
            InitializeHearts();
            _signalBus.Subscribe<PlayerHealthChangedSignal>(OnHealthChanged);
        }

        private void InitializeHearts()
        {
            // Создаем максимальное количество сердец из конфига
            for (int i = 0; i < _config.MaxHealth; i++)
            {
                var heart = Instantiate(_heartPrefab, _container);
                _hearts.Add(heart);
            }
        }

        private void OnHealthChanged(PlayerHealthChangedSignal signal)
        {
            // Обновляем видимость сердец
            for (int i = 0; i < _hearts.Count; i++)
            {
                // Если индекс сердца меньше текущего здоровья — оно активно
                _hearts[i].SetActive(i < signal.CurrentHealth);
            }
        }

        private void OnDestroy()
        {
            _signalBus?.TryUnsubscribe<PlayerHealthChangedSignal>(OnHealthChanged);
        }
    }
}