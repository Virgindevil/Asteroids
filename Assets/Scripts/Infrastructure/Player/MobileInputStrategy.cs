using Game.Core;
using UnityEngine;

namespace Game.Infrastructure
{
    public class MobileInputStrategy : IInputStrategy
    {
        private bool _isLaser;
        private bool _isShooting;
        private Vector2 _lookDir;
        private Vector2 _moveDir;

        public Vector2 GetMoveDirection()
        {
            return _moveDir;
        }

        public Vector2 GetLookDirection(Vector2 _)
        {
            return _lookDir;
        }

        public bool IsShooting()
        {
            return _isShooting;
        }

        public bool IsLaserActive()
        {
            return _isLaser;
        }

        public void SetMoveDirection(Vector2 dir)
        {
            _moveDir = dir;
            if (dir.sqrMagnitude > 0.01f) _lookDir = dir;
        }

        public void SetShooting(bool value)
        {
            _isShooting = value;
        }

        public void SetLaser(bool value)
        {
            _isLaser = value;
        }
    }
}