using Godot;
using rpgcore.component;

namespace rpgcore;

[GlobalClass, Tool]
public partial class ToolState : ComponentState {
    [Export] public int CurrentDurability { get; set; }
}