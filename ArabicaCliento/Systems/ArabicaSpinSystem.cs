using Content.Shared.MouseRotator;
using Robust.Client.Graphics;
using Robust.Client.Input;
using Robust.Client.Player;
using Robust.Shared.Map;
using Robust.Shared.Timing;

namespace ArabicaCliento.Systems;


public class ArabicaSpinSystem : EntitySystem
{
    [Dependency] private readonly IInputManager _input = default!;
    [Dependency] private readonly IPlayerManager _player = default!;
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly IEyeManager _eye = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;
    private float _lastDegrees;

    public override void Update(float frameTime)
    {
        if (!_timing.IsFirstTimePredicted)
            return;

        if (!ArabicaConfig.SpinBotEnabled)
            if (!_input.MouseScreenPosition.IsValid)
                return;

        var player = _player.LocalEntity;

        if (player == null || !TryComp<MouseRotatorComponent>(player, out var rotator))
            return;

        Angle angle;
        Angle curRot;

        if (ArabicaConfig.SpinBotEnabled)
        {
            _lastDegrees += ArabicaConfig.SpinBotDegreesPerSecond * frameTime;
            angle = Angle.FromDegrees(_lastDegrees);
            curRot = _transform.GetWorldRotation(Transform(player.Value));
            if (_lastDegrees > 360f)
                _lastDegrees = 0;
        }
        else
        {
            var xform = Transform(player.Value);

            var coords = _input.MouseScreenPosition;
            var mapPos = _eye.PixelToMap(coords);

            if (mapPos.MapId == MapId.Nullspace)
                return;

            angle = (mapPos.Position - _transform.GetMapCoordinates(xform).Position).ToWorldAngle();
            curRot = _transform.GetWorldRotation(xform);
        }


        if (rotator.Simple4DirMode)
        {
            var angleDir = angle.GetCardinalDir();
            if (angleDir == curRot.GetCardinalDir())
                return;

            RaisePredictiveEvent(new RequestMouseRotatorRotationEvent()
            {
                Rotation = angle,
            });

            return;
        }

        var diff = Angle.ShortestDistance(angle, curRot);
        if (Math.Abs(diff.Theta) < rotator.AngleTolerance.Theta)
            return;

        if (rotator.GoalRotation != null)
        {
            var goalDiff = Angle.ShortestDistance(angle, rotator.GoalRotation.Value);
            if (Math.Abs(goalDiff.Theta) < rotator.AngleTolerance.Theta)
                return;
        }

        RaisePredictiveEvent(new RequestMouseRotatorRotationEvent
        {
            Rotation = angle
        });
    }
}