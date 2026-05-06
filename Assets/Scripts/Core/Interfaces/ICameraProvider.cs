using UnityEngine;

namespace Game.Core
{
    public interface ICameraProvider
    {
        Camera Camera { get; }
        float OrthoHeight { get; }
        float OrthoWidth { get; }
    }
}
