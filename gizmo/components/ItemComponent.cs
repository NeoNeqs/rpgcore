using Godot;

namespace rpgcore.gizmo.components;

[GlobalClass, Tool]
public partial class ItemComponent : GizmoComponent {
    [Export, Notify] public ulong SellPrice { private set; get; }
    [Export, Notify] public ItemState State { private set; get; }
    [Export, Notify] public ulong RequiredLevel { private set; get; }
    [Export, Notify] public Rarity Rarity { private set; get; }
    // This means inventory can only hold gizmos with ItemComponent 
    [Export, Notify] public int MaxStack { private set; get; }
}