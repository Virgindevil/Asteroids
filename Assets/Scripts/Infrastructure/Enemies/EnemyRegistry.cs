using System.Collections.Generic;
using Game.Core;

namespace Game.Infrastructure
{
    public class EnemyRegistry : IEnemyProvider
    {
        private readonly List<EnemyModel> _enemies = new();

        public IReadOnlyList<EnemyModel> ActiveEnemies => _enemies;

        public void Add(EnemyModel enemy) => _enemies.Add(enemy);

        public void Remove(EnemyModel enemy) => _enemies.Remove(enemy);

        public bool Contains(EnemyModel enemy) => _enemies.Contains(enemy);
    }
}