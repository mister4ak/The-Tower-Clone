using UnityEngine;
using static Common.Enums;

namespace ScriptableObjects.Classes.Resources
{
    [CreateAssetMenu(fileName = "New ResourceData", menuName = "Resource Data")]
    public class ResourceData : ScriptableObject
    {
        [SerializeField] private ResourceType _type;
        [SerializeField] private int _price;
        [SerializeField, Min(1)] private int _countInPrefab = 1;

        [SerializeField] private Sprite _resourceIcon;

        [SerializeField] private Mesh _resourceMesh;

        [SerializeField] private Mesh _colliderMesh;

        public Mesh ResourceMesh => _resourceMesh;
        public Mesh ColliderMesh => _colliderMesh;
        public ResourceType Type => _type;
        public Sprite ResourceIcon => _resourceIcon;
        public int Price => _price;
        public int CountInPrefab => _countInPrefab;
    }
}