using System.Collections.Generic;
using System.Linq;
using Game.Core;
using UnityEngine;
using Zenject;

namespace Game.Infrastructure
{
    public class EnemySimulator : ITickable
    {
        private readonly List<EnemyConfig> _enemyConfigs;
        private readonly EnemyFactory _factory;
        private readonly float _frictionMultiplier;
        private readonly MapService _mapService;
        private readonly EnemyRegistry _registry;
        private readonly SignalBus _signalBus;
        private readonly float _teleportOffset;

        public EnemySimulator(
            EnemyRegistry registry,
            EnemyFactory factory,
            MapService mapService,
            SignalBus signalBus,
            List<EnemyConfig> enemyConfigs,
            WorldConfig worldConfig)
        {
            _registry = registry;
            _factory = factory;
            _mapService = mapService;
            _signalBus = signalBus;
            _enemyConfigs = enemyConfigs;
            _frictionMultiplier = worldConfig.FrictionTimeMultiplier;
            _teleportOffset = worldConfig.TeleportBoundaryOffset;
        }

        public void Tick()
        {
            var dt = Time.deltaTime;
            var enemies = (List<EnemyModel>)_registry.ActiveEnemies;

            for (var i = enemies.Count - 1; i >= 0; i--)
            {
                var enemy = enemies[i];

                if (enemy.IsDead)
                {
                    HandleDeath(enemy);
                    _registry.Remove(enemy);
                    continue;
                }

                enemy.Update(dt, _frictionMultiplier);
                enemy.Body.TeleportIfOutOfBounds(
                    _mapService.Width + _teleportOffset,
                    _mapService.Height + _teleportOffset);
            }
        }

        private void HandleDeath(EnemyModel enemy)
        {
            _signalBus.Fire(new EnemyDestroyedSignal { Enemy = enemy });

            if (enemy is not AsteroidModel asteroid || !asteroid.CanSplit)
                return;

            var fragmentConfig = _enemyConfigs
                .FirstOrDefault(c => c.EnemyType == EnemyType.Asteroid && !c.CanSplit);

            if (fragmentConfig == null) return;

            foreach (var data in asteroid.GetFragments())
            {
                var fragment = _factory.CreateFragment(data, fragmentConfig);
                _registry.Add(fragment);
                _signalBus.Fire(new EnemyCreatedSignal { Enemy = fragment });
            }
        }
    }
}