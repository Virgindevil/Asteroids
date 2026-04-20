using System.Collections.Generic;
using Game.Core;

public interface IEnemyProvider 
{ 
    List<EnemyModel> ActiveEnemies { get; } 
}

public interface IProjectileProvider 
{ 
    List<BulletModel> ActiveProjectiles { get; } 
}