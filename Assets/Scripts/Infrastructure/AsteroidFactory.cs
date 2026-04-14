using UnityEngine;
using Game.Core;
using Zenject;

namespace Game.Infrastructure
{
    public class AsteroidFactory
    {
        private readonly WorldConfig _worldConfig;

        public AsteroidFactory(WorldConfig worldConfig)
        {
            _worldConfig = worldConfig;
        }

        public AsteroidModel CreateRandomAsteroid(EnemyConfig config)
        {
            Vector2 spawnPos = GetRandomSpawnPosition();

            // Летим в сторону центра карты с небольшим отклонением
            Vector2 targetPos = new Vector2(
                Random.Range(-_worldConfig.Width / 4, _worldConfig.Width / 4),
                Random.Range(-_worldConfig.Height / 4, _worldConfig.Height / 4)
            );

            Vector2 direction = (targetPos - spawnPos).normalized;
            Vector2 velocity = direction * config.Speed;

            return new AsteroidModel(config, spawnPos, velocity);
        }

        private Vector2 GetRandomSpawnPosition()
        {
            float w = _worldConfig.Width / 2f + 1f; // +1 метр за границу
            float h = _worldConfig.Height / 2f + 1f;

            // Выбираем случайную сторону: 0-верх, 1-низ, 2-лево, 3-право
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