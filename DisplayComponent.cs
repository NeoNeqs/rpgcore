using Godot;
using rpgcore.component;

namespace rpgcore;

[GlobalClass, Tool]
public partial class DisplayComponent : ComponentBase {
    public override ComponentState? GetDefaultState() {
        return null;
    }
}