﻿using System.Numerics;

namespace SashManaged.OpenMp;

[OpenMpApi(typeof(IBaseObject))]
public readonly partial struct IPlayerObject
{
    public partial void AttachToObject(IPlayerObject objekt, Vector3 offset, Vector3 rotation);
    public partial void AttachToPlayer(IPlayer player, Vector3 offset, Vector3 rotation);
}