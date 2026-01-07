using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;

namespace rpgcore.component;

[Tool]
public abstract partial class ComponentSystem<[MustBeVariant] TComponent, TSelf> : Resource
    where TComponent : ComponentBase
    where TSelf : ComponentSystem<TComponent, TSelf>, new() {
    protected internal Godot.Collections.Dictionary<string, ComponentBase?> Components { set; get; } = [];

    private Godot.Collections.Dictionary<string, ComponentState?> States { set; get; } = [];

    protected const string ComponentsExportName = "components";

    public T? GetComponent<[MustBeVariant] T>() where T : TComponent {
        if (Components.TryGetValue(typeof(T).Name, out ComponentBase? value)) {
            return (T)value!;
        }

        return null;
    }

    public T? GetComponentState<[MustBeVariant] T>() where T : ComponentState {
        if (States.TryGetValue(typeof(T).Name, out ComponentState? value)) {
            return (T)value!;
        }

        return null;
    }

    public virtual TSelf Clone() {
        var clone = new TSelf {
            // Components are shared on purpose
            Components = Components,
        };

        // Dictionary.duplicate will not work here, cause it will ignore GodotObjects :(
        // Must do this *manually*
        foreach ((var key, ComponentState? state) in States) {
            if (state is not null) {
                clone.States[key] = (ComponentState)state.Duplicate();
            }
        }

        return clone;
    }

    public new Resource Duplicate(bool deep = false) {
        throw new InvalidOperationException(
            $"{GetType().Name}.Duplicate() must not be used. " +
            $"Use {GetType().Name}.Clone() instead, which correctly handles duplication."
        );
    }

    public override Array<Dictionary> _GetPropertyList() {
        return [
            // This is what the Inspector sees
            ExportUtils.ExportResourceArrayEditor<TComponent>(ComponentsExportName),
            // This is what is stored in a file
            ExportUtils.ExportStorage(nameof(Components), Variant.Type.Dictionary),
            ExportUtils.ExportProperty(nameof(States), Variant.Type.Dictionary)
        ];
    }

    public override Variant _Get(StringName pProperty) {
        if (pProperty == ComponentsExportName) {
            return Variant.From((Array<ComponentBase?>)Components.Values);
        }

        return default;
    }

    public override bool _Set(StringName pProperty, Variant pValue) {
        if (pProperty != ComponentsExportName) {
            return false;
        }
        
        GD.Print("TRIGGER!!!");

        Godot.Collections.Dictionary<string, ComponentBase?> newComponents = new();
        Godot.Collections.Dictionary<string, ComponentState?> newStates = new();

        var currentComponents = pValue.As<Array<Variant>>();

        foreach (TComponent? component in currentComponents) {
            var componentKey = component?.GetType().Name ?? "null";
            // Attempt to extract old value from _components, otherwise set the new one
            newComponents[componentKey] = Components.GetValueOrDefault(componentKey, component);

            if (component is not null) {
                ComponentState? defaultState = component?.GetDefaultState();
                var stateKey = component?.GetDefaultState()?.GetType().Name ?? string.Empty;
                // ComponentState? newState = States.GetValueOrDefault(stateKey, defaultState);
                // if (newState is not null && stateKey.Length != 0) {
                //     newStates[stateKey] = newState;
                // }

                if (defaultState is not null && stateKey.Length != 0) {
                    newStates[stateKey] = defaultState;
                }
            }
        }

        Components = newComponents;
        States = newStates;
        return true;
    }
}