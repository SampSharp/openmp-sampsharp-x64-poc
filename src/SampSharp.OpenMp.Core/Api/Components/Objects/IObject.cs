﻿using System.Numerics;

namespace SampSharp.OpenMp.Core.Api;

[OpenMpApi(typeof(IBaseObject))]
public readonly partial struct IObject
{
    public partial void AttachToPlayer(IPlayer player, Vector3 offset, Vector3 rotation);
    public partial void AttachToObject(IObject objekt, Vector3 offset, Vector3 rotation, bool syncRotation);
}