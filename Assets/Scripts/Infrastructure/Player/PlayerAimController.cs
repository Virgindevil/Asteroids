using Game.Core;
using UnityEngine;
using Zenject;

public class PlayerAimController : ITickable, IInitializable
{
    private Camera _camera;
    private readonly PlayerModel _model;
    private readonly IInputStrategy _input;
    
    public PlayerAimController(PlayerModel model, IInputStrategy input)
    {
        _input = input;
        _model = model;
    }

    public void Initialize()
    {
        _camera = Camera.main;
    }

    public void Tick()
    {
        Vector2 playerScreenPos = _camera.WorldToScreenPoint(_model.Body.Position);
        Vector2 lookDir = _input.GetLookDirection(playerScreenPos);

        if (lookDir.sqrMagnitude > 0.01f)
            _model.Body.Rotation = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

    }
}
