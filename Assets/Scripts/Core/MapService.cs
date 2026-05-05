using UnityEngine;
using Zenject;

public class MapService : IInitializable
{
    public float Width { get; private set; }
    public float Height { get; private set; }

    public void Initialize()
    {
        var cam = Camera.main;

        float orthoHeight = cam.orthographicSize * 2f;
        float orthoWidth = orthoHeight * cam.aspect;

        Height = orthoHeight;
        Width = orthoWidth;
    }
}