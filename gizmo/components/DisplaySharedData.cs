using Godot;

namespace rpgcore.gizmo.components;


[GlobalClass, Tool]
public partial class DisplaySharedData : Resource {
    [Export] public string DisplayName { set; get; } = "";
    [Export] public Texture2D? Icon { set; get; }
}