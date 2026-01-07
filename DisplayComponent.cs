using Godot;
using rpgcore.component;

namespace rpgcore;

[GlobalClass, Tool]
public partial class DisplayComponent : ComponentBase {
    public override void OnAction(Gizmo actionSource) {
        throw new System.NotImplementedException();
    }

    public override ComponentState? GetDefaultState() {
        return null;
    }

    public override ToolState? GetState(Gizmo gizmo) {
        return null;
    }
}