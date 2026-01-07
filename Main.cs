using Godot;
using rpgcore;

public partial class Main : Node {
    public override void _Ready() {
        // var gizmo = GD.Load<Gizmo>("res://pick.tres");
        var gizmo = GD.Load<Gizmo>("user://pick.tres");
        GD.Print(gizmo.GetComponentState<ToolComponent, ToolState>()?.CurrentDurability);
    }
}