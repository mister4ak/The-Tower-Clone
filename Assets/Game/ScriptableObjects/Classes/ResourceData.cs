using UnityEngine;
using static Common.Enums;

namespace ScriptableObjects.Classes
{
    [CreateAssetMenu(fileName = "New Resource Data", menuName = "Resource Data")]
    public class ResourceData : ScriptableObject
    {
        [SerializeField] private ResourceType _type;
        [SerializeField] private Sprite _resourceIcon;

        public ResourceType Type => _type;
        public Sprite ResourceIcon => _resourceIcon;
    }
}