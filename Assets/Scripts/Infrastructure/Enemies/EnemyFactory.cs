using UnityEngine;
using Game.Core;
using Zenject;

namespace Game.Infrastructure
{
    public class EnemyFactory
    {
        private readonly PlayerModel _player;
        private readonly WorldConfig _worldConfig;
        private readonly SignalBus _signalBus;

        public EnemyFactory(PlayerModel player, WorldConfig worldConfig, SignalBus signalBus)
        {
            _player = player;
            _worldConfig = worldConfig;
            _signalBus = signalBus;
        }

        public EnemyModel Create(EnemyConfig config)
        {
            Vector2 spawnPos = GetRandomSpawnPosition();
            Vector2 velocity = GetRandomVelocity(spawnPos, config.Speed);

            return config.EnemyType switch
            {
                "Asteroid" => new AsteroidModel(config, spawnPos, velocity),
                "UFO" => new UfoModel(config, spawnPos, velocity, _player),
                _ => new AsteroidModel(config, spawnPos, velocity)
            };
        }

        public EnemyModel CreateFragment(FragmentData data, EnemyConfig fragmentConfig)
        {
            // Создаем модель обломка, используя позицию и скорость из FragmentData
            var asteroid = new AsteroidModel(fragmentConfig, data.Position, data.Velocity);
    
            // МЫ УБРАЛИ ОТСЮДА _signalBus.Fire! 
            // Теперь только EnemySpawner сообщает системе о появлении врага.
    
            return asteroid;
        }

        private Vector2 GetRandomVelocity(Vector2 spawnPos, float speed)
        {
            // Летим в сторону центра с разбросом
            Vector2 targetPos = new Vector2(
                Random.Range(-_worldConfig.Width / 4, _worldConfig.Width / 4),
                Random.Range(-_worldConfig.Height / 4, _worldConfig.Height / 4)
            );

            return (targetPos - spawnPos).normalized * speed;
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