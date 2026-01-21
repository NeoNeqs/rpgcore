using Godot;

namespace rpgcore.gizmo;

[GlobalClass, Tool]
public partial class ReadonlyAttributes : Attributes { 
    public ReadonlyAttributes() {
        Flags = PropertyUsageFlags.ReadOnly; 
    }
}