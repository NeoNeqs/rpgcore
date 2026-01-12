using Godot;
using rpgcore.gizmo;
using rpgcore.gizmo.components;

public partial class Main : Node {
    public override void _Ready() {
        var pick = GD.Load<Gizmo>("res://resources/bone_melted_faceplate.tres");
        GD.Print(pick.GetComponent<DisplayComponent>()!.DisplaySharedData.DisplayName);
    }
}