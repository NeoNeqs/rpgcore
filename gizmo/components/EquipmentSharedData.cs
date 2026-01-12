using Godot;

namespace rpgcore.gizmo.components;

[GlobalClass]
[Tool]
public partial class EquipmentSharedData : Resource {
    [Export, Notify] public partial ulong MaxDurability { set; get; }
    [Export, Notify] public EquipmentSlot Slot { private set; get; }
    [Export, Notify] public ulong ItemLevel { private set; get; } = 1;
}