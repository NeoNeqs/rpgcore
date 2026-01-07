using Godot;

namespace rpgcore.component;

// TODO: Convert this to an interface
// TODO: Then try to make this generic ComponentBase<TSelf> so that GetState can be generic and simplified 
[GlobalClass, Tool]
public abstract partial class ComponentBase : Resource {
    public abstract void OnAction(Gizmo actionSource);

    public abstract ComponentState? GetDefaultState();
    public abstract ToolState? GetState(Gizmo gizmo);
}