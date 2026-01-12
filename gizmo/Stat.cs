using System;

namespace CombatSystem;

public enum Stat : byte {
    None = 0,
    Strength = 1,
    Dexterity = 2,
    Intelligence = 3,
    Wisdom = 4,
    Vigor = 5,

    Armor = 6,
    ArmorPenetration = 7,
    NatureResistance = 8,
    NaturePenetration = 9,

    DarkResistance = 10,
    DarkPenetration = 11,
    LightResistance = 12,
    LightPenetration = 13,

    FireResistance = 14,
    FirePenetration = 15,
    AirResistance = 16,
    AirPenetration = 17,
    WaterResistance = 18,
    WaterPenetration = 19,
    EarthResistance = 20,
    EarthPenetration = 21
}