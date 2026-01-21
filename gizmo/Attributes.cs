using Godot;
using rpgcore.gizmo.enums;

namespace rpgcore.gizmo;

[GlobalClass, Tool]
public partial class Attributes : Holder<Attribute> {
    [Signal]
    public delegate void AttributeChangedEventHandler(Attribute pAttribute, float pOldValue, float pNewValue);
    
    public override void OnChanged(Attribute t, float pOldValue, float pNewValue) {
        EmitSignalAttributeChanged(t, pOldValue, pNewValue); 
    }
}