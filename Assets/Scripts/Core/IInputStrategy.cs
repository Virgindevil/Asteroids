using UnityEngine;

namespace Game.Core
{
    public interface IInputStrategy
    {
        // Вектор от игрока к прицелу (мыши)
        Vector2 GetLookDirection(Vector2 playerScreenPos);
        // Вектор WASD
        Vector2 GetMoveDirection();
        
        bool IsShooting();
        bool IsLaserActive();
    }
}