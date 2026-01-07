using Godot;
using rpgcore.component;

namespace rpgcore;

[GlobalClass, Tool]
public partial class ToolComponent : ComponentBase {
    [Export]
    public int MaxDurability {
        private set {
            if (field == value) return;
            field = value;
            EmitChanged();
        }
        get;
    }

    [Export]
    public int Haste {
        private set {
            if (field == value) return;
            field = value;
            EmitChanged();
        }
        get;
    }

    public override void OnAction(Gizmo actionSource) {
        ToolState? toolState = GetState(actionSource);
        toolState?.CurrentDurability -= 1;
    }

    public override ToolState? GetDefaultState() {
        return new ToolState() {
            CurrentDurability = MaxDurability,
        };
    }

    public override ToolState? GetState(Gizmo gizmo) {
        return gizmo.GetComponentState<ToolComponent, ToolState>();
    }
}