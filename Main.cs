using System.Collections.Generic;
using Godot;
using rpgcore.gizmo;
using rpgcore.gizmo.components;
using rpgcore.gizmo.effect;
using rpgcore.gizmo.enums;

public partial class Main : Node {
    public override void _Ready() {
        // var a = new PropertyEffect();
        var g = GD.Load<Gizmo>("res://resources/1hand.tres");
        var weapon = g.GetComponent<WeaponComponent>();
        
        // This applies the random rolls defined by the pool and count
        
        GD.Print($"Total unique stats: {weapon?.Attributes?.Iter()?.Count}");
        foreach ((string key, float value) in weapon?.Attributes?.Iter()!) {
            GD.Print($"{key}: {value}");
        }
    }
}