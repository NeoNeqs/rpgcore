using Godot;

namespace rpgcore.gizmo.components;

[GlobalClass, Tool]
public partial class ArmorComponent : EquipmentComponent {
    [Export, Notify] public ArmorType ArmorType { private set; get; } = ArmorType.Light;
    [Export, Notify] public ArmorSlot Slot { private set; get; }

    protected override float GetAdditionalPrice() {
        return Pricing.GetArmorPrice(ArmorType, Slot);
    }

    protected override float GetPenalty() {
        return 1.0f;
    }
}