using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Collections;
using Type = Godot.Variant.Type;
using Hint = Godot.PropertyHint;
using Usage = Godot.PropertyUsageFlags;

namespace rpgcore;

public static class ExportUtils {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Dictionary Export(string pName, Type pType, Hint pHint, string pHintString, Usage pUsageFlags) {
        return new Dictionary {
            ["name"] = pName,
            ["type"] = (long)pType,
            ["hint"] = (long)pHint,
            ["hint_string"] = pHintString,
            ["usage"] = (long)pUsageFlags
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Dictionary ExportEditor(string pName, Type pType, Hint pHint, string pHintString) {
        return Export(pName, pType, pHint, pHintString, Usage.Editor);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Dictionary ExportEditor(string pName, Type pType) {
        return Export(pName, pType, Hint.None, string.Empty, Usage.Editor);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Dictionary ExportStorage(string pName, Type pType) {
        return Export(pName, pType, Hint.None, string.Empty, Usage.Storage | Usage.ScriptVariable);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Dictionary ExportResourceArrayEditor<TResource>(string pName) where TResource : Resource {
        return ExportEditor(
            pName,
            Type.Array,
            Hint.TypeString,
            $"{Type.Object:D}/{Hint.ResourceType:D}:{typeof(TResource).Name}"
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Dictionary ExportResourceDictionaryStorage<TResource>(string pName) where TResource : Resource {
        return Export(pName, Type.Dictionary, Hint.None, string.Empty,
            Usage.Storage | Usage.AlwaysDuplicate);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Dictionary ExportEnumEditor<TEnum>(string pName) where TEnum : struct, Enum {
        return ExportEditor(pName, Type.Int, Hint.Enum, Utils.EnumToHintString<TEnum>());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Dictionary ExportProperty(string pName, Type pType, Hint pHint, string pHintString) {
        return Export(pName, pType, pHint, pHintString, Usage.ScriptVariable | Usage.Default);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Dictionary ExportProperty(string pName, Type pType) {
        return Export(pName, pType, Hint.None, string.Empty, Usage.ScriptVariable | Usage.Default);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Dictionary ExportReadonlyProperty(string pName, Type pType) {
        return Export(pName, pType, Hint.None, string.Empty, Usage.ScriptVariable | Usage.Default | Usage.ReadOnly);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Dictionary ExportEnumProperty<TEnum>(string pName) where TEnum : struct, Enum {
        return ExportProperty(pName, Type.Int, Hint.Enum, Utils.EnumToHintString<TEnum>());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Dictionary ExportFlagsProperty<TEnum>(string pName) where TEnum : struct, Enum {
        return ExportProperty(pName, Type.Int, Hint.Flags, Utils.EnumToHintString<TEnum>());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Dictionary ExportIntegerProperty(string pName, Hint pHint, string pHintString) {
        return ExportProperty(pName, Type.Int, pHint, pHintString);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Dictionary ExportPositiveIntegerProperty<TInt>(string pName)
        where TInt : struct, IBinaryInteger<TInt>, IMinMaxValue<TInt> {
        return ExportProperty(pName, Type.Int, Hint.Range, $"0,{TInt.MaxValue},1");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Dictionary ExportPositiveIntegerProperty(string pName, int pMin, int pMax, int pStep) {
        return ExportProperty(pName, Type.Int, Hint.Range, $"{pMin},{pMax},{pStep}");
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Dictionary ExportFloatProperty(string pName, Hint pHint, string pHintString) {
        return ExportProperty(pName, Type.Float, pHint, pHintString);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Dictionary ExportPositiveFloatProperty(string pName, uint pMaxValue, float pStep) {
        return ExportProperty(pName, Type.Float, Hint.Range, $"0,{pMaxValue},{pStep}");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Dictionary ExportResourceProperty<TResource>(string pName) where TResource : Resource {
        return ExportProperty(pName, Type.Object, Hint.ResourceType, typeof(TResource).Name);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Dictionary ExportArrayEnumSuggestionProperty(string pName, Type pType, string pHint) {
        return ExportProperty(pName, Type.Array, Hint.TypeString, $"{pType:D}/{Hint.EnumSuggestion:D}:{pHint}");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Dictionary ExportResourceArrayProperty(string pName, string pElementName) {
        return ExportProperty(
            pName,
            Type.Array,
            Hint.TypeString,
            $"{Type.Object:D}/{Hint.ResourceType:D}:{pElementName}"
        );
    }
}