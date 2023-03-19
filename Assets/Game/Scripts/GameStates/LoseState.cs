using UI;
using Utils;

namespace GameStates
{
    public class LoseState : IState
    {
        private readonly GameUI _gameUI;
        public bool IsRestartButtonClicked { get; private set; }

        public LoseState(GameUI gameUI)
        {
            _gameUI = gameUI;
        }

        public void Tick()
        {
        }

        public void OnEnter()
        {
            _gameUI.LosingWindow.Enable(true);
            _gameUI.LosingWindow.OnRestartButtonClicked += OnRestartButtonClicked;
        }

        private void OnRestartButtonClicked()
        {
            IsRestartButtonClicked = true;
        }

        public void OnExit()
        {
            IsRestartButtonClicked = false;
            _gameUI.LosingWindow.Enable(false);
            _gameUI.LosingWindow.OnRestartButtonClicked -= OnRestartButtonClicked;
        }
    }
}