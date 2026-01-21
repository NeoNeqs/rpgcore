using Godot;

namespace rpgcore.gizmo.components;

[GlobalClass]
[Tool]
public partial class DisplayComponent : GizmoComponent {
    [Export, Notify] public string DisplayName { set; get; } = "";

    [Export(PropertyHint.MultilineText), Notify]
    public string Lore { set; get; } = "";

    [Export(PropertyHint.File, "Texture2D"), Notify]
    public string? Icon { set; get; }

    [Export(PropertyHint.File, "PackedScene"), Notify]
    public string? Model { set; get; }
}