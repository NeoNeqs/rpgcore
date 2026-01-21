using Godot;

namespace rpgcore.entity;

public partial class Entity : Resource {
    [Export] public string Name { get; set; } = "";
}