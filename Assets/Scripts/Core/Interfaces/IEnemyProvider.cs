using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public interface IEnemyProvider
    {
        IReadOnlyList<EnemyModel> ActiveEnemies { get; }
    }
}
