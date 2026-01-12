using System;

namespace rpgcore.component;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class SharedDataAttribute(Type sharedDataType) : Attribute {
    // ReSharper disable once UnusedMember.Global
    public Type SharedDataType { get; } = sharedDataType;
}