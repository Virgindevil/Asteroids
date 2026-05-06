using System.Collections.Generic;
using Game.Core;

namespace Game.Core
{
    public interface IEnemyProvider
    {
        IReadOnlyList<EnemyModel> ActiveEnemies { get; }
    }
}