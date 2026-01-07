using Godot;

namespace rpgcore.component;

[GlobalClass, Tool]
public abstract partial class ComponentBase : Resource {

    public abstract ComponentState? GetDefaultState();
}