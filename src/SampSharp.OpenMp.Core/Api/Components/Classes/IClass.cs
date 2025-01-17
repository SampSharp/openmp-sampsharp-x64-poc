﻿namespace SampSharp.OpenMp.Core.Api;

[OpenMpApi(typeof(IExtensible), typeof(IIDProvider))]
public readonly partial struct IClass
{
    public partial ref PlayerClass GetClass();
    public partial void SetClass(ref PlayerClass data);
}