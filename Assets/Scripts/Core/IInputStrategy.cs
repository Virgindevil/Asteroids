using UnityEngine;

namespace Game.Core
{
    public interface IInputStrategy
    {
        Vector2 GetRotationDirection();
        bool IsAccelerating();
    }
}