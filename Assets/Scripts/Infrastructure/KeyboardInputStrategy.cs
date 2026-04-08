using Game.Core;
using UnityEngine;

namespace Game.Infrastructure
{
    public class KeyboardInputStrategy : IInputStrategy
    {
        public Vector2 GetRotationDirection()
        {
            Vector2 mousePos = Input.mousePosition;
            Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
            return (mousePos - screenCenter).normalized;
        }

        public bool IsAccelerating() => Input.GetKey(KeyCode.W);
    }
}