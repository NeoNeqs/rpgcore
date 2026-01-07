using Godot;
using rpgcore.component;

namespace rpgcore;

[GlobalClass, Tool]
public partial class ToolComponent : ComponentBase {
    [Export] public int MaxDurability;
    [Export] public int Haste;

    public override ToolState? GetDefaultState() {
        return new ToolState() {
            CurrentDurability = MaxDurability,
        };
    }
}