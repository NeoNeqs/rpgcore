using Godot;
using rpgcore.component;

namespace rpgcore.gizmo.components;

[GlobalClass, Tool]
public partial class WeaponSharedData : Resource {
    [Export, Notify] public WeaponType Type { private set; get; } = WeaponType.MainHand;
    [Export, Notify] public ulong BaseDamage { private set; get; } = 1;
    [Export, Notify] public float Speed { private set; get; } = 2.0f;
}