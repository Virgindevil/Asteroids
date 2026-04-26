using Game.Core;
using UnityEngine;

namespace Game.Infrastructure
{
    public class MobileInputStrategy : IInputStrategy
    {
        private Vector2 _moveDir;
        private Vector2 _lookDir;
        private bool _isShooting;
        private bool _isLaser;

        public void SetMoveDirection(Vector2 dir) => _moveDir = dir;
        public void SetLookDirection(Vector2 dir) => _lookDir = dir;
        public void SetShooting(bool value) => _isShooting = value;
        public void SetLaser(bool value) => _isLaser = value;

        public Vector2 GetMoveDirection() => _moveDir;
        public Vector2 GetLookDirection(Vector2 _) => _lookDir;
        public bool IsShooting() => _isShooting;
        public bool IsLaserActive() => _isLaser;
    }
}