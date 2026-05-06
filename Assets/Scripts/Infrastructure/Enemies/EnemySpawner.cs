using System.Collections.Generic;
using System.Linq;
using Game.Core;
using UnityEngine;
using Zenject;

namespace Game.Infrastructure
{
    public class EnemySpawner : ITickable
    {
        private readonly EnemyRegistry _registry;
        private readonly EnemyFactory _factory;
        private readonly WorldConfig _worldConfig;
        private readonly List<EnemyConfig> _primaryConfigs; // кешируем один раз
        private readonly SignalBus _signalBus;

        private float _spawnTimer;

        public EnemySpawner(
            EnemyRegistry registry,
            EnemyFactory factory,
            WorldConfig worldConfig,
            List<EnemyConfig> enemyConfigs,
            SignalBus signalBus)
        {
            _registry = registry;
            _factory = factory;
            _worldConfig = worldConfig;
            _signalBus = signalBus;

            // Кешируем список допустимых конфигов один раз — без LINQ в Tick
            _primaryConfigs = enemyConfigs
                .Where(c => c.CanSplit || c.EnemyType == EnemyType.UFO)
                .ToList();
        }

        public void Tick()
        {
            _spawnTimer += Time.deltaTime;

            if (_spawnTimer < _worldConfig.EnemiesSpawnInterval) return;
            if (CountPrimaryEnemies() >= _worldConfig.MaxEnemies) return;

            SpawnRandom();
            _spawnTimer = 0f;
        }

        private void SpawnRandom()
        {
            if (_primaryConfigs.Count == 0) return;

            var config = _primaryConfigs[Random.Range(0, _primaryConfigs.Count)];
            var enemy = _factory.Create(config);
            _registry.Add(enemy);
            _signalBus.Fire(new EnemyCreatedSignal { Enemy = enemy });
        }

        // Считаем без LINQ — простой for, без аллокаций
        private int CountPrimaryEnemies()
        {
            var enemies = _registry.ActiveEnemies;
            int count = 0;
            for (int i = 0; i < enemies.Count; i++)
            {
                var e = enemies[i];
                if ((e is AsteroidModel a && a.CanSplit) || e is UfoModel)
                    count++;
            }
            return count;
        }
    }
}