﻿namespace SashManaged.OpenMp;

[OpenMpApi(typeof(IExtension))]
public readonly partial struct IPlayerClassData
{
    public static UID ExtensionId => new(0x185655ded843788b);
}