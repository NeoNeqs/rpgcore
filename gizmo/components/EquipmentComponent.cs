using Godot;
using rpgcore.component;

namespace rpgcore.gizmo.components;

[SharedData(typeof(EquipmentSharedData))]
[GlobalClass]
[Tool]
public abstract partial class EquipmentComponent : GizmoComponent {
    [Export, Notify]
    public ulong Durability {
        private set {
            if (value > EquipmentSharedData.MaxDurability) {
                value = EquipmentSharedData.MaxDurability;
            }

            _durability.Set(value);
        }
        get => _durability.Get();
    }

    protected EquipmentComponent() {
        EquipmentSharedData.MaxDurabilityChanged += () => {
            if (Durability > EquipmentSharedData.MaxDurability) {
                Durability = EquipmentSharedData.MaxDurability;
            }
        };
    }
}