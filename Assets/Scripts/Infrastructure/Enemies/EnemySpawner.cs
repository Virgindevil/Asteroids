using System.Collections.Generic;
using Game.Core;
using System.Linq;
using Zenject;
using UnityEngine;

namespace Game.Infrastructure
{
    public class EnemySpawner : ITickable, IEnemyProvider, IInitializable
    {
        private readonly EnemyFactory _factory;
        private readonly WorldConfig _worldConfig;
        private readonly SignalBus _signalBus;

        private readonly List<EnemyModel> _activeEnemies = new();
        public List<EnemyModel> ActiveEnemies => _activeEnemies;
        private float _spawnTimer;

        public EnemySpawner(EnemyFactory factory, WorldConfig worldConfig, SignalBus signalBus)
        {
            _factory = factory;
            _worldConfig = worldConfig;
            _signalBus = signalBus;
        }

        public void Initialize() { }

        public void Tick()
        {
            float dt = Time.deltaTime;

            for (int i = _activeEnemies.Count - 1; i >= 0; i--)
            {
                var enemy = _activeEnemies[i];

                if (enemy.IsDead)
                {
                    Debug.Log($"[Spawner] Removing dead enemy: {enemy.Config.EnemyType}");
                    HandleEnemyDeath(enemy);
                    _activeEnemies.RemoveAt(i);
                    continue;
                }

                enemy.Update(dt);
                enemy.Body.TeleportIfOutOfBounds(_worldConfig.Width+_worldConfig.Width/16, _worldConfig.Height+_worldConfig.Height/9);
            }

            _spawnTimer += dt;
            int primaryCount = _activeEnemies.Count(e => (e is AsteroidModel a && a.CanSplit) || e is UfoModel);

            if (_spawnTimer >= 2f && primaryCount < _worldConfig.MaxEnemies)
            {
                SpawnRandomEnemy();
                _spawnTimer = 0;
            }
        }

        private void HandleEnemyDeath(EnemyModel enemy)
        {
            // ЭТОТ СИГНАЛ ОБЯЗАН УНИЧТОЖАТЬ VIEW
            _signalBus.Fire(new EnemyDestroyedSignal { Enemy = enemy });

            if (enemy is AsteroidModel asteroid && asteroid.CanSplit)
            {
                var fragmentConfig = _worldConfig.Enemies.FirstOrDefault(e => 
                    e.EnemyType == "Asteroid" && !e.CanSplit);

                if (fragmentConfig != null)
                {
                    foreach (var data in asteroid.GetFragments())
                    {
                        var fragment = _factory.CreateFragment(data, fragmentConfig);
                        _activeEnemies.Add(fragment);
                        _signalBus.Fire(new EnemyCreatedSignal { Enemy = fragment });
                    }
                }
            }
        }

        private void SpawnRandomEnemy()
        {
            var validConfigs = _worldConfig.Enemies.Where(c => c.CanSplit || c.EnemyType == "UFO").ToList();
            if (validConfigs.Count == 0) return;

            var config = validConfigs[Random.Range(0, validConfigs.Count)];
            var enemy = _factory.Create(config);
            _activeEnemies.Add(enemy);
            _signalBus.Fire(new EnemyCreatedSignal { Enemy = enemy });
        }
    }
}