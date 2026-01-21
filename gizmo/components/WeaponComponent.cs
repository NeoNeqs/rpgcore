using Godot;
using rpgcore.gizmo.enums;

namespace rpgcore.gizmo.components;

[GlobalClass, Tool]
public partial class WeaponComponent : EquipmentComponent {
    [Export, Notify]
    public WeaponClass WeaponClass {
        private set {
            field = value;
            RecalculateStats();
        }
        get;
    } = WeaponClass.Axe;

    [Export, Notify]
    public HandRequirement Hand {
        private set {
            field = value;
            RecalculateStats();
        }
        get;
    } = HandRequirement.OneHanded;

    [Export, Notify] public WeaponSlot Slot { private set; get; }
    [Export, Notify] public ulong BaseDamage { private set; get; } = 1;
    [Export, Notify] public float Speed { private set; get; } = 2.0f;


    protected override float GetAdditionalPrice() {
        return Pricing.GetWeaponPrice(WeaponClass) * (int)Hand * 0.5f;
    }
}