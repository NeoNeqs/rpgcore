using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;

namespace rpgcore.gizmo;

// IMPORTANT: Careful with adding float properties here. See why in `_Set()` definition down below.

[Tool]
public abstract partial class Holder<[MustBeVariant] T> : Resource where T : struct, Enum {
    // [Signal]
    // public delegate void ChangedEventHandler(Attribute pAttribute, float pOldValue, float pNewValue);
    protected PropertyUsageFlags Flags { set; get; } = PropertyUsageFlags.None;

    // ReSharper disable once InconsistentNaming
    private Godot.Collections.Dictionary<string, float> _Data {
        set {
            field = value;
#if TOOLS
            NotifyPropertyListChanged();
#endif
        }
        get;
    } = new();

    public abstract void OnChanged(T t, float pOldValue, float pNewValue);

    public float Get2(T t, float pDefault = 0.0f) {
        var key = t.ToString();
        if (_Data.TryGetValue(key, out float value)) {
            return value;
        }

        _Data[key] = pDefault;
        // EmitSignalChanged(pAttribute, pDefault, pDefault);
        OnChanged(t, pDefault, pDefault);
        NotifyPropertyListChanged();
        return pDefault;
    }

    public void Set2(T t, float pNewValue) {
        var key = t.ToString();

        _Data.TryGetValue(key, out float oldValue);

        if (float.IsPositiveInfinity(pNewValue)) {
            _Data.Remove(key);
        } else {
            _Data[key] = pNewValue;
        }

        NotifyPropertyListChanged();
        // EmitSignalChanged(pAttribute, oldValue, pNewValue);
        OnChanged(t, oldValue, oldValue);
    }

    public void Increase(T t, float pIncrement) {
        var key = t.ToString();
        _Data.TryGetValue(key, out float oldValue);

        float newValue = oldValue + pIncrement;
        _Data[key] = newValue;
        // EmitSignalChanged(pAttribute, oldValue, newValue);
        OnChanged(t, oldValue, newValue);
        NotifyPropertyListChanged();
    }

    public float Sum() {
        return _Data.Values.Sum();
    }

    public int CountActive() {
        return _Data.Values.Select(f => f > 0).Count();
    }

    public static T[] GetValues() {
        return Enum.GetValues<T>();
    }

    public IReadOnlyDictionary<string, float> Iter() {
        return _Data.AsReadOnly();
    }

    public override Array<Dictionary> _GetPropertyList() {
        Array<Dictionary> propertyList = [
#if TOOLS
            // Helper properties for the Inspector that allow adding / removing values
            ExportUtils.ExportEnumEditor<T>(nameof(Add), Flags),
#endif
            // Actual storage of values:
            ExportUtils.ExportStorage(nameof(_Data), Variant.Type.Dictionary),
        ];
        // Helper properties. Those basically display the contents of the dictionaries as normal properties. Only used in the editor.
#if TOOLS
        foreach ((string str, float _) in _Data) {
            propertyList.Add(
                ExportUtils.Export(str, Variant.Type.Float, PropertyHint.None, "",
                    PropertyUsageFlags.Editor | Flags)
            );
        }
#endif
        return propertyList;
    }

    /// Redirects storage of float properties to _Data.
    public override bool _Set(StringName pProperty, Variant pValue) {
        var propertyName = pProperty.ToString();

        // IMPORTANT: ALL Decimal properties of this class are handled through here!

        // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
        // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
        // Only care about floats.
        switch (pValue.VariantType) {
            case Variant.Type.Float: {
                var floatValue = pValue.As<float>();
                float oldValue = _Data[propertyName];
#if TOOLS
                if (float.IsPositiveInfinity(floatValue)) {
                    _Data.Remove(propertyName);
                    OnChanged(Enum.Parse<T>(propertyName), oldValue, floatValue);
                    // EmitSignalChanged(Enum.Parse<Attribute>(propertyName), oldValue, floatValue);
                    NotifyPropertyListChanged();
                    return true;
                }
#endif

                _Data[propertyName] = floatValue;
                // EmitSignalChanged(Enum.Parse<T>(propertyName), oldValue, floatValue);
                OnChanged(Enum.Parse<T>(propertyName), oldValue, floatValue);
                return true;
            }
            case Variant.Type.Int: {
                if (propertyName == nameof(Add)) {
                    _ = Get2(pValue.As<T>());
                }

                break;
            }
        }

        return false;
    }

    public override Variant _Get(StringName pProperty) {
        if (_Data.TryGetValue(pProperty.ToString(), out float value)) {
            return Variant.From(value);
        }

        return default;
    }

#if TOOLS
    private T Add { set; get; }

    public override bool _PropertyCanRevert(StringName pProperty) {
        return _Data.TryGetValue(pProperty.ToString(), out float _);
    }

    public override Variant _PropertyGetRevert(StringName pProperty) {
        if (_Data.TryGetValue(pProperty.ToString(), out float _)) {
            return Variant.From(Mathf.Inf); // Default revert to "impossible" value of Decimal 
        }

        return default;
    }
#endif
}