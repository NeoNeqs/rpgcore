using System;

namespace rpgcore.gizmo.enums;

[Flags]
public enum WeaponSlot {
    Main = 1 << 0,
    Off = 1 << 1,
    Secondary = 1 << 2
}