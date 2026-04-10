using UnityEngine;

namespace Game.Core
{
    public interface IInputStrategy
    {
        Vector2 GetRotationDirection(Vector2 pos);
        bool IsAccelerating();          // Жмем ли газ
        bool IsShooting();              // Обычный выстрел
        bool IsLaserActive();           // Лазер
    }
}