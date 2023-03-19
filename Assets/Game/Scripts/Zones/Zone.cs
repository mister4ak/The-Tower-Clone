using UnityEngine;

namespace Zones
{
    public class Zone : MonoBehaviour
    {
        [SerializeField] private float _borderX;
        [SerializeField] private float _borderY;
        [SerializeField] private bool _isShowGizmo;

        public Vector2 GetRandomPoint()
        {
            var position = transform.position;
            return new Vector2(position.x + Random.Range(-_borderX, _borderX), position.y + Random.Range(-_borderY, _borderY));
        }
        
        private void OnDrawGizmos()
        {
            if (!_isShowGizmo) return;
            Gizmos.color = new Color(0f, 1f, 0, 0.3f);
            Gizmos.DrawCube(transform.position, new Vector3(_borderX * 2, _borderY * 2, 0.1f));
        }
    }
}