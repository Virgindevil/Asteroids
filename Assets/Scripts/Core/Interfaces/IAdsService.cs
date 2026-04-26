using System;

namespace Game.Core
{
    public interface IAdsService
    {
        void ShowRewardedVideo(Action onComplete);
    }
}