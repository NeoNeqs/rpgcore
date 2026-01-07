using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Godot;
using Godot.Collections;

namespace rpgcore.component;

[Tool]
public abstract partial class ComponentSystem<[MustBeVariant] TComponentBase, TSelf> : Resource
    where TComponentBase : ComponentBase
    where TSelf : ComponentSystem<TComponentBase, TSelf>, new() {
    protected internal Godot.Collections.Dictionary<string, ComponentBase?> Components { set; get; } = [];

    private Godot.Collections.Dictionary<string, ComponentState?> States {
        set {
            if (Engine.IsEditorHint()) {
                States.Clear();
                foreach ((var key, ComponentBase? component) in Components) {
                    if (component is null) continue;
                    field[key] = component.GetDefaultState();
                    component.Changed += () => {
                        if (Components.TryGetValue(key, out ComponentBase? value1)) {
                            States[key] = value1?.GetDefaultState();
                        }
                    };
                }
            } else {
                field = value;
            }
        }
        get;
    } = [];

    protected const string ComponentsExportName = "components";

    public TComponent? GetComponent<[MustBeVariant] TComponent>() where TComponent : TComponentBase {
        if (Components.TryGetValue(typeof(TComponent).Name, out ComponentBase? value)) {
            return (TComponent)value!;
        }

        return null;
    }

    public TState? GetComponentState<[MustBeVariant] TComponent, [MustBeVariant] TState>()
        where TComponent : ComponentBase where TState : ComponentState {
        if (States.TryGetValue(typeof(TComponent).Name, out ComponentState? value)) {
            return (TState)value!;
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

    
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public new Resource Duplicate(bool deep = false) {
        throw new InvalidOperationException(
            $"{GetType().Name}.Duplicate() must not be used. " +
            $"Use {GetType().Name}.Clone() instead, which correctly handles duplication."
        );
    }

    public override Array<Dictionary> _GetPropertyList() {
        return [
            // This is what the Inspector sees
            ExportUtils.ExportResourceArrayEditor<TComponentBase>(ComponentsExportName),
            // This is what is stored in a file
            ExportUtils.ExportStorage(nameof(Components), Variant.Type.Dictionary),
            ExportUtils.ExportStorage(nameof(States), Variant.Type.Dictionary)
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

        Godot.Collections.Dictionary<string, ComponentBase?> newComponents = new();
        Godot.Collections.Dictionary<string, ComponentState?> newStates = new();

        var currentComponents = pValue.As<Array<Variant>>();

        foreach (TComponentBase? component in currentComponents) {
            var componentKey = component?.GetType().Name ?? "null";
            // Attempt to extract old value from _components, otherwise set the new one
            newComponents[componentKey] = Components.GetValueOrDefault(componentKey, component);

            ComponentState? defaultState = component?.GetDefaultState();

            if (defaultState != null) {
                newStates[componentKey] = defaultState;
            }
        }

        Components = newComponents;
        States = newStates;
        return true;
    }
}