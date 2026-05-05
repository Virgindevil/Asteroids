using Game.Core;
using UnityEngine;

namespace Game.Infrastructure
{
    public class KeyboardInputStrategy : IInputStrategy
    {
        public Vector2 GetLookDirection(Vector2 playerScreenPos)
        {
            Vector2 mousePos = Input.mousePosition;
            return (mousePos - playerScreenPos).normalized;
        }
        
        public Vector2 GetMoveDirection()
        {
            float x = Input.GetAxisRaw("Horizontal"); 
            float y = Input.GetAxisRaw("Vertical");   
            return new Vector2(x, y).normalized;
        }

        public bool IsShooting() => Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space);
        public bool IsLaserActive() => Input.GetMouseButton(1);
    }
}