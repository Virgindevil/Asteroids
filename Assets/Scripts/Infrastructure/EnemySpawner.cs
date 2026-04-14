using System.Collections.Generic;
using Game.Core;
using Zenject;
using UnityEngine;

namespace Game.Infrastructure
{
    public class EnemySpawner : ITickable
    {
        private readonly EnemyFactory _factory; // Используем новую фабрику
        private readonly WorldConfig _worldConfig;
        private readonly SignalBus _signalBus;

        private readonly List<EnemyModel> _activeEnemies = new();
        private float _spawnTimer;

        public EnemySpawner(EnemyFactory factory, WorldConfig worldConfig, SignalBus signalBus)
        {
            _factory = factory;
            _worldConfig = worldConfig;
            _signalBus = signalBus;
        }

        public void Tick()
        {
            _spawnTimer += Time.deltaTime;

            if (_spawnTimer >= 2f && _activeEnemies.Count < _worldConfig.MaxEnemies)
            {
                SpawnEnemy(); // Переименовали метод
                _spawnTimer = 0;
            }

            for (int i = 0; i < _activeEnemies.Count; i++)
            {
                var enemy = _activeEnemies[i];
                enemy.Update(Time.deltaTime);
                enemy.Body.TeleportIfOutOfBounds(_worldConfig.Width, _worldConfig.Height);
            }
        }

        private void SpawnEnemy()
        {
            var config = _worldConfig.Enemies[Random.Range(0, _worldConfig.Enemies.Count)];
            var enemy = _factory.Create(config);

            _activeEnemies.Add(enemy);

            // Просто передаем enemy. Никаких (AsteroidModel) или (Core.EnemyModel)
            _signalBus.Fire(new EnemyCreatedSignal { Enemy = (Core.EnemyModel)enemy });
        }
    }
}