using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;


namespace rpgcore.gizmo;

[Tool, GlobalClass]
public partial class Gizmo : Composable<GizmoComponent> {
    public override void _Notification(int what) {
    }
    // protected internal Godot.Collections.Dictionary<string, GizmoComponent?> Components {
    //     set {
    //         Action emitChangedAction = EmitChanged;
    //         Callable emitChangedCallable = Callable.From(emitChangedAction);
    //
    //         foreach ((_, GizmoComponent? component) in field) {
    //             if (component is not null && component.IsConnected(Resource.SignalName.Changed, emitChangedCallable)) {
    //                 component.Changed -= emitChangedAction;
    //             }
    //         }
    //
    //         foreach ((_, GizmoComponent? component) in value) {
    //             if (component is not null && !component.IsConnected(Resource.SignalName.Changed, emitChangedCallable)) {
    //                 component.Changed += emitChangedAction;
    //             }
    //         }
    //
    //         field = value;
    //     }
    //     get;
    // } = [];
    //
    // private const string ComponentsExportName = "components";
    //
    // public override Array<Dictionary> _GetPropertyList() {
    //     return [
    //         // This is what the Inspector sees
    //         ExportUtils.ExportResourceArrayEditor<GizmoComponent>(ComponentsExportName),
    //         // This is what is stored in a file
    //         ExportUtils.ExportResourceDictionaryStorage<GizmoComponent>(nameof(Components)),
    //     ];
    // }
    //
    // public override Variant _Get(StringName pProperty) {
    //     return pProperty == ComponentsExportName ? Variant.From((Array<GizmoComponent?>)Components.Values) : default;
    // }
    //
    // public override bool _Set(StringName pProperty, Variant pValue) {
    //     if (pProperty != ComponentsExportName) {
    //         return false;
    //     }
    //
    //     Godot.Collections.Dictionary<string, GizmoComponent?> newComponents = new();
    //
    //     var incomingComponents = pValue.As<Array<Variant>>();
    //
    //     foreach (GizmoComponent? component in incomingComponents) {
    //         string componentKey = component?.GetType().FullName ?? "null";
    //
    //         newComponents[componentKey] = Components.GetValueOrDefault(componentKey, component);
    //     }
    //
    //     Components = newComponents;
    //     return true;
    // }
    //
    //
    // public T? GetComponent<T>() where T : GizmoComponent {
    //     if (Components.TryGetValue(typeof(T).FullName!, out GizmoComponent? c)) {
    //         return c as T;
    //     }
    //
    //     return null;
    // }
}