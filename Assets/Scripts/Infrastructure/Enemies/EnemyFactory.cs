using UnityEngine;
using Game.Core;
using Zenject;

namespace Game.Infrastructure
{
    public class EnemyFactory
    {
        private readonly PlayerModel _player;
        private readonly MapService _mapService;
        private readonly SignalBus _signalBus;

        public EnemyFactory(PlayerModel player, MapService mapService)
        {
            _player = player;
            _mapService = mapService;
        }

        public EnemyModel Create(EnemyConfig config)
        {
            Vector2 spawnPos = GetRandomSpawnPosition();
            Vector2 velocity = GetRandomVelocity(spawnPos, config.Speed);
            
            return config.EnemyType switch {
                EnemyType.Asteroid => new AsteroidModel(config, spawnPos, velocity),
                EnemyType.UFO => new UfoModel(config, spawnPos, velocity, _player),
                 _ => new AsteroidModel(config, spawnPos, velocity)
            };
        }

        public EnemyModel CreateFragment(FragmentData data, EnemyConfig fragmentConfig)
        {
            var asteroid = new AsteroidModel(fragmentConfig, data.Position, data.Velocity);
            return asteroid;
        }

        private Vector2 GetRandomVelocity(Vector2 spawnPos, float speed)
        {
            Vector2 targetPos = new Vector2(
                Random.Range(-_mapService.Width / 4, _mapService.Width / 4),
                Random.Range(-_mapService.Height / 4, _mapService.Height / 4)
            );

            return (targetPos - spawnPos).normalized * speed;
        }

        private Vector2 GetRandomSpawnPosition()
        {
            float w = _mapService.Width / 2f + 1f; 
            float h = _mapService.Height / 2f + 1f;

            int side = Random.Range(0, 4);
            return side switch
            {
                0 => new Vector2(Random.Range(-w, w), h),
                1 => new Vector2(Random.Range(-w, w), -h),
                2 => new Vector2(-w, Random.Range(-h, h)),
                3 => new Vector2(w, Random.Range(-h, h)),
                _ => Vector2.zero
            };
        }
    }
}