using System.Collections.Generic;
using Extensions;
using UnityEngine;

public class SpawnZone : MonoBehaviour
{
    [SerializeField] private List<Zone> _zones;

    public Vector2 GetRandomPoint()
    {
        var randomZone = _zones.GetRandomElement();
        return randomZone.GetRandomPoint();
    }
}