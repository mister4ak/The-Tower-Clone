using UnityEngine;

namespace Enemy
{
    public class EnemyMover : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody;

        private Vector2 _targetPosition;
        private float _moveSpeed;
        private bool _isInitialized;
        private bool _isMoving;

        public void Initialize(float moveSpeed)
        {
            _moveSpeed = moveSpeed;
        }
        
        public void MoveTo(Vector2 targetPosition)
        {
            _targetPosition = targetPosition;
            _isMoving = true;
        }

        public void StopMoving()
        {
            _rigidbody.velocity = Vector2.zero;
            _isMoving = false;
        }

        private void FixedUpdate()
        {
            if (!_isMoving) return;
            Move();
        }

        private void Move()
        {
            var currentPosition = _rigidbody.position;
            var direction = (_targetPosition - currentPosition).normalized;
            _rigidbody.velocity = direction * _moveSpeed;
        }
    }
}