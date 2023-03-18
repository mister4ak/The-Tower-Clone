using UnityEngine;

namespace UI
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private LosingWindow _losingWindow;
        [SerializeField] private GameWindow _gameWindow;
        
        public LosingWindow LosingWindow => _losingWindow;

        public GameWindow GameWindow => _gameWindow;
    }
}