using System;
using System.Reflection;

namespace rpgcore;

public static class Utils {
    public static string EnumToHintString<TEnum>() where TEnum : struct, Enum {
        return string.Join(',', Enum.GetNames<TEnum>());
    }
}