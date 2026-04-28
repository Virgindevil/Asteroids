using Game.Core;
using UnityEngine;

namespace Game.Infrastructure
{
    /// <summary>
    /// Паттерн Facade: скрывает сложность создания и уничтожения врагов.
    /// Остальные системы работают только с EnemyFacade, не зная о Spawner и Factory.
    /// </summary>
    public class EnemyFacade
    {
        private readonly EnemySpawner _spawner;
        private readonly EnemyFactory _factory;

        public EnemyFacade(EnemySpawner spawner, EnemyFactory factory)
        {
            _spawner = spawner;
            _factory = factory;
        }

        public System.Collections.Generic.List<EnemyModel> GetActiveEnemies() 
            => _spawner.ActiveEnemies;

        public void SpawnFragment(FragmentData data, EnemyConfig config)
        {
            var fragment = _factory.CreateFragment(data, config);
            _spawner.AddEnemy(fragment);
        }

        public void RemoveEnemy(EnemyModel enemy) 
            => _spawner.RemoveEnemy(enemy);
    }
}