using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Core
{
    public class BulletModel
    {
        public PhysicsBody Body { get; }
        public bool IsActive { get; private set; } = true;

        public BulletModel(Vector2 pos, Vector2 direction)
        {
            Body = new PhysicsBody(pos, 1.0f);
            Body.Velocity = direction * 15f;
        }

        // Асинхронный полет пули
        public async UniTask RunLifeCycle(TimeSpan duration)
        {
            float elapsed = 0;
            float totalSeconds = (float)duration.TotalSeconds;

            while (elapsed < totalSeconds)
            {
                // Используем PlayerLoop.Update для синхронизации с кадрами Unity
                float dt = Time.deltaTime;
                Body.UpdatePhysics(dt);
                
                elapsed += dt;
                await UniTask.Yield(PlayerLoopTiming.Update);
            }

            IsActive = false;
        }
    }
}