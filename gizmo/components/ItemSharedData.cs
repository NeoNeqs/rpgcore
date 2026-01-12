using Godot;

namespace rpgcore.gizmo.components;

public partial class ItemSharedData : Resource{
    [Export, Notify] public ulong SellPrice { private set; get; }
    [Export, Notify] public ItemState State { private set; get; }
    [Export, Notify] public ulong RequiredLevel { private set; get; }
}