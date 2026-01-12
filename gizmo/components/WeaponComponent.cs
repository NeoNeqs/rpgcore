using Godot;
using rpgcore.component;

namespace rpgcore.gizmo.components;

[SharedData(typeof(WeaponSharedData))]
[GlobalClass, Tool]
public partial class WeaponComponent : EquipmentComponent { }