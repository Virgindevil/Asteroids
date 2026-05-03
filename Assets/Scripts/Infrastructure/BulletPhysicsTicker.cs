using UnityEngine;
using Zenject;

public class BulletPhysicsTicker : ITickable
{
    private readonly IProjectileProvider _provider;
    
    public BulletPhysicsTicker(IProjectileProvider provider) 
    {
        _provider = provider;
    }
    public void Tick()
    {
        foreach (var b in _provider.ActiveProjectiles)
        {
            b.Body.UpdatePhysics(Time.deltaTime);
            b.LifeTime -= Time.deltaTime;
            if (b.LifeTime <= 0) b.IsActive = false;
        }
    }
}
