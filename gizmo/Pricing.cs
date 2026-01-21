using System;
using System.Collections.Generic;
using rpgcore.gizmo.enums;
using Attribute = rpgcore.gizmo.enums.Attribute;

namespace rpgcore.gizmo;

public static class Pricing {
    private static readonly Dictionary<Attribute, float> BasePrices = new() {
        { Attribute.Strength, 2.0f },
        { Attribute.Dexterity, 2.0f },
        { Attribute.Intelligence, 2.0f },
        { Attribute.Wisdom, 1.5f },
        { Attribute.Vigor, 2.5f },
    };

    private static readonly Dictionary<WeaponClass, float> WeaponPrices = new() {
        { WeaponClass.Sword, 1.0f },
        { WeaponClass.Dagger, 1.0f },
        { WeaponClass.Axe, 1.0f },
        { WeaponClass.Staff, 1.0f },
        { WeaponClass.Shield, 1.0f },
        { WeaponClass.Bow, 1.0f },
    };

    private static readonly Dictionary<ArmorType, float> ArmorTypePrices = new() {
        { ArmorType.Light, 0.2f },
        { ArmorType.Medium, 0.1f },
        { ArmorType.Heavy, 0.0f },
    };

    private static readonly Dictionary<ArmorSlot, float> ArmorSlotPrices = new() {
        { ArmorSlot.Body, 0.0f },
        { ArmorSlot.Head, 0.1f },
        { ArmorSlot.Legs, 0.2f },
        { ArmorSlot.Feet, 0.3f },
    };


#if TOOLS
    static Pricing() {
        // Validation: Ensure every enum value has a defined price.
        foreach (Attribute stat in Enum.GetValues<Attribute>()) {
            if (!BasePrices.ContainsKey(stat)) {
                throw new Exception(
                    $"Missing price definition for {nameof(Attribute)}: {stat}. Please add it to {nameof(Pricing)} class.");
            }
        }

        foreach (WeaponClass weaponClass in Enum.GetValues<WeaponClass>()) {
            if (!WeaponPrices.ContainsKey(weaponClass)) {
                throw new Exception(
                    $"Missing price definition for {nameof(WeaponClass)}: {weaponClass}. Please add it to {nameof(Pricing)} class.");
            }
        }
    }
#endif
    public static float GetBasePrice(Attribute attribute) => BasePrices[attribute];
    public static float GetWeaponPrice(WeaponClass weaponClass) => WeaponPrices[weaponClass];

    public static float GetArmorPrice(ArmorType armorType, ArmorSlot armorSlot) =>
        ArmorSlotPrices[armorSlot] + ArmorTypePrices[armorType];
}