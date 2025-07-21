using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ATG.UI
{
    public readonly struct FadeViewData
    {
        public float FadeDelayAfterShow {get;}
        public float FadeDelayAfterHide {get;}

        public Action AfterShowCallback {get;}
        public Action AfterHideCallback {get;}

        public FadeViewData(float fadeDelayAfterShow, float fadeDelayAfterHide, 
            Action afterShowCallback, Action afterHideCallback)
        {
            FadeDelayAfterShow = fadeDelayAfterShow;
            FadeDelayAfterHide = fadeDelayAfterHide;

            AfterShowCallback = afterShowCallback;
            AfterHideCallback = afterHideCallback;
        }
    }

    public sealed class FadeView: InfoView
    {
        private IEnumerator _fadeViewCoroutine;

        public override void Show(object sender, object data)
        {
            if(data is not FadeViewData fadeViewData)
                throw new ArgumentOutOfRangeException("data must be FadeViewData");

            _canvas.worldCamera = Camera.main;

            base.Show(sender, null);

            if(_fadeViewCoroutine != null)
            {
                StopCoroutine(_fadeViewCoroutine);
            }

            _fadeViewCoroutine = FadeViewWithDelay(fadeViewData.FadeDelayAfterShow, 
                fadeViewData.FadeDelayAfterHide, fadeViewData.AfterShowCallback, fadeViewData.AfterHideCallback);
                
            StartCoroutine(_fadeViewCoroutine);
        }

        public override void Hide()
        {
            base.Hide();
        }

        private IEnumerator FadeViewWithDelay(float delayAfterShow, float delayAfterHide, 
            Action afterShowCallback, Action afterHideCallback)
        {
            yield return new WaitForSeconds(delayAfterShow);

            afterShowCallback?.Invoke();
            
            FadeInAnimate(0f, fadeOutDuration, _source.Token).Forget();

            yield return new WaitForSeconds(delayAfterHide);
            
            afterHideCallback?.Invoke();

            _fadeViewCoroutine = null;
        }
    }
}