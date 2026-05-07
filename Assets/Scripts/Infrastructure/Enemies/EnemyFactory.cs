using Game.Core;
using UnityEngine;

namespace Game.Infrastructure
{
    public class EnemyFactory
    {
        private readonly MapService _mapService;
        private readonly PlayerModel _player;

        private readonly float _spawnAreaDivisor;
        private readonly float _teleportOffset;

        public EnemyFactory(PlayerModel player, MapService mapService, WorldConfig worldConfig)
        {
            _player = player;
            _mapService = mapService;
            _spawnAreaDivisor = worldConfig.SpawnAreaDivisor;
            _teleportOffset = worldConfig.TeleportBoundaryOffset;
        }

        public EnemyModel Create(EnemyConfig config)
        {
            var spawnPos = GetRandomSpawnPosition();
            var velocity = GetRandomVelocity(spawnPos, config.Speed);

            return config.EnemyType switch
            {
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
            var half = _mapService.Width / _spawnAreaDivisor;
            var halfH = _mapService.Height / _spawnAreaDivisor;

            var targetPos = new Vector2(
                Random.Range(-half, half),
                Random.Range(-halfH, halfH)
            );

            return (targetPos - spawnPos).normalized * speed;
        }

        private Vector2 GetRandomSpawnPosition()
        {
            var w = _mapService.Width / 2f + _teleportOffset;
            var h = _mapService.Height / 2f + _teleportOffset;

            var side = Random.Range(0, 4);
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