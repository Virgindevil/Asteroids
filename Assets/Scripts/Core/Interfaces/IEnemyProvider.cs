using System.Collections.Generic;

namespace Game.Core
{
    public interface IEnemyProvider
    {
        IReadOnlyList<EnemyModel> ActiveEnemies { get; }
    }
}