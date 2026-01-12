using Godot;

namespace rpgcore.gizmo.components;

[GlobalClass, Tool]
public partial class ArmorSharedData : Resource {
    [Export, Notify] public ArmorType ArmorType { private set; get; } = ArmorType.Light;
}