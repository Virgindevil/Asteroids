using UnityEngine;

namespace Game.Core
{
    public interface IInputStrategy
    {
        Vector2 GetLookDirection(Vector2 playerScreenPos);

        Vector2 GetMoveDirection();

        bool IsShooting();
        bool IsLaserActive();
    }
}