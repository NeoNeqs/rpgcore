using System;
using System.Linq;
using System.Reflection;
using Godot;

namespace rpgcore.gizmo.effect;

[GlobalClass, Tool]
public partial class PropertyEffect : Effect {
    [Signal]
    public delegate void AEventHandler();
    
    [Signal]
    public delegate void BEventHandler();

    public PropertyEffect() {
        GD.Print(string.Join(",", PropertyEffect.GetGodotSignalList().Select(mi => mi.Name)));
    }
    // public enum Mode {
    //     Add = 0,
    //     Multiply = 1,
    //     Set = 2
    // }
    //
    // public string Component { get; set; } = "";
    // public string Property { get; set; } = "";
    // public Mode Operation { get; set; } = Mode.Set;
    // public Variant Value { get; set; }
    //
    // public override void Execute(Gizmo source) {
    //     Component? comp = source.GetComponent(Component);
    //     if (comp == null) return;
    //
    //     switch (Operation) {
    //         case Mode.Set:
    //             comp.Set(Property, Value);
    //             break;
    //         case Mode.Add:
    //             double currentAdd = comp.Get(Property).AsDouble();
    //             comp.Set(Property, currentAdd + Value.AsDouble());
    //             break;
    //         case Mode.Multiply:
    //             double currentMult = comp.Get(Property).AsDouble();
    //             comp.Set(Property, currentMult * Value.AsDouble());
    //             break;
    //         default:
    //             throw new NotImplementedException();
    //     }
    //
    //     comp.EmitChanged();
    // }
    //
    // public override Array<Dictionary> _GetPropertyList() {
    //     var properties = new Array<Dictionary>();
    //
    //     string[] componentTypes = Assembly.GetExecutingAssembly().GetTypes()
    //         .Where(t => t.IsSubclassOf(typeof(Component)) && !t.IsAbstract && t.Name != "UseComponent")
    //         .Select(t => t.Name)
    //         .OrderBy(n => n)
    //         .ToArray();
    //
    //     properties.Add(WithRefresh(ExportUtils.ExportProperty(
    //             nameof(Component),
    //             Variant.Type.String,
    //             PropertyHint.Enum,
    //             string.Join(",", componentTypes)
    //         )
    //     ));
    //
    //     var propertyHintList = "";
    //     var valueType = Variant.Type.Nil;
    //     var valueHint = PropertyHint.None;
    //     var valueHintString = "";
    //     var isNumeric = false;
    //
    //     if (!string.IsNullOrEmpty(Component)) {
    //         Type? selectedType = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(t => t.Name == Component);
    //         if (selectedType != null && Activator.CreateInstance(selectedType) is GodotObject tempInstance) {
    //             Dictionary[] godotProps = tempInstance.GetPropertyList()
    //                 .Where(p => (p["usage"].AsInt32() & (int)PropertyUsageFlags.ScriptVariable) != 0)
    //                 .ToArray();
    //
    //             string[] validNames = godotProps.Select(p => p["name"].AsString()).OrderBy(n => n).ToArray();
    //             propertyHintList = string.Join(",", validNames);
    //
    //             if (!string.IsNullOrEmpty(Property) && !validNames.Contains(Property)) {
    //                 Property = "";
    //             }
    //
    //             Dictionary? activeProp = godotProps.FirstOrDefault(p => p["name"].AsString() == Property);
    //             if (activeProp != null) {
    //                 valueType = (Variant.Type)activeProp["type"].AsInt32();
    //                 valueHint = (PropertyHint)activeProp["hint"].AsInt32();
    //                 valueHintString = activeProp["hint_string"].AsString();
    //
    //                 // Check if numeric and NOT an enum
    //                 isNumeric = (valueType == Variant.Type.Int || valueType == Variant.Type.Float) &&
    //                             valueHint != PropertyHint.Enum;
    //
    //                 // Fallback for Enums with missing hint strings
    //                 if (valueHint == PropertyHint.Enum && string.IsNullOrEmpty(valueHintString)) {
    //                     MemberInfo? memberInfo = (MemberInfo?)selectedType.GetProperty(Property) ??
    //                                              selectedType.GetField(Property);
    //                     Type? enumType = memberInfo switch {
    //                         PropertyInfo pi => pi.PropertyType,
    //                         FieldInfo fi => fi.FieldType,
    //                         _ => null
    //                     };
    //                     if (enumType is { IsEnum: true }) {
    //                         valueHintString = string.Join(",", Enum.GetNames(enumType));
    //                     }
    //                 }
    //
    //                 // If doing math, override the type of Value to Float to allow decimals (e.g., 1.2x)
    //                 if (isNumeric && Operation != Mode.Set) {
    //                     valueType = Variant.Type.Float;
    //                     valueHint = PropertyHint.None;
    //                     valueHintString = "";
    //                 }
    //             }
    //         }
    //     }
    //
    //     properties.Add(WithRefresh(ExportUtils.ExportProperty(nameof(Property), Variant.Type.String, PropertyHint.Enum,
    //         propertyHintList)));
    //
    //     // 3. Operation Dropdown (Numeric gets Add/Mult/Set, others get only Set)
    //     string opHint = isNumeric ? "Add:0,Multiply:1,Set:2" : "Set:2";
    //     properties.Add(WithRefresh(ExportUtils.ExportProperty(nameof(Operation), Variant.Type.Int, PropertyHint.Enum,
    //         opHint)));
    //
    //     // 4. Final Value Field
    //     properties.Add(ExportUtils.ExportProperty(nameof(Value), valueType, valueHint, valueHintString));
    //
    //     return properties;
    // }
    //
    // private static Dictionary WithRefresh(Dictionary prop) {
    //     prop["usage"] = (long)(PropertyUsageFlags.Default | PropertyUsageFlags.UpdateAllIfModified);
    //     return prop;
    // }
    //
    // public override bool _Set(StringName property, Variant value) {
    //     if (property == (StringName)nameof(Component)) {
    //         Component = value.AsString();
    //         Property = "";
    //         Operation = Mode.Set;
    //         Value = default;
    //         NotifyPropertyListChanged();
    //         return true;
    //     }
    //
    //     if (property == (StringName)nameof(Property)) {
    //         Property = value.AsString();
    //         NotifyPropertyListChanged();
    //         return true;
    //     }
    //
    //     if (property == (StringName)nameof(Operation)) {
    //         Operation = (Mode)value.AsInt32();
    //         NotifyPropertyListChanged();
    //         return true;
    //     }
    //
    //     if (property != (StringName)nameof(Value)) return false;
    //     Value = value;
    //     return true;
    // }
    //
    // public override Variant _Get(StringName property) {
    //     if (property == (StringName)nameof(Component)) return Component;
    //     if (property == (StringName)nameof(Property)) return Property;
    //     if (property == (StringName)nameof(Operation)) return (int)Operation;
    //     return property == (StringName)nameof(Value) ? Value : default;
    // }
}