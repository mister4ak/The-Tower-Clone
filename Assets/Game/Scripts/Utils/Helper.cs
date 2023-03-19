using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public static class Helper
	{
        #region CameraMain

        private static Camera _camera;
        
        public static Camera Camera
        {
            get
            {
                if (_camera == null)
                    _camera = Camera.main;

                return _camera;
            }
        }

        #endregion
       
        #region WaitDictionary
        
        private static readonly Dictionary<float, WaitForSeconds> WaitDictionary = new Dictionary<float, WaitForSeconds>();
        public static WaitForSeconds GetWait(float time)
        {
            if (WaitDictionary.TryGetValue(time, out var wait))
                return wait;

            WaitDictionary[time] = new WaitForSeconds(time);
            return WaitDictionary[time];
        }

        public static IEnumerator WaitCoroutine(float duration, Action finished = null)
        {
            yield return GetWait(duration);
            finished?.Invoke();
        }
        
        #endregion
    }
}
