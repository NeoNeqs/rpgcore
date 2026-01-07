using Godot;
using rpgcore.component;

namespace rpgcore;

[GlobalClass, Tool]
public partial class Gizmo : ComponentSystem<ComponentBase, Gizmo> {
    [Export] public StringName Id { private set; get; } = new();

    public void OnUse() {
        var toolState = GetComponentState<ToolState>();
        toolState?.CurrentDurability -= 1;
    }

    public override Gizmo Clone() {
        Gizmo baseObject = base.Clone();
        baseObject.Id = Id;
        return baseObject;
    }
}