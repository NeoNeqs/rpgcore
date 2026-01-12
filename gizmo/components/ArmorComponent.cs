using Godot;
using rpgcore.component;

namespace rpgcore.gizmo.components;

[SharedData(typeof(ArmorSharedData))]
[GlobalClass, Tool]
public partial class ArmorComponent : EquipmentComponent {
}