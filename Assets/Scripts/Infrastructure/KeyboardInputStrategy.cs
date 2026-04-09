using Game.Core;
using UnityEngine;

namespace Game.Infrastructure
{
    public class KeyboardInputStrategy : IInputStrategy
    {
        /*public Vector2 GetRotationDirection()
        {
            Vector2 mousePos = Input.mousePosition;
            Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
            return (mousePos - screenCenter).normalized;
        }*/

        public Vector2 GetRotationDirection(Vector2 playerPosition)
        {
            // Конвертация экранных координат мыши в мировые
            Vector3 mouseScreen = Input.mousePosition;
            mouseScreen.z = -Camera.main.transform.position.z; // Важно для 2D!
            Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);

            // Направление от игрока к курсору
            return (mouseWorld - playerPosition).normalized;
        }

        public bool IsAccelerating() => Input.GetKey(KeyCode.W);
    }
}