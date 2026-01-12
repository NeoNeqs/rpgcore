using System;
using System.Collections.Generic;
using CombatSystem;
using Godot;
using Godot.Collections;

namespace rpgcore.gizmo;

// IMPORTANT: Careful with adding float properties here. See `_Set()` definition down below.

[Tool, GlobalClass]
public partial class Stats : Resource {
    [Signal]
    public delegate void StatChangedEventHandler(Stat pStat, float pOldValue, float pNewValue);

    private Godot.Collections.Dictionary<string, float> _stats = new();

    // ReSharper disable once InconsistentNaming
    private Godot.Collections.Dictionary<string, float> _Stats {
        set {
            _stats = value;
#if TOOLS
            NotifyPropertyListChanged();
#endif
        }
        get => _stats;
    }

    public float GetStat(Stat pStat, float pDefault = 0.0f) {
        var key = pStat.ToString();
        _stats.TryAdd(key, pDefault);

        return _stats[key];
    }

    public void SetStat(Stat pStat, float pNewValue) {
        var key = pStat.ToString();
        _stats.TryGetValue(key, out float oldValue);
        _stats[key] = pNewValue;

        EmitSignalStatChanged(pStat, oldValue, pNewValue);
    }

    public void IncreaseStat(Stat pStat, float pIncrement) {
        var key = pStat.ToString();
        _stats.TryGetValue(key, out float oldValue);

        float newValue = oldValue + pIncrement;
        _stats[key] = newValue;

        EmitSignalStatChanged(pStat, oldValue, newValue);
    }

    public static Stat[] GetStatValues() {
        return Enum.GetValues<Stat>();
    }

    public override Array<Dictionary> _GetPropertyList() {
        Array<Dictionary> propertyList = [
#if TOOLS
            // Helper properties for the Inspector that allow adding / removing stats.
            ExportUtils.ExportEnumEditor<Stat>(nameof(AddStat)),
#endif
            // Actual storage of stats:
            ExportUtils.ExportStorage(nameof(_Stats), Variant.Type.Dictionary),
        ];
        // Helper properties. Those basically display the contents of the dictionaries as normal properties. Only used in editor.
#if TOOLS
        foreach ((string statName, float _) in _stats) {
            propertyList.Add(
                ExportUtils.ExportEditor(statName, Variant.Type.Float)
            );
        }
#endif
        return propertyList;
    }

    /// Redirects storage of float properties to _stats.
    public override bool _Set(StringName pProperty, Variant pValue) {
        string propertyName = pProperty.ToString();

        //IMPORTANT: ALL Decimal properties of this class are handled through here!

        // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
        // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
        // Only care about floats.
        switch (pValue.VariantType) {
            case Variant.Type.Float: {
                float floatValue = pValue.As<float>();
#if TOOLS
                if (float.IsPositiveInfinity(floatValue)) {
                    _stats.Remove(propertyName);
                    NotifyPropertyListChanged();
                    return true;
                }
#endif
                _stats[propertyName] = floatValue;
                break;
            }
        }

        return false;
    }

    public override Variant _Get(StringName pProperty) {
        if (_stats.TryGetValue(pProperty.ToString(), out float value)) {
            return Variant.From(value);
        }

        return default;
    }

#if TOOLS
    private Stat AddStat {
        set {
            if (_stats.TryAdd(value.ToString(), 1)) {
                NotifyPropertyListChanged();
            }
        }
    }

    public override bool _PropertyCanRevert(StringName pProperty) {
        return _stats.TryGetValue(pProperty.ToString(), out float _);
    }

    public override Variant _PropertyGetRevert(StringName pProperty) {
        if (_stats.TryGetValue(pProperty.ToString(), out float _)) {
            return Variant.From(Mathf.Inf); // Default revert (impossible) value of Decimal Stats
        }

        return default;
    }
#endif
}