using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows
{
    public class LosingWindow : BaseWindow
    {
        public event Action OnRestartButtonClicked;
        
        [SerializeField] private Button _restartButton;

        private void Awake()
        {
            _restartButton.interactable = false;
            _restartButton.onClick.AddListener(RestartButtonClicked);
        }

        public override void Enable(bool enable, Action finished = null)
        {
            base.Enable(enable, finished);
            _restartButton.interactable = enable;
        }

        private void RestartButtonClicked()
        {
            OnRestartButtonClicked?.Invoke();
        }

        private void OnDestroy()
        {
            _restartButton.onClick.RemoveListener(RestartButtonClicked);
        }
    }
}