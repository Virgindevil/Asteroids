using Game.Core;
using UnityEngine;

namespace Game.Presentation
{
    public class UnityCameraProvider : MonoBehaviour, ICameraProvider
    {
        [SerializeField] private Camera _camera;

        public Camera Camera => _camera;

        public float OrthoHeight => _camera.orthographicSize * 2f;
        public float OrthoWidth => OrthoHeight * _camera.aspect;
    }
}