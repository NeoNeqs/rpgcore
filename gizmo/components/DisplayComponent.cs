using Godot;
using rpgcore.component;

namespace rpgcore.gizmo.components;

[SharedData(typeof(DisplaySharedData))]
[GlobalClass]
[Tool]
public partial class DisplayComponent : GizmoComponent {
    [Export] public string Lore { set; get; } = "";
}