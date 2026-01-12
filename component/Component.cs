using Godot;
using Godot.Collections;

namespace rpgcore.component;

[Tool]
public abstract partial class Component : Resource {
    public override Array<Dictionary> _GetPropertyList() {
        var properties = new Array<Dictionary>();
        _RegisterGeneratedProperties(properties);
        return properties;
    }

    public override Variant _Get(StringName property) {
        return _TryGetGeneratedProperty(property, out Variant value) ? value : default;
    }

    public override bool _Set(StringName property, Variant value) {
        return _TrySetGeneratedProperty(property, value);
    }

    protected virtual bool _TryGetGeneratedProperty(StringName property, out Variant value) {
        value = default;
        return false;
    }

    protected virtual bool _TrySetGeneratedProperty(StringName property, Variant value) {
        return false;
    }

    // Hook for the Generator
    protected virtual void _RegisterGeneratedProperties(Array<Dictionary> properties) { }
}