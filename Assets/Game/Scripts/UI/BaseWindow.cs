using System;
using Extensions;
using UnityEngine;

namespace UI
{
    public class BaseWindow : MonoBehaviour
    {
        [SerializeField] protected CanvasGroup _canvasGroup;
        [SerializeField] protected float _fadeDuration;
        
        public virtual void Enable(bool enable, Action finished = null)
        {
            if (enable)
            {
                gameObject.SetActive(true);
                _canvasGroup.Show(_fadeDuration, callback: () =>
                {
                    finished?.Invoke();
                });
            }
            else
            {
                _canvasGroup.Hide(_fadeDuration, callback: () =>
                {
                    finished?.Invoke();
                    gameObject.SetActive(false);
                });
            }
        }
    }
}