#if TOOLS
using Godot;

namespace rpgcore.addons.editor;

[Tool]
public partial class Main : EditorPlugin {
    private Control? _itemEditorScene;

    public override void _EnablePlugin() {
        RPGEditorSettings.CreateGizmosDirectory("res://resources/");
    }

    public override void _EnterTree() {
        _itemEditorScene = GD
            .Load<PackedScene?>("res://addons/editor/ItemEditor.tscn")
            ?.Instantiate<Control>();
        EditorInterface.Singleton.GetEditorMainScreen().AddChild(_itemEditorScene);
        _MakeVisible(false);
    } 

    public override void _ExitTree() {
        _itemEditorScene?.QueueFree();
    }

    public override void _MakeVisible(bool visible) {
        _itemEditorScene?.Visible = visible;
    }

    public override Texture2D _GetPluginIcon() {
        return EditorInterface.Singleton.GetEditorTheme().GetIcon("Node", "EditorIcons");
    }

    public override string _GetPluginName() {
        return "RPGEditor";
    }

    public override bool _HasMainScreen() {
        return true;
    }
}
#endif