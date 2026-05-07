using UnityEngine;

namespace Game.Core
{
    public interface ICameraProvider
    {
        float OrthoHeight { get; }
        float OrthoWidth { get; }

        Vector2 WorldToScreenPoint(Vector2 worldPosition);
    }
}