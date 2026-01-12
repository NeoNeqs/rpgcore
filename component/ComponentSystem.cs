using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;

namespace rpgcore.component;

[GlobalClass, Tool]
public abstract partial class ComponentSystemBase : Resource {
    private protected ComponentSystemBase() { }

    protected internal Godot.Collections.Dictionary<string, Component?> Components {
        set {
            Action emitChangedAction = EmitChanged;
            Callable emitChangedCallable = Callable.From(emitChangedAction);

            foreach ((_, Component? component) in field) {
                if (component is not null && component.IsConnected(Resource.SignalName.Changed, emitChangedCallable)) {
                    component.Changed -= emitChangedAction;
                }
            }

            foreach ((_, Component? component) in value) {
                if (component is not null && !component.IsConnected(Resource.SignalName.Changed, emitChangedCallable)) {
                    component.Changed += emitChangedAction;
                }
            }

            field = value;
        }
        get;
    } = [];
}

[Tool]
public abstract partial class ComponentSystem<TComponentBase, TSelf> : ComponentSystemBase
    where TComponentBase : Component
    where TSelf : ComponentSystem<TComponentBase, TSelf>, new() {
    public StringName Id { private set; get; } = new();

    private protected ComponentSystem() { }

    protected internal const string ComponentsExportName = "components";

    public TComponent? GetComponent<TComponent>() where TComponent : TComponentBase {
        if (Components.TryGetValue(typeof(TComponent).Name, out Component? value)) {
            return (TComponent)value!;
        }

        return null;
    }

    public new TSelf Duplicate(bool deep = true) {
        return (TSelf)base.Duplicate(true);
    }

    public override Array<Dictionary> _GetPropertyList() {
        return [
            ExportUtils.ExportProperty(nameof(Id), Variant.Type.StringName),
            // This is what the Inspector sees
            ExportUtils.ExportResourceArrayEditor<TComponentBase>(ComponentsExportName),
            // This is what is stored in a file
            ExportUtils.ExportResourceDictionaryStorage<TComponentBase>(nameof(Components)),
        ];
    }

    public override Variant _Get(StringName pProperty) {
        return pProperty == ComponentsExportName ? Variant.From((Array<Component?>)Components.Values) : default;
    }

    public override bool _Set(StringName pProperty, Variant pValue) {
        if (pProperty != ComponentsExportName) {
            return false;
        }

        Godot.Collections.Dictionary<string, Component?> newComponents = new();

        var incomingComponents = pValue.As<Array<Variant>>();

        foreach (TComponentBase? component in incomingComponents) {
            var componentKey = component?.GetType().Name ?? "null";

            newComponents[componentKey] = Components.GetValueOrDefault(componentKey, component);
        }

        Components = newComponents;
        return true;
    }
}