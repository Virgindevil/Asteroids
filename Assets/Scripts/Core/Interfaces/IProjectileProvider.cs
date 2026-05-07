using System.Collections.Generic;

namespace Game.Core
{
    public interface IProjectileProvider
    {
        IReadOnlyList<BulletModel> ActiveProjectiles { get; }
    }
}