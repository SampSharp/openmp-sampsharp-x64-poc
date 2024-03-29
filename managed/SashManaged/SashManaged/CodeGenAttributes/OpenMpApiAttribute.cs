﻿namespace SashManaged;

[AttributeUsage(AttributeTargets.Struct)]
public class OpenMpApiAttribute(params Type[] implements) : Attribute
{
    public Type[] Implements { get; } = implements;
}

[AttributeUsage(AttributeTargets.Struct)]
public class OpenMpApi2Attribute(params Type[] implements) : Attribute
{
    public Type[] Implements { get; } = implements;
    public string Library { get; set; } = "SampSharp";
    public string? NativeTypeName { get; set; }
}