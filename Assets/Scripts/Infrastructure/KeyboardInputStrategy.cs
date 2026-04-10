using Game.Core;
using UnityEngine;

namespace Game.Infrastructure
{
    public class KeyboardInputStrategy : IInputStrategy
    {
        public Vector2 GetLookDirection(Vector2 playerScreenPos)
        {
            // Направление от позиции игрока на экране до курсора мыши
            Vector2 mousePos = Input.mousePosition;
            return (mousePos - playerScreenPos).normalized;
        }
        
        public Vector2 GetMoveDirection()
        {
            float x = Input.GetAxisRaw("Horizontal"); // A (-1) D (1)
            float y = Input.GetAxisRaw("Vertical");   // S (-1) W (1)
            return new Vector2(x, y).normalized;
        }

        public bool IsShooting() => Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space);
        public bool IsLaserActive() => Input.GetMouseButtonDown(1);
    }
}