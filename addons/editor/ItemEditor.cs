using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Godot;
using rpgcore.component;

namespace rpgcore.addons.editor;

[Tool, SceneTree(root: "children")]
public partial class ItemEditor : Control {
    private GodotObject? _current;
    //

    public override void _EnterTree() {
        _current = EditorInterface.Singleton.GetInspector().GetEditedObject();
        EditorInterface.Singleton.GetInspector().Connect(EditorInspector.SignalName.EditedObjectChanged,
            new Callable(this, nameof(OnObjectEdit)));
    }

    public override void _Ready() {
        Debug.Assert(typeof(ItemEditor).BaseType == typeof(Control),
            "Main Screen plugins must derive directly from Control");

        PopulateItemTree();
    }

    public override void _ExitTree() {
        EditorInterface.Singleton.GetInspector().Disconnect(EditorInspector.SignalName.EditedObjectChanged,
            new Callable(this, nameof(OnObjectEdit)));
    }

    public void OnObjectEdit() {
        GodotObject? newObject = EditorInterface.Singleton.GetInspector().GetEditedObject();
        if (newObject is ComponentSystemBase componentSystemBase) {
            if (!Visible) {
                EditorInterface.Singleton.SetMainScreenEditor("RPGEditor");
            }

            // Prevent Godot's inspector from showing this object
            // if (_current is Resource current) {
            //     EditorInterface.Singleton.CallDeferred("edit_resource", current);
            // }

            Edit(componentSystemBase);
        }

        _current = newObject;
    }

    private void Edit(ComponentSystemBase objectToEdit) {
        children.Container.TopBar.CurrentFile.Text = objectToEdit.ResourcePath;

        foreach (Node child in children.Container.Content.TabContainer.Get().GetChildren()) {
            child.QueueFree();
            children.Container.Content.TabContainer.Get().RemoveChild(child);
        }

        foreach ((_, Component? component) in objectToEdit.Components) {
            if (component == null) continue;

            var control = new Control();
            var end = component.GetType().Name.IndexOf("Component", StringComparison.Ordinal);
            if (end == -1) {
                end = component.GetType().Name.Length;
            }

            control.Name = component.GetType().Name[..end]; 
            children.Container.Content.TabContainer.Get().AddChild(control);
        }
    }

    private void PopulateItemTree() {
        Tree tree = children.Container.Content.VBoxContainer.Tree;
        tree.Clear();

        TreeItem root = tree.CreateItem();
        Type[] componentSystems = GetSubClasses();

        // Map types to their respective TreeItem categories
        Dictionary<Type, TreeItem> categoryMap = componentSystems.ToDictionary(
            type => type,
            type => {
                TreeItem item = tree.CreateItem(root);
                item.SetText(0, type.Name);
                return item;
            }
        );

        TreeItem unknownCategory = tree.CreateItem(root);

        tree.ItemActivated += () => {
            TreeItem selected = tree.GetSelected();
            if (selected.GetParent() != unknownCategory) {
                if (selected.GetText(0).StartsWith("res://")) {
                    Edit((GD.Load<Resource>(selected.GetText(0)) as ComponentSystemBase)!);
                }
            }
        };

        unknownCategory.SetText(0, "Impostor (SUS)");

        var gizmoDir = RPGEditorSettings.GetGizmosDirectory();
        foreach (var path in GetAllFilesRecursive(gizmoDir)) {
            var resource = GD.Load<Resource>(path);

            // Find the first matching system type
            Type? matchingType = componentSystems.FirstOrDefault(t => t.IsInstanceOfType(resource));
            TreeItem parent = matchingType != null ? categoryMap[matchingType] : unknownCategory;

            TreeItem child = parent.CreateChild();
            child.SetText(0, path);
        }
    }

    private static Type[] GetSubClasses() {
        Type baseType = typeof(ComponentSystem<,>);

        Assembly assembly = baseType.Assembly;
        return assembly
            .GetTypes()
            .Where(t =>
                t is { IsClass: true, IsAbstract: false, BaseType.IsGenericType: true } &&
                t.BaseType.GetGenericTypeDefinition() == baseType)
            .ToArray();
    }

    private static IEnumerable<string> GetAllFilesRecursive(string dir) {
        using DirAccess? da = DirAccess.Open(dir);
        if (da == null)
            yield break;

        da.ListDirBegin();
        while (true) {
            var entry = da.GetNext();
            if (string.IsNullOrEmpty(entry))
                break;

            // Skip . and ..
            if (entry is "." or "..")
                continue;

            var fullPath = dir.PathJoin(entry);

            if (da.CurrentIsDir()) {
                foreach (var file in GetAllFilesRecursive(fullPath))
                    yield return file;
            } else {
                yield return fullPath;
            }
        }

        da.ListDirEnd();
    }
}