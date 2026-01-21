using Godot;

namespace rpgcore.gizmo.effect;

[GlobalClass, Tool]
public partial class Effect : Resource {
    public virtual void Execute(Gizmo source) { }
}