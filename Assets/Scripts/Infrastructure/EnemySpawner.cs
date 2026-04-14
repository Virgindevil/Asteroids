using System.Collections.Generic;
using Game.Core;
using Zenject;
using UnityEngine;

namespace Game.Infrastructure
{
    public class EnemySpawner : ITickable
    {
        private readonly AsteroidFactory _factory;
        private readonly WorldConfig _worldConfig;
        private readonly SignalBus _signalBus;

        private readonly List<AsteroidModel> _activeAsteroids = new();
        private float _spawnTimer;

        public EnemySpawner(AsteroidFactory factory, WorldConfig worldConfig, SignalBus signalBus)
        {
            _factory = factory;
            _worldConfig = worldConfig;
            _signalBus = signalBus;
        }

        public void Tick()
        {
            _spawnTimer += Time.deltaTime;

            if (_spawnTimer >= 2f && _activeAsteroids.Count < _worldConfig.MaxEnemies)
            {
                SpawnAsteroid();
                _spawnTimer = 0;
            }

            // Обновляем физику всех астероидов
            for (int i = 0; i < _activeAsteroids.Count; i++)
            {
                _activeAsteroids[i].Update(Time.deltaTime);
            }
        }

        private void SpawnAsteroid()
        {
            // Берем случайный тип астероида из конфига
            var config = _worldConfig.Enemies[Random.Range(0, _worldConfig.Enemies.Count)];
            var asteroid = _factory.CreateRandomAsteroid(config);

            _activeAsteroids.Add(asteroid);

            // Кидаем сигнал презентации, чтобы создать View
            _signalBus.Fire(new AsteroidCreatedSignal { Asteroid = asteroid });
        }
    }

    public struct AsteroidCreatedSignal { public AsteroidModel Asteroid; }
}