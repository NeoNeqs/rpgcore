using Godot;
using Godot.Collections;

namespace rpgcore.addons.editor;

public static class RPGEditorSettings {
    private const string GizmosDirectory = "RPGEditor/paths/gizmos_directory";
    
    static RPGEditorSettings() {
        AddDirSetting(GizmosDirectory, "res://resources/");
    }

    public static string GetGizmosDirectory() {
        return ProjectSettings.GetSetting(GizmosDirectory).AsString();
    }

    public static void CreateGizmosDirectory(string defaultValue) {
        AddDirSetting(GizmosDirectory, defaultValue);
    } 
    
    private static void AddDirSetting(string name, string defaultValue) {
        if (!ProjectSettings.HasSetting(name)) {
            ProjectSettings.SetSetting(name, defaultValue);
        }
        
        ProjectSettings.SetInitialValue(name, "");
        ProjectSettings.AddPropertyInfo(new Dictionary() {
            ["name"] = name,
            ["type"] = (long)Variant.Type.String,
            ["hint"] = (long)PropertyHint.Dir,
        });
    }
}