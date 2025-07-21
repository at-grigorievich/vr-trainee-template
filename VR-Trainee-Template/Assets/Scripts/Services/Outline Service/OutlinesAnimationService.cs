using System;
using System.Collections.Generic;
using System.Threading;
using cakeslice;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;

namespace ATG.Services.Outlines
{
    public sealed class OutlinesAnimationService: IInitializable, IDisposable
    {
        private OutlineEffect _outlineEffect;
        private readonly Dictionary<OutlineAnimationType, CancellationTokenSource> _animationSources = new();
        private readonly Dictionary<OutlineAnimationType, HashSet<object>> _animationOwners = new();

        public OutlinesAnimationService()
        {
            _outlineEffect = Camera.main.GetComponent<OutlineEffect>();    
        }
        
        public void Initialize()
        {
            Dispose();

            var outlines = GameObject.FindObjectsOfType<BaseOutlineView>();
            
            foreach (var baseOutlineView in outlines)
            {
                baseOutlineView.Init(this);
            }
        }

        public void AddAnimation(OutlineViewParameters config, object src)
        {
            if (_animationSources.ContainsKey(config.AnimationType) == true)
            {
                _animationOwners[config.AnimationType].Add(src);
                return;
            }
            
            CancellationTokenSource source = new();
            _animationSources.Add(config.AnimationType, source);

            if (_animationOwners.ContainsKey(config.AnimationType) == false)
            {
                _animationOwners.Add(config.AnimationType, new HashSet<object>());
            }
            
            switch (config.AnimationType)
            {
                case OutlineAnimationType.BlinkOne:
                case OutlineAnimationType.BlinkTwo:
                    BlinkAnimationAsync(config, true, source).Forget();
                    break;
            }
        }

        public void StopAnimation(OutlineViewParameters config, object src)
        {
            if(_animationSources.TryGetValue(config.AnimationType, out var cts) == false) return;

            if (_animationOwners[config.AnimationType].Contains(src) == true)
            {
                var owners = _animationOwners[config.AnimationType];
                
                owners.Remove(src);

                if (owners.Count == 0)
                {
                    cts.Cancel();
                    cts.Dispose();

                    _animationSources.Remove(config.AnimationType);
                }
            }
        }
        
        public void Dispose()
        {
            foreach (var cancellationTokenSource in _animationSources.Values)
            {
                cancellationTokenSource?.Cancel();
                cancellationTokenSource?.Dispose();
            }
            
            _animationOwners.Clear();
            _animationSources.Clear();
        }

        private async UniTask BlinkAnimationAsync(OutlineViewParameters config, bool fromInitial, CancellationTokenSource cts)
        {
            float curTime = 0f;

            Color startColor = fromInitial ? config.InitialColor : config.FinalColor;
            Color endcolor = fromInitial ? config.FinalColor : config.InitialColor;
            
            while (curTime < 1f)
            {
                if (cts.IsCancellationRequested == true) return;
                
                if(config.AnimationType == OutlineAnimationType.BlinkOne)
                {
                    _outlineEffect.lineColor0 = Color.Lerp(startColor, endcolor, curTime);
                }
                else
                {
                    _outlineEffect.lineColor1 = Color.Lerp(startColor, endcolor, curTime);
                }
                
                await UniTask.Yield(cancellationToken: cts.Token);
                
                curTime += config.Duration * Time.deltaTime;
            }
            
            BlinkAnimationAsync(config, !fromInitial, cts).Forget();
        }
    }
}