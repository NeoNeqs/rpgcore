using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;

namespace rpgcore;

[Tool]
public partial class Composable<[MustBeVariant] TComponentBase> : Resource where TComponentBase : Component {
    
      protected internal Godot.Collections.Dictionary<string, TComponentBase?> Components {
        set {
            Action emitChangedAction = EmitChanged;
            Callable emitChangedCallable = Callable.From(emitChangedAction);

            foreach ((_, TComponentBase? component) in field) {
                if (component is not null && component.IsConnected(Resource.SignalName.Changed, emitChangedCallable)) {
                    component.Changed -= emitChangedAction;
                }
            }

            foreach ((_, TComponentBase? component) in value) {
                if (component is not null && !component.IsConnected(Resource.SignalName.Changed, emitChangedCallable)) {
                    component.Changed += emitChangedAction;
                }
            }

            field = value;
        }
        get;
    } = [];

    private const string ComponentsExportName = "components";
    
    public override Array<Dictionary> _GetPropertyList() {
        return [
            // This is what the Inspector sees
            ExportUtils.ExportResourceArrayEditor<TComponentBase>(ComponentsExportName),
            // This is what is stored in a file
            ExportUtils.ExportResourceDictionaryStorage<TComponentBase>(nameof(Components)),
        ];
    }
    
    public override Variant _Get(StringName pProperty) {
        return pProperty == ComponentsExportName ? Variant.From((Array<TComponentBase?>)Components.Values) : default;
    }

    public override bool _Set(StringName pProperty, Variant pValue) {
        if (pProperty != ComponentsExportName) {
            return false;
        }

        Godot.Collections.Dictionary<string, TComponentBase?> newComponents = new();

        var incomingComponents = pValue.As<Array<Variant>>();

        foreach (TComponentBase? component in incomingComponents) {
            string componentKey = component?.GetType().FullName ?? "null";

            newComponents[componentKey] = Components.GetValueOrDefault(componentKey, component);
        }

        Components = newComponents;
        return true;
    }


    public T? GetComponent<T>() where T : TComponentBase {
        if (Components.TryGetValue(typeof(T).FullName!, out TComponentBase? c)) {
            return c as T;
        }

        return null;
    }
    
}