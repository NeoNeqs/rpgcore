using Godot;
using rpgcore;

public partial class Main : Node {
    public override void _Ready() {
        var gizmo = GD.Load<Gizmo>("pick.tres");
        GD.Print(gizmo.GetComponentState<ToolState>()?.CurrentDurability);
        gizmo.OnUse();
        GD.Print(gizmo.GetComponentState<ToolState>()?.CurrentDurability);
        ResourceSaver.Save(gizmo, "pick.tres");
    }
}