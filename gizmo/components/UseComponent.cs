using Godot;
using rpgcore.gizmo.effect;

namespace rpgcore.gizmo.components;

[GlobalClass, Tool]
public partial class UseComponent : GizmoComponent {
    [Export, Notify] public float Cooldown { get; set; } = 1.0f;
    [Export, Notify] public Effect[] Effects { get; set; } = [];

    public double LastUseTime { get; private set; } = double.MinValue;

    public bool CanUse(double currentTime) => currentTime >= LastUseTime + Cooldown;

    public void Trigger(Gizmo owner) {
        ulong currentTime = Time.GetTicksUsec();
        if (!CanUse(currentTime)) return;

        LastUseTime = currentTime;
        foreach (Effect effect in Effects) {
            effect.Execute(owner);
        }
        EmitChanged();
    }
}