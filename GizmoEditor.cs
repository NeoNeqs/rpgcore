using System;
using System.Diagnostics.CodeAnalysis;
using Godot;
using Godot.Collections;
using rpgcore.component;

namespace rpgcore;

#if TOOLS
public partial class Gizmo {
    public EditorGizmoTemplate GizmoTemplate {
        get;
        private set {
            if (field == value || !Engine.IsEditorHint())
                return;

            var isTemplateApplied = ApplyTemplate(value);
            if (isTemplateApplied) {
                field = value;
            }
        }
    }

    private bool ApplyTemplate(EditorGizmoTemplate template) {
        var attr = template.GetAttribute<EditorGizmoDefaultsAttribute>();
        if (attr is null) {
            GD.PushError(
                $"{nameof(EditorGizmoTemplate)}.{template:G} is missing a {nameof(EditorGizmoDefaultsAttribute)}.");
            return false;
        }

        var result = ApplyComponents(attr);
        ApplyId(attr);

        return result;
    }

    private bool ApplyComponents(EditorGizmoDefaultsAttribute attr) {
        if (Components.Count != 0) {
            GD.PushWarning(
                $"Template was not applied to '{ResourcePath}' since Components are not empty.");
            return false;
        }

        var components = new Array<ComponentBase>();

        foreach (Type type in attr.DefaultComponents) {
            if (!typeof(ComponentBase).IsAssignableFrom(type)) {
                GD.PushError(
                    $"{type.FullName} does not derive from {nameof(ComponentBase)}.");
                return false;
            }

            components.Add((ComponentBase)Activator.CreateInstance(type)!);
        }

        Set(ComponentsExportName, Variant.From(components));

        return true;
    }

    private void ApplyId(EditorGizmoDefaultsAttribute attr) {
        if (!Id.IsEmpty)
            return;

        Id = new StringName(attr.DefaultIdPrefix);
    }

    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public enum EditorGizmoTemplate {
        [EditorGizmoDefaults("item:", typeof(DisplayComponent))]
        Item,

        [EditorGizmoDefaults("spell:", typeof(DisplayComponent))]
        Spell
    }

    [AttributeUsage(AttributeTargets.Field)]
    public sealed class EditorGizmoDefaultsAttribute(
        string defaultIdPrefix,
        params Type[] defaultComponents)
        : Attribute {
        public string DefaultIdPrefix { get; } = defaultIdPrefix;
        public Type[] DefaultComponents { get; } = defaultComponents;
    }

    public override Array<Dictionary> _GetPropertyList() {
        var baseProperties = base._GetPropertyList();

        baseProperties.AddRange([
            ExportUtils.Export("Editor Only", Variant.Type.Nil, PropertyHint.None, string.Empty,
                PropertyUsageFlags.Group),
            ExportUtils.ExportEnumEditor<EditorGizmoTemplate>(nameof(GizmoTemplate))
        ]);

        return baseProperties;
    }
}
#endif