﻿using System.Numerics;

namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// This type represents a pointer to an unmanaged open.mp <see cref="IPlayerObjectData" /> interface.
/// </summary>
[OpenMpApi(typeof(IExtension), typeof(IPool<IPlayerObject>))]
public readonly partial struct IPlayerObjectData
{
    /// <inheritdoc />
    public static UID ExtensionId => new(0x93d4ed2344b07456);

    public partial IPlayerObject Create(int modelID, Vector3 position, Vector3 rotation, float drawDist = 0);
    public partial void SetAttachedObject(int index, ref ObjectAttachmentSlotData data);
    public partial void RemoveAttachedObject(int index);
    public partial bool HasAttachedObject(int index);
    public partial ref ObjectAttachmentSlotData GetAttachedObject(int index);
    public partial void BeginSelecting();
    public partial bool SelectingObject();
    public partial void EndEditing();
    public partial void BeginEditing(IObject objekt);
    public partial void BeginEditing(IPlayerObject objekt);
    public partial bool EditingObject();
    public partial void EditAttachedObject(int index);

    public IPool<IPlayerObject> AsPool()
    {
        return (IPool<IPlayerObject>)this;
    }
}