using ArabicaCliento.Overlays;
using ArabicaCliento.Systems.Abstract;
using Content.Client.CombatMode;
using Content.Client.Weapons.Ranged.Systems;
using Robust.Client.Graphics;
using Robust.Client.Input;
using Robust.Client.Player;
using Robust.Shared.Player;

namespace ArabicaCliento.Systems;

public class ArabicaGunAddOverlaySystem : LocalPlayerSystem
{
    [Dependency] private readonly IOverlayManager _overlayMan = default!;
    [Dependency] private readonly IEyeManager _eye = default!;
    [Dependency] private readonly IInputManager _input = default!;
    [Dependency] private readonly IPlayerManager _player = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;
    [Dependency] private readonly GunSystem _gun = default!;
    [Dependency] private readonly CombatModeSystem _combatMode = default!;

    private GunAimOverlay? _overlay;

    protected override void OnAttached(LocalPlayerAttachedEvent ev)
    {
        _overlay ??= new GunAimOverlay(EntityManager, _eye, _input, _player, _gun, _transform, _combatMode);
        _overlayMan.AddOverlay(_overlay);
    }

    protected override void OnDetached(LocalPlayerDetachedEvent ev)
    {
        if (_overlay != null) _overlayMan.RemoveOverlay(_overlay);
    }
}