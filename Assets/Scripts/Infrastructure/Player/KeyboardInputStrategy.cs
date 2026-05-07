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
            var x = Input.GetAxisRaw("Horizontal");
            var y = Input.GetAxisRaw("Vertical");
            return new Vector2(x, y).normalized;
        }

        public bool IsShooting()
        {
            return Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space);
        }

        public bool IsLaserActive()
        {
            return Input.GetMouseButton(1);
        }
    }
}