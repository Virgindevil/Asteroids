using Game.Core;
using UnityEngine;

namespace Game.Presentation
{
    public class UnityCameraProvider : MonoBehaviour, ICameraProvider
    {
        [SerializeField] private Camera _camera;

        public float OrthoHeight => _camera.orthographicSize * 2f;
        public float OrthoWidth => OrthoHeight * _camera.aspect;

        public Vector2 WorldToScreenPoint(Vector2 worldPosition)
        {
            var screenPoint = _camera.WorldToScreenPoint(worldPosition);
            return new Vector2(screenPoint.x, screenPoint.y);
        }
    }
}